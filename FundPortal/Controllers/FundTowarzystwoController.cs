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
    public class FundTowarzystwoController : Controller
    {
        private FundPortalContext db = new FundPortalContext();

        // GET: /FundTowarzystwo/
        public async Task<ActionResult> Index()
        {
            return View(await db.FundTowarzystwa.ToListAsync());
        }

        // GET: /FundTowarzystwo/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundTowarzystwo fundtowarzystwo = await db.FundTowarzystwa.FindAsync(id);
            if (fundtowarzystwo == null)
            {
                return HttpNotFound();
            }
            return View(fundtowarzystwo);
        }

        // GET: /FundTowarzystwo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /FundTowarzystwo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include= "FundTowarzystwoID,FundTowarzystwoNazwa,FundTowarzystwoAdresUrl,FundTowarzystwoDataDodania")] FundTowarzystwo fundtowarzystwo)
        {
            if (ModelState.IsValid)
            {
                fundtowarzystwo.FundTowarzystwoNazwa = fundtowarzystwo.FundTowarzystwoNazwa.ToUpper();
                db.FundTowarzystwa.Add(fundtowarzystwo);
                await db.SaveChangesAsync();
                TempData["TowarzystwoDodanoSukces"] = "Nowe towarzystwo zostało dodane";
                return RedirectToAction("Index");
            }

            return View(fundtowarzystwo);
        }

        // GET: /FundTowarzystwo/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundTowarzystwo fundtowarzystwo = await db.FundTowarzystwa.FindAsync(id);
            if (fundtowarzystwo == null)
            {
                return HttpNotFound();
            }
            return View(fundtowarzystwo);
        }

        // POST: /FundTowarzystwo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include= "FundTowarzystwoID,FundTowarzystwoNazwa,FundTowarzystwoAdresUrl,FundTowarzystwoDataDodania")] FundTowarzystwo fundtowarzystwo)
        {
            if (ModelState.IsValid)
            {
                fundtowarzystwo.FundTowarzystwoNazwa = fundtowarzystwo.FundTowarzystwoNazwa.ToUpper();
                db.Entry(fundtowarzystwo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["TowarzystwoEditSuccess"] = "Pomyślnie zapisano zmianę w towarzystwie";
                return RedirectToAction("Index");
            }
            return View(fundtowarzystwo);
        }

        // GET: /FundTowarzystwo/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundTowarzystwo fundtowarzystwo = await db.FundTowarzystwa.FindAsync(id);
            if (fundtowarzystwo == null)
            {
                return HttpNotFound();
            }
            return View(fundtowarzystwo);
        }

        // POST: /FundTowarzystwo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            FundTowarzystwo fundtowarzystwo = await db.FundTowarzystwa.FindAsync(id);
            db.FundTowarzystwa.Remove(fundtowarzystwo);
            await db.SaveChangesAsync();
            TempData["TowarzystwoDeleteSuccess"] = "Towarzystwo zostało usunięte";
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
