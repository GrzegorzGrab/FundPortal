using Microsoft.AspNet.Identity;
using Repozytorium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FundPortal.Models;
using System.Threading.Tasks;
using System.Data.Entity;

namespace FundPortal.Controllers
{
    public class FundsSummaryController : Controller
    {
        private FundPortalContext db = new FundPortalContext();
        private AccountController ac = new AccountController();

        // GET: FundsSummary
        public ActionResult FundsSummaryGraph(string fundName)
        {

            string userID = User.Identity.GetUserId();


            // //2016-04-07;GG: utowrzene na potrzeby dodania aktualnej wyceny

            /* var fundsCurrentPrice = (from w in db.FundWycena
                                     where w.FundWycenaDate == (from z in db.FundWycena where z.FundID == w.FundID
                                                                group z by z.FundWycenaDate into grp
                                                                select new
                                                                { FundMaxDate = grp.Max(z => z.FundWycenaDate) }
                                     )

                                    .Select (w.FundID)    )        ;
                                    */
            string query = "SELECT DataWycenyNajnowsza, FundWycenaID,FundWycenaCena,FundID,FundName FROM vFundWycenaAktualne;";
            //string query = "select FundOplataTypNazwa from FundOplataTyp";
            var fundsCurrentPrice = db.Database.SqlQuery<FundsCurrentPrice>(query);
            fundsCurrentPrice.ToList();

            var fundSummary = (from x in db.FundZakup
                               join f in db.Funds on x.FundID equals f.FundID
                               //join u in db.Uzytkownik on x.UzytkownikID equals u.Id
                               where x.UzytkownikID == userID
                               group x by f.FundName into grp
                               select new
                               { FundNaz = grp.Key,
                                   FundPrice = grp.Average(x => x.FundZakupCena),
                                   FundMinPrice = grp.Min(x => x.FundZakupCena),
                                   FundMaxPrice = grp.Max(x => x.FundZakupCena),
                                   FundSumJednostek = grp.Sum(x => x.FundZakupIlosc)
                               }).ToList()
                .Select(item => new FundsSummaryViewModel
                {
                    FundSummaryNazwa = item.FundNaz,
                    FundSummaryAvgPrice = item.FundPrice,
                    FundSummaryMinPrice = item.FundMinPrice,
                    FundSummaryMaxPrice = item.FundMaxPrice,
                    FundSummarySumJednostek = item.FundSumJednostek
                }).ToList();


            fundSummary = (from x in fundSummary
                           join z in fundsCurrentPrice on x.FundSummaryNazwa equals z.FundName into currentPrice
                           from z in currentPrice.DefaultIfEmpty(new FundsCurrentPrice(x.FundSummaryNazwa))
                           select new FundsSummaryViewModel
                           {
                               FundSummaryNazwa = x.FundSummaryNazwa,
                               FundSummaryAvgPrice = x.FundSummaryAvgPrice,
                               FundSummaryMinPrice = x.FundSummaryMinPrice,
                               FundSummaryMaxPrice = x.FundSummaryMaxPrice,
                               FundSummarySumJednostek = x.FundSummarySumJednostek,
                               // CurrentPrice = String.IsNullOrEmpty(z.FundWycenaCena.ToString())?1:z.FundWycenaCena
                               CurrentPrice =z.FundWycenaCena
                           }).ToList();

            return PartialView(fundSummary);
        }

        
        public ActionResult Index ()
        {
            string userID = User.Identity.GetUserId();

            var fundSumPerFund = (from x in db.FundZakup
                           join f in db.Funds on x.FundID equals f.FundID
                           where x.UzytkownikID == userID
                           group x by f.FundName into grp
                           select new
                           {
                               fundsName= grp.Key,
                               fundsSum = grp.Sum(x => x.FundZakupCena),
                               fundsUnitsCount=grp.Sum(x=>x.FundZakupIlosc)
                           }).ToList()
                           .Select(item => new FundsSummaryViewModel
                           {
                               FundSummaryNazwa = item.fundsName,
                               FundSummarySumAllFund = item.fundsSum  * item.fundsUnitsCount
                           }).ToList();

            decimal sumOfFunds = fundSumPerFund.AsEnumerable().Sum(o => o.FundSummarySumAllFund);

            var selectedItems = new Dictionary<string, decimal> { };

            foreach (var k in fundSumPerFund)
            {
                selectedItems.Add(k.FundSummaryNazwa, ( decimal.Round((k.FundSummarySumAllFund/sumOfFunds*100),2,MidpointRounding.AwayFromZero)));
            }

            

             //return PartialView(fundSum);
            return View(selectedItems);
        }


