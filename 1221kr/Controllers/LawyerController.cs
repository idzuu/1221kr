using System.IO;
using System.Web.Mvc;
using System.Web;
using System;
using System.Linq;
using System.Data.Entity;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using _1221kr.Models;
using static _1221kr.Models.Consultation;

public class LawyerController : Controller
{
    private ApplicationDbContext db = new ApplicationDbContext();

    // Метод для получения ID текущего пользователя
    private int GetCurrentUserId()
    {
        if (User.Identity.IsAuthenticated)
        {
            // Получаем claim с идентификатором пользователя
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userIdClaim = claimsIdentity.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
        }

        throw new UnauthorizedAccessException("Пользователь не аутентифицирован");
    }

    [Authorize]
    public ActionResult Profile()
    {
        var userId = User.Identity.GetUserId();
        var user = db.Users
                   .Include(u => u.Questionnaires)
                   .FirstOrDefault(u => u.Id.ToString() == userId);

        if (user == null)
        {
            return HttpNotFound();
        }
       
        ViewBag.Consultations = db.Consultations.Where(c => c.LawyerId == userId).ToList();
        return View(user);
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            db.Dispose();
        }
        base.Dispose(disposing);
    }
    public ActionResult UpdateProfile()
    {
        var userId = User.Identity.GetUserId();
        var user = db.Users.Find(userId);

        if (user == null)
        {
            return HttpNotFound();
        }

        return View(user);
    }
    // POST: /Lawyer/UpdateProfile
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult UpdateProfile(ApplicationUser model, HttpPostedFileBase photoFile)
    {
        if (ModelState.IsValid)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user == null)
            {
                return HttpNotFound();
            }

            // Обновляем данные
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Specialization = model.Specialization;
            user.Education = model.Education;
            user.Certifications = model.Certifications;
            user.About = model.About;

            // Обработка фото
            if (photoFile != null && photoFile.ContentLength > 0)
            {
                try
                {
                    // Создаем уникальное имя файла
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(photoFile.FileName);

                    // Определяем путь к папке Uploads
                    string uploadFolder = Server.MapPath("~/Content/Uploads");

                    // Создаем папку, если она не существует
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    // Полный путь для сохранения
                    string fullPath = Path.Combine(uploadFolder, fileName);

                    // Сохраняем файл
                    photoFile.SaveAs(fullPath);

                    // Сохраняем путь в БД (относительный путь)
                    user.PhotoPath = "/Content/Uploads/" + fileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при загрузке файла: " + ex.Message);
                    return View(model);
                }
            }

            db.SaveChanges();
            return RedirectToAction("Profile");
        }

        return View(model);
    }
}