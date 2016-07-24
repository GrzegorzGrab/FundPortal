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

namespace FundPortal.Controllers
{
    [Authorize(Roles = "admin")]
    public class FundController : Controller
    {
        private FundPortalContext db = new FundPortalContext();

        // GET: /Fund/
        public async Task<ActionResult> Index()
        {
            var funds = db.Funds.Include(d=>d.FundTyp).Include(t=>t.FundTowarzystwo);
            return View(await db.Funds.ToListAsync());
        }

        // GET: /Fund/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fund fund = await db.Funds.FindAsync(id);
            if (fund == null)
            {
                return HttpNotFound();
            }
            return View(fund);
        }

        // GET: /Fund/Create
        public ActionResult Create()
        {
            PopulateFundTypsDropDownList();
            PopulateFundTowarzystwaDropDownList();
            PopulateFundWalutyDropDownList();
            return View();
        }

        // POST: /Fund/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include= "FundID,FundName,FundSymbol,FundTypID,FundTowarzystwoID,FundWalutaID,FundZarzadzajacy,FundDataUruchomienia,FundPierwszajWplata,"
            + "FundNastepnaWplata,FundPolityka")] Fund fund)
        {
            PopulateFundTypsDropDownList();
            PopulateFundTowarzystwaDropDownList();
            PopulateFundWalutyDropDownList();
            if (fund.FundTowarzystwoID==0)
            {
                ModelState.AddModelError("FundTowarzystwo", "Proszę wybrać towarzystwo");
            }

            fund.FundName = fund.FundName.ToUpper();
            //GG_2016-04-08 uzupełnienie brakującej obsługi Model.State.IsValid  ==false

            //sprawdzenie czy dana nazwa funduszu została już wykorzystana
            var fundName = (from x in db.Funds
                            where x.FundName == fund.FundName
                            select x.FundName).FirstOrDefault();

            if (!String.IsNullOrEmpty(fundName))
            {
                ModelState.AddModelError("FundName", "Podana nazwa funduszu jest już wprowadzona w bazie.");
            }

            //sprawdzenie czy dany symbol funduszu został już wprowadzony
            var fundSymbolName = (from x in db.Funds
                                  where x.FundSymbol == fund.FundSymbol
                                  select x.FundSymbol).FirstOrDefault();
            if (!String.IsNullOrEmpty(fundSymbolName))
            {
                ModelState.AddModelError("FundSymbol", "Podany symbol funduszu został już wcześniej wprowadzony w bazie.");
            }

            if (ModelState.IsValid)
            {
                db.Funds.Add(fund);
                await db.SaveChangesAsync();
                TempData["fundZostalDodany"] = "Fundusz: " + fund.FundName.ToUpper() + " został dodany.";
                return RedirectToAction("Index");
            }

            return View(fund);
        }

        // GET: /Fund/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fund fund = await db.Funds.FindAsync(id);
            if (fund == null)
            {
                return HttpNotFound();
            }
            PopulateFundTypsDropDownList(fund.FundTypID);
            PopulateFundTowarzystwaDropDownList(fund.FundTowarzystwoID);
            PopulateFundWalutyDropDownList(fund.FundWalutaID);
            return View(fund);
        }

        // POST: /Fund/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include= "FundID,FundName,FundSymbol,FundTypID,FundTowarzystwoID,FundWalutaID,FundZarzadzajacy,FundDataUruchomienia,FundPierwszajWplata,"
            + "FundNastepnaWplata,FundPolityka")] Fund fund)
        {
            if (ModelState.IsValid)
            {
                //sprawdzenie czy FundSymbol jest unikalny
                var fundIDSymbol = (from funds in db.Funds
                                    where funds.FundSymbol == fund.FundSymbol
                                    where funds.FundID != fund.FundID
                                    select funds.FundID).FirstOrDefault();

                //sprawdzenie czy nazwa funduszu jest już wykorzytana
                var fundIDName = (from funds in db.Funds
                                  where funds.FundName == fund.FundName
                                  where funds.FundID != fund.FundID
                                  select funds.FundID).FirstOrDefault();

                if (fundIDSymbol == 0 && fundIDName==0)
                {
                    db.Entry(fund).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    if (fundIDSymbol != 0) //jeżeli symbol jest wykorzystany
                    { 
                        TempData["FundSymbolExists"] = "Podany symbol jest już wykorzystany dla innego funduszu";
                    }
                    else //jeżeli FundName jest już wykorsztane
                    {
                        TempData["FundNameExists"] = "Podana nazwa funduszu jest już wykorzystana dla innego funduszu";
                    }
                }
            }
            PopulateFundTypsDropDownList(fund.FundTypID);
            PopulateFundTowarzystwaDropDownList(fund.FundTowarzystwoID);
            PopulateFundWalutyDropDownList(fund.FundWalutaID);
            return View(fund);
        }

        // GET: /Fund/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fund fund = await db.Funds.FindAsync(id);
            if (fund == null)
            {
                return HttpNotFound();
            }
            return View(fund);
        }

        // POST: /Fund/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            { 
                Fund fund = await db.Funds.FindAsync(id);
                db.Funds.Remove(fund);
                await db.SaveChangesAsync();
                TempData["FundDeleted"] = "Fundusz został usunięty z bazy!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["DeleteFailed"] = "Nie udało się usunąć funduszu." + e.Message;
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void PopulateFundTypsDropDownList(object selectedFundTyp = null)
        {
            var fundTypsQuery = from f in db.FundTyp
                               orderby f.FundTypName
                               select f;
            ViewBag.FundTypId = new SelectList(fundTypsQuery, "FundTypID", "FundTypName", selectedFundTyp);  
        }

        private void PopulateFundTowarzystwaDropDownList(object selectedTowarzystwo = null)
        {
            var fundTowarzystwoQuery = from t in db.FundTowarzystwa
                                       orderby t.FundTowarzystwoNazwa
                                       select t;
            ViewBag.FundTowarzystwoId = new SelectList(fundTowarzystwoQuery, "FundTowarzystwoID", "FundTowarzystwoNazwa", selectedTowarzystwo);
        }

        private void PopulateFundWalutyDropDownList(object selectedTowarzystwo = null)
        {
            var fundWalutaQuery = from w in db.FundWaluta
                                  orderby w.FundWalutaPelnaNazwa
                                  select w;
            ViewBag.FundWalutaId = new SelectList(fundWalutaQuery, "FundWalutaID", "FundWalutaPelnaNazwa", selectedTowarzystwo);
        }
    }
}
