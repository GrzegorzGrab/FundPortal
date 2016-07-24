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
using PagedList;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Text;

namespace FundPortal.Controllers
{
    [Authorize(Roles = "admin")]
    public class FundWycenaController : Controller
    {
        private FundPortalContext db = new FundPortalContext();

        // GET: /FundWycena/
        //public ViewResult Index(int? page)
        public async Task<ActionResult> Index(string fundName, string sortOrder, int? page, System.DateTime? wycenaOd, System.DateTime? wycenaDo)
        //
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_asc" : "";
            ViewBag.CenaSortParm = sortOrder == "Cena" ? "cena_desc" : "Cena";

            ViewBag.CurrentWycenaOd = wycenaOd;

            ViewBag.CurrentWycenDo = wycenaDo;
            ViewBag.CurrentFundName = fundName;

            ViewBag.FundName = (from f in db.Funds
                                select f.FundName).Distinct();

            var fundwycena = await db.FundWycena.Include(f => f.Fund).ToListAsync();

            switch (sortOrder)
            {
                case "cena_desc":
                    fundwycena = await db.FundWycena.Include(f => f.Fund).OrderByDescending(f => f.FundWycenaCena).ToListAsync();
                    break;
                case "date_asc":
                    fundwycena = await db.FundWycena.Include(f => f.Fund).OrderBy(f => f.FundWycenaDate).ToListAsync();
                    break;
                case "Cena":
                    fundwycena = await db.FundWycena.Include(f => f.Fund).OrderBy(f => f.FundWycenaCena).ToListAsync();
                    break;
                default: //date descending
                    fundwycena = await db.FundWycena.Include(f => f.Fund).OrderByDescending(f => f.FundWycenaDate).ToListAsync();
                    break;
            }
            int pageSize = 20;
            int pageNumber = (page ?? 1);

            fundwycena.Take(pageSize);

            //jeżeli data wyceny od jest pusta - przypisane daty 1901-12-01
            DateTime dataWycenyOd = !String.IsNullOrEmpty(wycenaOd.ToString()) ? (DateTime)wycenaOd : DateTime.Now.AddMonths(-1);

            //jeżeli data wyceny do jest pusta - przypisanie daty bieżącej
            DateTime dataWycenyDo = !String.IsNullOrEmpty(wycenaDo.ToString()) ? (DateTime)wycenaDo : DateTime.Now;

            //przypisanie do ViewBag.CurrentWycenaOd (jeżeli jest pusty) datyWycenyOd
            if (String.IsNullOrEmpty(wycenaOd.ToString()))
            {
                ViewBag.CurrentWycenaOd = dataWycenyOd;
            }

            if (!String.IsNullOrEmpty(fundName))
            {
                fundwycena = fundwycena.Where(n => n.Fund.FundName == fundName).ToList();
            }

            // if (!String.IsNullOrEmpty(wycenaOd.ToString()) || !String.IsNullOrEmpty(wycenaDo.ToString()))
            // {
            return View(fundwycena.Where(n => n.FundWycenaDate >= dataWycenyOd)
                    .Where(n => n.FundWycenaDate <= dataWycenyDo)
                    .ToPagedList(pageNumber, pageSize));

            //}
            //   else
            // {
            //     return View(fundwycena.ToPagedList(pageNumber, pageSize));
            //}

            // return View(fundwycena.Where(n => n.FundWycenaDate >= wycenaOd).ToPagedList(pageNumber, pageSize));
            //  .Skip((page - 1) * pageSize)
            // .Take(pageSize);

            //fundwycena.OrderBy(f => f.FundWycenaDate)

            //fundwycena
            //  .OrderByDescending(f => f.FundWycenaDate)
            //.Skip((page - 1) * pageSize)

            //return View(fundwycena.ToPagedList(pageNumber, pageSize));

        }

