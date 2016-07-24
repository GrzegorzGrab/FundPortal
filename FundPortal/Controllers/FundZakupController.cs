using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repozytorium.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System.Diagnostics;

namespace FundPortal.Controllers
{
   
    public class FundZakupController : Controller
    {
        private FundPortalContext db = new FundPortalContext();
        //protected UserManager<Uzytkownik> UserManager { get; set; }

        AccountController ac = new AccountController();

        // GET: /FundZakup/
        public async Task<ActionResult> Index(string fundName, string sortOrder, int? page, System.DateTime? zakupOd, System.DateTime? zakupDo)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_asc" : "";
            ViewBag.CenaSortParm = sortOrder == "Cena" ? "cena_desc" : "Cena";
            ViewBag.FundName = (from f in db.Funds
                                select f.FundName).Distinct();
            ViewBag.CurrentZakupOd = zakupOd;
            ViewBag.CurrentZakupDo = zakupDo;
            ViewBag.CurrentFundName = fundName;

            string userID = User.Identity.GetUserId();

            var fundZakup =  await db.FundZakup
                .Include(f => f.Fund)
                .Include(f => f.Uzytkownik)
                .Include(f => f.FundOplataTyp)
               // .Join w //(db.FundWycena, f=>f.FundID ,w=>w.FundID, (f.FundID, w.FundID)=> new { f.FundID=FundID,FundID=w.FundID })
               /*.Join(db.FundWaluta,
                    z => z.Fund.FundWalutaID,
                    w => w.FundWalutaID,
                    (z, w) => new { FundWaluta = z, Fund = w })*/
                .Where(f=>f.UzytkownikID==userID).ToListAsync();

            /*
            List < ZakupIndexData > lista= new List<ZakupIndexData>();

            var fundZakup =new ZakupIndexData();
            fundZakup.fundZakups =
                (from fundZak in db.FundZakup
                 select fundZak).FirstOrDefault();


            fundZakup.funds =
                (from fund in db.Funds
                 select fund).FirstOrDefault();

            lista.Add(fundZakup);
            */
            /*
            var fundZakup = new ZakupIndexData();
            fundZakup = new ZakupIndexData {
                 from fundZak in db.FundZakup
                join fund in db.Funds on fundZak.FundID equals fund.FundID
                join user in db.Uzytkownik on fundZak.UzytkownikID equals user.Id
                join wycena in
        //db.FundWycena
        //on FundZakup.FundID equals wycena.FundID        
        (
                        from w in db.FundWycena
                        group w by w.FundID into g
                        select new {GroupID = g.Key, MaxData = g.Max(x=>x.FundWycenaDate)}
                    )
                on fundZak.FundID equals wycena.GroupID 
                
                //where wycena.FundWycenaDate > '2015-12-31'
                // select new { fundZak.FundID, fundZak.FundZakupDataZakupu, fund.FundName,user.Email, wycena }
                //select  new ZakupIndexData { FundZakups= new List<FundZakup> { new FundZakup{ FundZakupID = fundZak.FundZakupID } } }
                select (fundZak, fund, user, wycena)
                }
               // select new {FundZakupCena=FundZakup.FundZakupCena, FundZakupDataZakupu=FundZakup.FundZakupDataZakupu, FundName=fund.FundName}
                ;
                */

            //GG_2016-04-08 : dodanie aktualnej ceny jednostki z wyceny

            

            int pageSize = 20;
            int pageNumber = (page ?? 1);

            fundZakup.Take(pageSize);
            if (!String.IsNullOrEmpty(fundName))
            {
                fundZakup = fundZakup.Where(n => n.Fund.FundName == fundName).ToList();
            }

            if (!String.IsNullOrEmpty(zakupOd.ToString()))
            {
                fundZakup = fundZakup.Where(n => n.FundZakupDataZakupu >= zakupOd).ToList();
            }

            if (!String.IsNullOrEmpty(zakupDo.ToString()))
            {
                fundZakup = fundZakup.Where(n => n.FundZakupDataZakupu <= zakupDo).ToList();
            }

            switch (sortOrder)
            {
                case "cena_desc":
                    fundZakup = fundZakup.OrderByDescending(z => z.FundZakupCena).ToList();
                    break;
                case "date_asc":
                    fundZakup =fundZakup.OrderBy(z => z.FundZakupDataZakupu).ToList();
                    break;
                case "Cena":
                    fundZakup = fundZakup.OrderBy(z => z.FundZakupCena).ToList();
                    break;
                default: //date descending
                    fundZakup = fundZakup.OrderByDescending(z => z.FundZakupDataZakupu).ToList();
                    break;
            }

            return View( fundZakup.ToPagedList(pageNumber,pageSize));

            
        }

        // GET: /FundZakup/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundZakup fundzakup = await db.FundZakup.FindAsync(id);
            