        //GG: na próbe wykresy dla wyceny
        public JsonResult FundWycenaGraphJson (string fundName, DateTime? startDate, DateTime? endDate)
        {
            //DateTime startDate = new DateTime(2000, 1, 1);
            endDate = endDate == null ? new DateTime(2099, 12, 31) : endDate;
            IEnumerable<FundWycena> wycenaData = db.FundWycena.Include(z => z.Fund)
                .Where(z => z.Fund.FundName == fundName)
                .Where(f=>f.FundWycenaDate >= startDate)
                .Where(f=>f.FundWycenaDate <= endDate);

            var data = wycenaData.Select(f => new
            {
                FundName = f.Fund.FundName,
                FundWycenaDate = f.FundWycenaDate.ToShortDateString(),
                FundWycenaCena = f.FundWycenaCena,
                FundWycenaCenaMin = f.FundWycenaCenaMin,
                FundWycenaCenaMax = f.FundWycenaCenaMax
            })
            .OrderBy (x=>x.FundWycenaDate);
            return Json(data, JsonRequestBehavior.AllowGet);
            
            /*
            var fundwycena =  db.FundWycena.Include(z => z.Fund)
                .Where(z=>z.Fund.FundName==fundName)
                .ToList();

            //var selectedItems =new Dictionary<string, DateTime, decimal> { };
            var selectedItems = new[]
            {
               new {label="FundName", data=fundwycena.Select(f=> new string[] { f.Fund.FundName })},
               new {label="FundID",data=fundwycena.Select(f=> new string[] {f.FundID.ToString()})}
               //new {label="USA", data="AE"},
               //new {label="USA", data="AE"}
            };

            ViewData["Wycena"] = selectedItems;
            //return View(selectedItems);
            return Json(selectedItems,JsonRequestBehavior.AllowGet);
            */
        }

        public PartialViewResult FundWycenaGraphData(string fundName = "GGTEST")
        {
            IEnumerable<FundWycena> wycenaData = db.FundWycena.Include(z => z.Fund)
                .Where(z => z.Fund.FundName == fundName);
            return PartialView(wycenaData);
        }

        public ActionResult FundWycenaGraph(string fundName = "GGTEST")
        {
            ViewBag.FundsName=  (from f in db.Funds
                                  select f.FundName).Distinct();
            return View((object)fundName);
        }

        public ActionResult FundsSummaryGraphGGTEST(string fundName)
        {

            string userID = User.Identity.GetUserId();

            var fundSummary = (from x in db.FundZakup
                               join f in db.Funds on x.FundID equals f.FundID
                               //join u in db.Uzytkownik on x.UzytkownikID equals u.Id
                               where x.UzytkownikID == userID
                               where x.Fund.FundName==fundName
                               group x by f.FundName into grp
                               select new
                               {
                                   FundNaz = grp.Key,
                                   FundPrice = grp.Average(x => x.FundZakupCena),
                                   FundMinPrice = grp.Min(x => x.FundZakupCena),
                                   FundMaxPrice = grp.Max(x => x.FundZakupCena),
                                   FundSumJednostek = grp.Sum(x => x.FundZakupIlosc)
                               }).ToList()
                .Select(item => new FundsSummaryViewModel
                {
                    FundSummaryNazwa = item.FundNaz,
                    FundSummaryAvgPrice = item.FundPrice,
                    FundSummaryMinPrice = item.FundMinPrice,
                    FundSummaryMaxPrice = item.FundMaxPrice,
                    FundSummarySumJednostek = item.FundSumJednostek
                }).ToList();

            return PartialView(fundSummary);
        }
    }
}