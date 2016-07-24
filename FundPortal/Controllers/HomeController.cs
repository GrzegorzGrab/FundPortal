using Repozytorium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FundPortal.Controllers
{
    public class HomeController : Controller
    {

        private FundPortalContext db = new FundPortalContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult>Create([Bind(Include = "FundMessagesID,UsernName,UserEmail,SentDate,Message")]FundMessages fm)
        {
            if (ModelState.IsValid)
            {
                db.FundMessages.Add(fm);
                await db.SaveChangesAsync();
                TempData["WiadomoscZostalaWyslana"] = "Wiadomość została wysłana. Dziękujemy za poświęcony czas!";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}