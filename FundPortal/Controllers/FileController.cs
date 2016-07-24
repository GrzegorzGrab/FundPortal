using FundPortal.Models;
using Repozytorium.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FundPortal.Controllers
{
    public class FileController : Controller
    {
        private FundPortalContext db = new FundPortalContext();
        // GET: File
        [Authorize(Roles = "admin")]
        public ActionResult Index(int? fundId)
        {
            var model = new FileViewModel();
            model.FundId = fundId;
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Index(FileViewModel model)
        {
            //sprawdzenie czy został wybrany jakiś plik
            if (model.File == null)
            {
                TempData["NieWybranoPliku"] = "Nie wybrano żadnego pliku. Proszę wybrać plik i spróbować podobnie!";
                return View(model);
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            FundFile fundFile = new FundFile();
            byte[] uploadFile = new byte[model.File.InputStream.Length];
            model.File.InputStream.Read(uploadFile, 0, uploadFile.Length);

            fundFile.FundFileName = model.File.FileName;
            fundFile.FundType = model.File.ContentType;
            fundFile.FundContent = uploadFile;

            db.FundFile.Add(fundFile);
            db.SaveChanges();
            var fundFileLastId = db.FundFile.OrderByDescending(f => f.FundFileID).Take(1).Select(f => f.FundFileID).FirstOrDefault();
            Fund fund = new Fund();
            fund = db.Funds.Find(model.FundId);
            fund.FundFileID = fundFileLastId;
            db.Database.Log = T => Debug.Write(T);
            //db.Entry(fund).State = EntityState.Modified;
            //db.Funds.SqlQuery("Fund_Update", fund);
            db.SaveChanges();

            return Content("File Uploaded");
        }

        public ActionResult DownloadFile()
        {
            return View(db.FundFile.ToList());
        }

        public FileContentResult FileDownload(int? id)
        {
            
            byte[] fileData;
            string fileName;
            FundFile fileRecord = db.FundFile.Find(id);
            //sprawdzenie czy jest taki plik
            if (fileRecord != null)
            {
                fileData = (byte[])fileRecord.FundContent.ToArray();
                fileName = fileRecord.FundFileName;
                return File(fileData, "text", fileName);
            }
            else
            {
                return null;
            }
        }
    }
}