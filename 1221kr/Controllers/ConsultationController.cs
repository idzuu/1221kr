using _1221kr.Models;
using _1221kr.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace _1221kr.Controllers
{

    public class ConsultationController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ApplicationUserManager _userManager;

        public ConsultationController()
        {
            _db = new ApplicationDbContext();
            _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_db));
        }


        public ActionResult Create()
        {
            return View(new ConsultationViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ConsultationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var user = await _userManager.FindByIdAsync(userId);

                var consultation = new Consultation
                {
                    Title = model.Title,
                    Description = model.Description,
                    Price = model.Price,
                    DurationMinutes = model.DurationMinutes,
                    Type = model.Type,
                    AvailableDays = model.AvailableDaysJson,
                    WorkingHours = model.WorkingHours,
                    LawyerId = userId,
                    CreatedAt = DateTime.Now
                };

                _db.Consultations.Add(consultation);
                await _db.SaveChangesAsync();

                return RedirectToAction("Profile", "Lawyer");
            }

         
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
                _userManager.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var consultation = _db.Consultations.Find(id);
            if (consultation != null)
            {
                _db.Consultations.Remove(consultation);
                _db.SaveChanges();
            }
            return RedirectToAction("Profile", "Lawyer");
        }
        
        public ActionResult AllConsultations()
        {
            var consultations = _db.Consultations
                .Include(c => c.Lawyer)  
                .Where(c => c.IsActive)  
                .OrderByDescending(c => c.CreatedAt)  
                .ToList();

            return View(consultations);
        }
        public ActionResult Details(int id)
        {
            var consultation = _db.Consultations
                .Include(c => c.Lawyer)
                .FirstOrDefault(c => c.Id == id);

            if (consultation == null)
            {
                return HttpNotFound();
            }

            var model = new ConsultationDetailsViewModel
            {
                Consultation = consultation,
                BookingModel = new _1221kr.Models.ConsultationBooking { ConsultationId = id }
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookConsultation(_1221kr.Models.ConsultationBooking model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Создаем новый объект бронирования
                    var booking = new _1221kr.Models.ConsultationBooking
                    {
                        ConsultationId = model.ConsultationId,
                        ClientId = User.Identity.GetUserId(),
                        SelectedDate = model.SelectedDate,
                        Notes = model.Notes,
                        BookingDate = DateTime.Now,
                        Status = _1221kr.Models.BookingStatus.Pending
                    };

                    // Явное указание типа при добавлении
                    _db.Set<_1221kr.Models.ConsultationBooking>().Add(booking);
                    _db.SaveChanges();

                    return RedirectToAction("BookingConfirmation", new { id = booking.Id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при сохранении: " + ex.Message);
                    // Логирование ошибки
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }

            // Повторная загрузка консультации для отображения формы
            var consultation = _db.Consultations
                .Include(c => c.Lawyer)
                .FirstOrDefault(c => c.Id == model.ConsultationId);

            if (consultation == null)
            {
                return HttpNotFound();
            }

            var viewModel = new ConsultationDetailsViewModel
            {
                Consultation = consultation,
                BookingModel = model
            };

            return View("Details", viewModel);
        }
        [HttpGet]
        public ActionResult BookConsultation(int consultationId)
        {
            var consultation = _db.Consultations
                .Include(c => c.Lawyer)
                .FirstOrDefault(c => c.Id == consultationId);

            if (consultation == null)
            {
                return HttpNotFound();
            }

            var model = new ConsultationBooking
            {
                ConsultationId = consultationId,
                Consultation = consultation
            };

            return View(model);
        }
        [HttpGet]
        public ActionResult BookingConfirmation(int id)
        {
            var booking = _db.ConsultationBookings
                .Include(b => b.Consultation)
                .Include(b => b.Consultation.Lawyer)
                .FirstOrDefault(b => b.Id == id);

            if (booking == null)
            {
                return HttpNotFound();
            }

            return View(booking);
        }

    }
}
