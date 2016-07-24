using Repozytorium.Models.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FundPortal.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminNavController : Controller
    {
        //
        // GET: /AdminNav/
        public PartialViewResult AdminMenu()
        {
            
            //nav.NavigatorWyswietl=  {"Fundusze","Typy Funduszy"};
            string[] nawigacja =  {"Fundusze","Typy Funduszy"};
            Navigator nav = new Navigator(nawigacja);
            //string nawigacja = "TO JEST z"
            return PartialView(nav);
        }
        public PartialViewResult AdminMain()
        {
            return PartialView();
        }
	}
}