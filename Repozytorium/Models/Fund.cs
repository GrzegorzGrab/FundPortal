using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Repozytorium.Models
{
    public class Fund
    {
        public int FundID { get; set; }

        [Display(Name = "Nazwa Funduszu")]
        [Required(ErrorMessage = "Proszę podać nazwę funduszu")]
        //[Index(IsUnique =true)]
        public string FundName { get; set; }

        [Display(Name = "Typ Funduszu")]
        [Required]
        public int FundTypID { get; set; }
        public int FundTowarzystwoID { get; set; }
        [Display(Name = "Waluta funduszu")]
        public int FundWalutaID { get; set; }

        [Required(ErrorMessage = "Proszę podać symbol funduszu")]
        public string FundSymbol { get; set; }
        [Display(Name = "Zarządzający: ")]
        public string FundZarzadzajacy { get; set; }
        [Display(Name = "Data uruchomienia: ")]
        [DataType(DataType.Date)]
        public DateTime FundDataUruchomienia { get; set; }
        [Display(Name = "Pierwsza wpłata: ")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal FundPierwszajWplata { get; set; }
        [Display(Name = "Następna wpłata: ")]
        [DisplayFormat(DataFormatString = "{0:c}") ] 
        public decimal FundNastepnaWplata { get; set; }

        [Display(Name ="Polityka inwestycyjna: ")]
        public string FundPolityka { get; set; }

        //dodanie pliku
        //public int FundFileID { get; set; }
        public int? FundFileID { get; set; }
        public virtual FundFile FundFile { get; set; }
        public virtual FundTyp FundTyp { get; set; }

        public virtual FundTowarzystwo FundTowarzystwo { get; set; }
        public virtual ICollection<FundZakup> FundZakup { get; set; }
        public virtual FundWaluta FundWaluta { get; set; }

        public virtual ICollection<FundWycena> FundWycena { get; set; }
        public ICollection <Transakcja>Transakcje { get; set; }
    }
}