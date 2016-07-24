using FundPortal.Models;
using Microsoft.AspNet.Identity;
using Repozytorium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FundPortal.Controllers
{
    public class FundsSummaryHelperController : Controller
    {

        private FundPortalContext db = new FundPortalContext();
        private AccountController ac = new AccountController();

        // GET: FundsSummaryHelper
        public ActionResult FundsSummaryGraph()
        {
            string userID = User.Identity.GetUserId();

            var fundSum = (from x in db.FundZakup
                           where x.UzytkownikID == userID
                           group x by x.UzytkownikID into grp
                           select new
                           {
                               fundsSum = grp.Sum(x => x.FundZakupCena)
                           }).ToList()
                           .Select(item => new FundsSummaryViewModel
                           {
                               FundSummarySumAllFund = item.fundsSum
                           }).ToList();
            return View(fundSum);
        }
    }
}