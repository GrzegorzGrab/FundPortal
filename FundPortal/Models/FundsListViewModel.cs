using Repozytorium.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FundPortal.Models
{
    public class FundsListViewModel
    {
        
        public int FundListFundID { get; set; }
        [Display(Name = "Nazwa Funduszu")]
        public string  FundListFundName{ get; set; }
        [Display(Name ="Symbol funduszu")]
        public string FundListFundSymbol { get; set; }
        [Display(Name ="Typ funduszu")]
        public string FundListTypName{ get; set; }
        [Display(Name ="Towarzystwo")]
        public string FundListTowarzystwoName { get; set; }
        [Display(Name ="Data wyceny")]
        [DataType(DataType.Date)]
        public DateTime DataWyceny { get; set; }
        [Display(Name ="Ostatnia wycena")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal FundListCurrentPrice { get; set; }
        [Display(Name ="Adres URL Towarzystwa")]
        public string FundListFundTowarzystwoAdresUrl { get; set; }

        [Display(Name = "Zarządzający: ")]
        public string FundListZarzadzajacy { get; set; }
        [Display(Name = "Data uruchomienia: ")]
        [DataType(DataType.Date)]
        public DateTime FundListDataUruchomienia { get; set; }
        [Display(Name = "Pierwsza wpłata: ")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal FundListPierwszajWplata { get; set; }
        [Display(Name = "Następna wpłata: ")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal FundListNastepnaWplata { get; set; }

        [Display(Name = "Polityka inwestycyjna: ")]
        public string FundListPolityka { get; set; }

        public int? FundListFundFileID { get; set; }
        public string FundListFileName { get; set; }
        public byte[] FundListFundContent { get; set; }
    }
}