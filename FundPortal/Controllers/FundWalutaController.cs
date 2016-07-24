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
using System.Diagnostics;

namespace FundPortal.Controllers
{
    [Authorize (Roles ="admin")]
    public class FundWalutaController : Controller
    {
        private FundPortalContext db = new FundPortalContext();

        // GET: /FundWaluta/
        public async Task<ActionResult> Index()
        { 
                return View(await db.FundWaluta.ToListAsync());
        }

        // GET: /FundWaluta/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundWaluta fundwaluta = await db.FundWaluta.FindAsync(id);
            if (fundwaluta == null)
            {
                return HttpNotFound();
            }
            return View(fundwaluta);
        }

        // GET: /FundWaluta/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /FundWaluta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FundWalutaID,FundWalutaKod,FundWalutaPelnaNazwa,FundWalutaDataDodania")] FundWaluta fundwaluta)
        {
            var kodWaluty = (from w in db.FundWaluta
                                 where w.FundWalutaKod == fundwaluta.FundWalutaKod
                                 select w ).SingleOrDefault();

            if ( kodWaluty!=null)
            {
                ViewBag.IstniejeTakiKodWaluty = "Istnieje już taki kod waluty";
                TempData["istniejeTakiKodWaluty"] = "Istnieje już taki kod waluty";
                return View(fundwaluta);
            }
            if (ModelState.IsValid)
            {
                db.FundWaluta.Add(fundwaluta);
                db.Database.Log = message => Trace.WriteLine(message);
                await db.SaveChangesAsync();
                //string msg = "Kod " + fundwaluta.FundWalutaKod + " został dodany.";
                TempData["kodZostalDodany"] = "Kod waluty: " + fundwaluta.FundWalutaKod.ToUpper() + " został dodany.";
                return RedirectToAction("Index");
            }

            return View(fundwaluta);
        }

        // GET: /FundWaluta/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundWaluta fundwaluta = await db.FundWaluta.FindAsync(id);
            if (fundwaluta == null)
            {
                return HttpNotFound();
            }
            return View(fundwaluta);
        }

        // POST: /FundWaluta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="FundWalutaID,FundWalutaKod,FundWalutaPelnaNazwa,FundWalutaDataDodania")] FundWaluta fundwaluta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fundwaluta).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["WalutaEditSuccess"] = "Zapisano zmiany w walucie";
                return RedirectToAction("Index");
            }
            return View(fundwaluta);
        }

        // GET: /FundWaluta/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundWaluta fundwaluta = await db.FundWaluta.FindAsync(id);
            if (fundwaluta == null)
            {
                return HttpNotFound();
            }
            return View(fundwaluta);
        }

        // POST: /FundWaluta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            FundWaluta fundwaluta = await db.FundWaluta.FindAsync(id);
            db.FundWaluta.Remove(fundwaluta);
            await db.SaveChangesAsync();
            TempData["WalutaDeleteSuccess"] = "Waluta została usunięta";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
