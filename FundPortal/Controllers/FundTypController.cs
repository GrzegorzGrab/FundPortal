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
    public class FundTypController : Controller
    {
        private FundPortalContext db = new FundPortalContext();

        // GET: /FundTyp/
        public async Task<ActionResult> Index()
        {
            return View(await db.FundTyp.ToListAsync());
        }

        // GET: /FundTyp/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundTyp fundtyp = await db.FundTyp.FindAsync(id);
            if (fundtyp == null)
            {
                return HttpNotFound();
            }
            return View(fundtyp);
        }

        // GET: /FundTyp/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /FundTyp/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="FundTypID,FundTypName,FundTypOpis")] FundTyp fundtyp)
        {
            if (ModelState.IsValid)
            {
                fundtyp.FundTypName = fundtyp.FundTypName.ToUpper();
                db.FundTyp.Add(fundtyp);
                await db.SaveChangesAsync();
                TempData["Success"]= "Dodany nowy typ funduszu";
                return RedirectToAction("Index");
            }

            return View(fundtyp);
        }

        // GET: /FundTyp/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundTyp fundtyp = await db.FundTyp.FindAsync(id);
            if (fundtyp == null)
            {
                return HttpNotFound();
            }
            return View(fundtyp);
        }

        // POST: /FundTyp/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="FundTypID,FundTypName,FundTypOpis")] FundTyp fundtyp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fundtyp).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["Edit_Success"] = "Zmiany typu funduszu zostały zapisane";
                return RedirectToAction("Index");
            }
            return View(fundtyp);
        }

        // GET: /FundTyp/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundTyp fundtyp = await db.FundTyp.FindAsync(id);
            if (fundtyp == null)
            {
                return HttpNotFound();
            }
            return View(fundtyp);
        }

        // POST: /FundTyp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            FundTyp fundtyp = await db.FundTyp.FindAsync(id);
            db.FundTyp.Remove(fundtyp);
            await db.SaveChangesAsync();
            TempData["Delete_Success"] = "Typ funduszu został usunięty";
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