            if (fundzakup == null)
            {
                return HttpNotFound();
            }
            fundzakup.CurrentPrice = GetCurrentPrice(fundzakup.FundID);
            return View(fundzakup);
        }

        // GET: /FundZakup/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.FundID = new SelectList(db.Funds, "FundID", "FundName");
            string userID = User.Identity.GetUserId();
            ViewBag.UzytkownikID = userID;// new SelectList(db.Users.Where(u =>u.Id ==userID), "Nazwisko", "Imie");
            ViewBag.FundOplataTypID = new SelectList(db.FundOplatyTyp, "FundOplataTypID", "FundOplataTypNazwa");
           

            //ViewBag.UzytkownikID=userID;
            return View();
        }

        // POST: /FundZakup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create([Bind(Include=
            "FundZakupID,FundZakupIlosc,FundZakupCena,FundID,FundZakupDataZakupu,UzytkownikID,FundOplataTypID,FundOplataWysokosc")] FundZakup fundzakup)
        {
                if (ModelState.IsValid)
            {
                string userID = User.Identity.GetUserId();
                fundzakup.UzytkownikID = userID;
                db.FundZakup.Add(fundzakup);
                await db.SaveChangesAsync();
                TempData["CreateZakupSuccess"] = "Rejestracja nowych jednostek została zapisana";
                return RedirectToAction("Index");
            }

            ViewBag.FundID = new SelectList(db.Funds, "FundID", "FundName", fundzakup.FundID);
            
            ViewBag.UzytkownikID = new SelectList(db.Users, "Id", "Email", fundzakup.UzytkownikID);
            ViewBag.FundOplataTypID = new SelectList(db.FundOplatyTyp, "FundOplataTypID", "FundOplataTypNazwa");
            return View(fundzakup);
        }

        // GET: /FundZakup/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundZakup fundzakup = await db.FundZakup.FindAsync(id);
            if (fundzakup == null)
            {
                return HttpNotFound();
            }
            ViewBag.FundID = new SelectList(db.Funds, "FundID", "FundName", fundzakup.FundID);
            //ViewBag.UzytkownikID = new SelectList(db.Users, "Id", "Email", fundzakup.UzytkownikID);
            ViewBag.FundOplataTypID = new SelectList(db.FundOplatyTyp, "FundOplataTypID", "FundOplataTypNazwa", fundzakup.FundOplataTyp);
           
            return View(fundzakup);
        }

        // POST: /FundZakup/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include=
            "FundZakupID,FundZakupIlosc,FundZakupCena,FundID,FundZakupDataZakupu,UzytkownikID,FundZakupDataDodaniaZakupu,FundOplataTypID,FundOplataWysokosc")] FundZakup fundzakup)
        {
            if (ModelState.IsValid)
            {
                db.Database.Log = T => Debug.Write(T);
                string userID = User.Identity.GetUserId();
                fundzakup.UzytkownikID = userID;
                db.Entry(fundzakup).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["EditZakupSuccess"] = "Zmiany zostały zapisane";
                return RedirectToAction("Index");
            }
            ViewBag.FundID = new SelectList(db.Funds, "FundID", "FundName", fundzakup.FundID);
            ViewBag.UzytkownikID = new SelectList(db.Users, "Id", "Email", fundzakup.UzytkownikID);
            return View(fundzakup);
        }

        // GET: /FundZakup/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundZakup fundzakup = await db.FundZakup.FindAsync(id);
            if (fundzakup == null)
            {
                return HttpNotFound();
            }
            return View(fundzakup);
        }

        // POST: /FundZakup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            FundZakup fundzakup = await db.FundZakup.FindAsync(id);
            db.FundZakup.Remove(fundzakup);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet, ActionName("Main")]
        public async Task<ActionResult> Main()
        {
            string userID = User.Identity.GetUserId();
            var fundZakup = db.FundZakup.Include(f => f.Fund).Include(f => f.Uzytkownik).Where(f=>f.UzytkownikID==userID);
            return View(await fundZakup.ToListAsync());

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public decimal GetCurrentPrice(int? fundID)
        {

            /*
            var maxData =
                (
                    from fuw in db.FundWycena
                    where fuw.FundID == fundID
                   
                   select fuw.FundWycenaDate
                    
                ).Max();
                */
            var result =
                (
                from fundWy in db.FundWycena
                where fundWy.FundID ==fundID &&
                 fundWy.FundWycenaDate.Equals(
                     (
                    from fuw in db.FundWycena
                    where fuw.FundID == fundID

                    select fuw.FundWycenaDate

                ).Max()
                     )
                orderby fundWy.FundWycenaDate descending
                select fundWy.FundWycenaCena

            ).FirstOrDefault();

            result = System.Convert.ToDecimal(result);
            return result;
        }
    }
}
