using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Repozytorium.Models
{
    public class Transakcja
    {
        public int TransakcjaID { get; set; }
        public int FundID { get; set; }
        
        public string UzytkownikId { get; set; }

        public virtual Fund Fund { get; set; }
        public virtual Uzytkownik Uzytkownik { get; set; }
    }
}