        // GET: /FundWycena/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundWycena fundwycena = await db.FundWycena.FindAsync(id);
            if (fundwycena == null)
            {
                return HttpNotFound();
            }
            return View(fundwycena);
        }

        // GET: /FundWycena/Create
        public ActionResult Create()
        {
            ViewBag.FundID = new SelectList(db.Funds, "FundID", "FundName");
            return View();
        }

        // POST: /FundWycena/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FundWycenaID,FundWycenaDate,FundWycenaCena,FundID")] FundWycena fundwycena)
        {
            if (ModelState.IsValid)
            {
                db.FundWycena.Add(fundwycena);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.FundID = new SelectList(db.Funds, "FundID", "FundName", fundwycena.FundID);
            return View(fundwycena);
        }

        // GET: /FundWycena/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundWycena fundwycena = await db.FundWycena.FindAsync(id);
            if (fundwycena == null)
            {
                return HttpNotFound();
            }
            ViewBag.FundID = new SelectList(db.Funds, "FundID", "FundName", fundwycena.FundID);
            return View(fundwycena);
        }

        // POST: /FundWycena/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "FundWycenaID,FundWycenaDate,FundWycenaCena,FundID")] FundWycena fundwycena)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fundwycena).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.FundID = new SelectList(db.Funds, "FundID", "FundName", fundwycena.FundID);
            return View(fundwycena);
        }

        // GET: /FundWycena/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundWycena fundwycena = await db.FundWycena.FindAsync(id);
            if (fundwycena == null)
            {
                return HttpNotFound();
            }
            return View(fundwycena);
        }

        // POST: /FundWycena/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            FundWycena fundwycena = await db.FundWycena.FindAsync(id);
            db.FundWycena.Remove(fundwycena);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //GET
        public ActionResult UploadFile()
        {
            return View();
        }

        //POST
        [HttpPost]
        public async Task<ActionResult> UploadFile(HttpPostedFileBase FileUpload)
        {
            try
            { 
                if (FileUpload.ContentLength > 0 ) {
                    string fileName = Path.GetFileName(FileUpload.FileName);
                    string path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                    try
                        {
                            FileUpload.SaveAs(path);
                            var dt = ProcessCSV(path);

                            //string builder na potrzeby komunikatu o nie zdefiniowanych funduszach
                            StringBuilder missFundInfo = new StringBuilder();
                            
                            //zmiena dataWyceny dodana na potrzebę spradzenia czy istnieje wycena na dany dzien dla danego funduszu
                            DateTime dataWyceny;

                        //string builder na potrzeby komunikatu że wycena dla danego funduszu i danej dany jest już wpowadzona
                        StringBuilder wycaneDateFundIDExists = new StringBuilder();
                            
                            foreach (DataRow row in dt.Rows)
                            {
                               FundWycena fw = new FundWycena();
                            //fw.FundID = Convert.ToInt32(row[2]);

                            string FundSumbol = row[0].ToString();
                            fw.FundID = (from funds in db.Funds
                                         where funds.FundSymbol == FundSumbol
                                         select funds.FundID).FirstOrDefault();

                            if (fw.FundID !=0) {  
                                 
                                //przypisanie daty wyceny z pliku do zmiennej dataWyceny
                                 dataWyceny = DateTime.ParseExact(row[2].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

                                //pobranie danych z tabeli FundWycena dla danego fundID i dla danej daty
                                var wycenaFundDate = (from fundDate in db.FundWycena
                                                      where fundDate.FundID == fw.FundID
                                                      && fundDate.FundWycenaDate.Equals(dataWyceny)
                                                      select fundDate.FundID).FirstOrDefault();
                                //sprawdzenie czy pobrane zostały jakieś dany. Jeżeli nie procesuj dalej. Jeżeli wycena na daną datę istnieje zwróć komunikat
                                if (wycenaFundDate == 0) { 
                                    string fundCena = row[6].ToString();
                                    fundCena = fundCena.Replace(".", ",");
                                    fw.FundWycenaCena = Convert.ToDecimal(fundCena);

                                    string fundCenaMin = row[5].ToString();
                                    fundCenaMin = fundCenaMin.Replace(".", ",");
                                    fw.FundWycenaCenaMin = Convert.ToDecimal(fundCenaMin);

                                    string fundCenaMax = row[4].ToString();
                                    fundCenaMax = fundCenaMax.Replace(".", ",");
                                    fw.FundWycenaCenaMax = Convert.ToDecimal(fundCenaMax);

                                    //fw.FundWycenaDate = Convert.ToDateTime(row[2]);
                                    //fw.FundWycenaDate = DateTime.ParseExact(row[2].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                                    fw.FundWycenaDate = dataWyceny;

                                    db.FundWycena.Add(fw);
                                }
                                else
                                {
                                    if (wycaneDateFundIDExists.Length == 0)
                                    {
                                        wycaneDateFundIDExists.Append("Wycena dla następującego funduszu i dla następującej daty została istnieje już w bazie danych:");
                                    }
                                    wycaneDateFundIDExists.Append(FundSumbol + "[" + dataWyceny.ToShortDateString() + "]");
                                }
                            }
                            else
                            {
                                if (missFundInfo.Length == 0)
                                {
                                    missFundInfo.Append("Następujące symbole funduszy nie zostały odnalezione w bazie danych:");
                                }
                                missFundInfo.Append(  FundSumbol);

                            }
                        }
                            await db.SaveChangesAsync();

                            ViewData["Success"] = "Plik został zaimportowany";

                        //sprawdzenie czy stringbuilder missFundInfo przechowuje jakiś tekst
                        if (missFundInfo.Length > 0)
                        {
                            ViewData["MissFund"] = missFundInfo;
                        }
                        //sprawdzenie czy stringbuilder przechowuje jakiś teksx
                        if (wycaneDateFundIDExists.Length > 0)
                            { 
                                ViewData["WycenaExist"] = wycaneDateFundIDExists;
                            }
                    }
                    catch(Exception ex)
                    {
                        ViewData["Feedback"] = "Niestety coś poszło nie tak" + ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["Feedback"] = "Niestety coś poszło nie tak" + ex.Message;
            }
            return View("UploadFile", ViewData["Feedback"]);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private  DataTable ProcessCSV(string fileName)
        {
            //Set up our variables
            string Feedback = string.Empty;
            string line = string.Empty;
            string[] strArray;
            DataTable dt = new DataTable();
            DataRow row;
            // work out where we should split on comma, but not in a sentence
            Regex r = new Regex(",");
            //Set the filename in to our stream
            StreamReader sr = new StreamReader(fileName);

            //Read the first line and split the string at , with our regular expression in to an array
            line = sr.ReadLine();
            strArray = r.Split(line);

            //For each item in the new split array, dynamically builds our Data columns. Save us having to worry about it.
            Array.ForEach(strArray, s => dt.Columns.Add(new DataColumn()));

            //Read each line in the CVS file until it’s empty
            FundWycena fw = new FundWycena();
            while ((line = sr.ReadLine()) != null)
            {
                row = dt.NewRow();

                //add our current value to our data row
                row.ItemArray = r.Split(line);

                string FundSumbol = row[0].ToString();
                //fw.FundID= Convert.ToInt32(row[2]);
                fw.FundID = (from funds in db.Funds
                             where funds.FundSymbol == FundSumbol
                             select funds.FundID).FirstOrDefault() ;
                string fundCena = row[3].ToString();
                fundCena = fundCena.Replace(".", ",");
                fw.FundWycenaCena =Convert.ToDecimal(fundCena);
                //fw.FundWycenaDate = Convert.ToDateTime(row[2]);
                fw.FundWycenaDate = DateTime.ParseExact(row[2].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

                dt.Rows.Add(row);
            }

            //Tidy Streameader up
            sr.Dispose();

            //return a the new DataTable
            return dt;

        }

    }

 


}
