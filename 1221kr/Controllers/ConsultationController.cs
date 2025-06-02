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
                Consultation = consultation,
                ConsultationId = consultation.Id, 
                MinDate = DateTime.Now.AddDays(1),
                MaxDate = DateTime.Now.AddDays(14)
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookConsultation(ConsultationBooking model)
        {
            // Всегда загружаем Consultation перед возвратом View
            model.Consultation = _db.Consultations
                .Include(c => c.Lawyer)
                .FirstOrDefault(c => c.Id == model.ConsultationId);

            if (model.Consultation == null)
            {
                return HttpNotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var existingBooking = _db.ConsultationBookings
                    .Any(b => b.ConsultationId == model.ConsultationId &&
                             b.SelectedDate == model.SelectedDate);

                if (existingBooking)
                {
                    ModelState.AddModelError("SelectedDate", "Это время уже занято");
                    return View(model);
                }

                var booking = new ConsultationBooking
                {
                    ConsultationId = model.ConsultationId,
                    ClientId = User.Identity.GetUserId(),
                    SelectedDate = model.SelectedDate,
                    Notes = model.Notes,
                    BookingDate = DateTime.Now,
                    Status = BookingStatus.Confirmed
                };

                _db.ConsultationBookings.Add(booking);
                _db.SaveChanges();

                TempData["BookingSuccess"] = true;
                return RedirectToAction("BookingConfirmation", new { id = booking.Id });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при бронировании: {ex}");
                ModelState.AddModelError("", "Произошла ошибка при бронировании. Пожалуйста, попробуйте позже.");
                return View(model);
            }
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
