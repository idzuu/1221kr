using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _1221kr.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace _1221kr.Controllers
{
    public class FileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                var files = db.UploadedFiles.ToList();
                return View(files ?? new List<UploadedFile>());
            }
            catch (Exception ex)
            {
                
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return View(new List<UploadedFile>()); 
            }
        
    }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    if (file.ContentLength > 10 * 1024 * 1024)
                    {
                        ModelState.AddModelError("", "Файл слишком большой. Максимальный размер - 10MB.");
                        return View("Index");
                    }

                    var uploadedFile = new UploadedFile
                    {
                        FileName = Path.GetFileName(file.FileName),
                        ContentType = file.ContentType, 
                        Content = new byte[file.ContentLength]
                    };

                    file.InputStream.Read(uploadedFile.Content, 0, file.ContentLength);

                    db.UploadedFiles.Add(uploadedFile);
                    db.SaveChanges();
                    TempData["Message"] = "Файл успешно загружен.";
                }
                else
                {
                    ModelState.AddModelError("", "Пожалуйста, выберите файл для загрузки.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Произошла ошибка при загрузке файла: " + ex.Message);
            }

            return RedirectToAction("Index");
        }
        public ActionResult Download(int id)
        {
            var file = db.UploadedFiles.Find(id);
            if (file == null || file.Content == null)
                return HttpNotFound();

           
            string contentType = file.ContentType;
            if (string.IsNullOrEmpty(contentType))
            {
                contentType = MimeMapping.GetMimeMapping(file.FileName) ?? "application/octet-stream";
            }

            return File(file.Content, contentType, file.FileName);
        }
    }
}
