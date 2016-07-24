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
    public class FundOplataTypsController : Controller
    {
        private FundPortalContext db = new FundPortalContext();

        // GET: FundOplataTyps
        public async Task<ActionResult> Index()
        {
            return View(await db.FundOplatyTyp.ToListAsync());
        }

        // GET: FundOplataTyps/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundOplataTyp fundOplataTyp = await db.FundOplatyTyp.FindAsync(id);
            if (fundOplataTyp == null)
            {
                return HttpNotFound();
            }
            return View(fundOplataTyp);
        }

        // GET: FundOplataTyps/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FundOplataTyps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FundOplataTypID,FundOplataTypNazwa,FundOplataTypOpis")] FundOplataTyp fundOplataTyp)
        {
            //sprawdzenie czy podana nazwa typu opłaty nie istnieje w bazie
            var typOplaty = (from tO in db.FundOplatyTyp
                            where tO.FundOplataTypNazwa == fundOplataTyp.FundOplataTypNazwa
                            select tO.FundOplataTypNazwa).FirstOrDefault();
            //jeżeli taki typ oplaty został już wprowadzony do bazy wróć widok
            if (typOplaty != null)
            {
                TempData["NazwaOplatyIstnieje"] = "Podana nazwa oplaty już istnieje";
                return View();
            }

            if (ModelState.IsValid)
            {
                db.FundOplatyTyp.Add(fundOplataTyp);
                await db.SaveChangesAsync();
                TempData["Success"] = "Nowa opłata została dodana";
                return RedirectToAction("Index");
            }

            return View(fundOplataTyp);
        }

        // GET: FundOplataTyps/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundOplataTyp fundOplataTyp = await db.FundOplatyTyp.FindAsync(id);
            if (fundOplataTyp == null)
            {
                return HttpNotFound();
            }
            return View(fundOplataTyp);
        }

        // POST: FundOplataTyps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "FundOplataTypID,FundOplataTypNazwa,FundOplataTypOpis,FundOplataTypDataDodania")] FundOplataTyp fundOplataTyp)
        {

            //sprawdzenie czy podana nowa nazwa opłaty nie jest już wprowadzona
            try { 
                if (ModelState.IsValid)
                {
                    db.Entry(fundOplataTyp).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["OplataEdited"] = "Typ opłaty został zmieniony";
                    return RedirectToAction("Index");
                }
                return View(fundOplataTyp);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("FundOplataTypNazwa", "Podana nazwa opłaty została wprowadzna dla innej opłaty. Proszę podać inną nazwę opłaty");
                return View();
            }
        }

        // GET: FundOplataTyps/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundOplataTyp fundOplataTyp = await db.FundOplatyTyp.FindAsync(id);
            if (fundOplataTyp == null)
            {
                return HttpNotFound();
            }
            return View(fundOplataTyp);
        }

        // POST: FundOplataTyps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try { 
                FundOplataTyp fundOplataTyp = await db.FundOplatyTyp.FindAsync(id);
                db.FundOplatyTyp.Remove(fundOplataTyp);
                await db.SaveChangesAsync();
                TempData["OplataZostalaUsunieta"] = "Opłata została usunięta";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["NieMoznaUsunacOplaty"] = "Oplata nie może zostać usunięta: " + ex.Message;
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
    }
}
