using FundPortal.Models;
using Repozytorium.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FundPortal.Controllers
{
    public class FundListController : Controller
    {
        private FundPortalContext db = new FundPortalContext();

        // GET: FundList
        public async Task<ActionResult> Index()
        {
            /*
            var funds = db.Funds
                    .Include(d => d.FundTyp)
                    .Include(t => t.FundTowarzystwo).ToListAsync();
                    */

            var funds = (
                from f in db.Funds
                join d in db.FundTyp on f.FundTypID equals d.FundTypID
                join t in db.FundTowarzystwa on f.FundTowarzystwoID equals t.FundTowarzystwoID
                select new  FundsListViewModel
                {
                    FundListFundID = f.FundID,
                    FundListFundName = f.FundName,
                    FundListFundSymbol = f.FundSymbol,
                    FundListTypName = d.FundTypName,
                    FundListTowarzystwoName = t.FundTowarzystwoNazwa,
                    FundListFundTowarzystwoAdresUrl = t.FundTowarzystwoAdresUrl
                }
                ).ToList();
            //uwzględnienie aktualnej wyceny funduszu
            string query = "SELECT DataWycenyNajnowsza, FundWycenaID,FundWycenaCena,FundID,FundName FROM vFundWycenaAktualne;";
            var fundsCurrentPrice = db.Database.SqlQuery<FundsCurrentPrice>(query);
            fundsCurrentPrice.ToList();

            funds = (from f in funds
                     join z in fundsCurrentPrice on f.FundListFundID equals z.FundId into currentPrice
                     from z in currentPrice.DefaultIfEmpty(new FundsCurrentPrice(f.FundListFundName))
                     select new FundsListViewModel
                     {
                         FundListFundID = f.FundListFundID,
                        FundListFundName= f.FundListFundName,
                         FundListFundSymbol = f.FundListFundSymbol,
                        FundListTypName = f.FundListTypName,
                         FundListTowarzystwoName=f.FundListTowarzystwoName,
                         FundListFundTowarzystwoAdresUrl = f.FundListFundTowarzystwoAdresUrl,
                         DataWyceny = z.DataWycenyNajnowsza,
                         FundListCurrentPrice = z.FundWycenaCena 
                     }
                     ).ToList();


            return View(funds);
        }

        //GET Details

        public async Task<ActionResult> Details (int? fundListFundID)
        {
            if (fundListFundID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var fundListDetail = (
                from f in db.Funds
                join d in db.FundTyp on f.FundTypID equals d.FundTypID
                join t in db.FundTowarzystwa on f.FundTowarzystwoID equals t.FundTowarzystwoID
               // join l in db.FundFile on f.FundFileID==null?f.FundFileID:0 equals l.FundFileID==null?l.FundFileID:0 
                where f.FundID == fundListFundID
                select new FundsListViewModel
                {
                    FundListFundID = f.FundID,
                    FundListFundName = f.FundName,
                    FundListFundSymbol = f.FundSymbol,
                    FundListTypName = d.FundTypName,
                    FundListTowarzystwoName = t.FundTowarzystwoNazwa,
                    //FundListFileName = l.FundFileName,
                    //FundListFundContent = l.FundContent,
                    FundListFundFileID = f.FundFileID,
                    FundListZarzadzajacy = f.FundZarzadzajacy,
                    FundListDataUruchomienia = f.FundDataUruchomienia,
                    FundListPierwszajWplata = f.FundPierwszajWplata,
                    FundListNastepnaWplata = f.FundNastepnaWplata,
                    FundListPolityka = f.FundPolityka
                }
                ).FirstOrDefault();

            string query = "SELECT DataWycenyNajnowsza, FundWycenaID,FundWycenaCena,FundID,FundName FROM vFundWycenaAktualne;";
            var fundsCurrentPrice = db.Database.SqlQuery<FundsCurrentPrice>(query);
            fundsCurrentPrice.ToList();


            fundListDetail.FundListCurrentPrice = (
                                                   from z in fundsCurrentPrice
                                                   where z.FundId == fundListDetail.FundListFundID
                                                   select z.FundWycenaCena
            ).FirstOrDefault();

            fundListDetail.DataWyceny = (
                    from z in fundsCurrentPrice
                    where z.FundId == fundListDetail.FundListFundID
                    select z.DataWycenyNajnowsza
                ).FirstOrDefault();

            if (fundListDetail.FundListFundFileID !=null)
            {
                FundFile recordFile = new FundFile();
                recordFile = db.FundFile.Find(fundListDetail.FundListFundFileID);
                fundListDetail.FundListFileName = recordFile.FundFileName;
                fundListDetail.FundListFundContent = recordFile.FundContent;                  
            }
            /*
            fundListDetail = (from f in fundListDetail
                              join z in fundsCurrentPrice on f.FundListFundID equals z.FundId into currentPrice
                              where f.FundListFundID == id
                              from z in currentPrice.DefaultIfEmpty(new FundsCurrentPrice(f.FundListFundName))
                              select new FundsListViewModel
                              {
                                  FundListFundName = f.FundListFundName,
                                  FundListFundSymbol = f.FundListFundSymbol,
                                  FundListTypName = f.FundListTypName,
                                  FundListTowarzystwoName = f.FundListTowarzystwoName,
                                  DataWyceny = z.DataWycenyNajnowsza,
                                  FundListCurrentPrice = z.FundWycenaCena
                              }
                     ).ToList();
                     */
            return View(fundListDetail);
        }
    }
}