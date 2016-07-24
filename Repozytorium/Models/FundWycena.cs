using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Repozytorium.Models
{
    public class FundWycena
    {
        public int FundWycenaID { get; set; }

        [Display(Name="Data wyceny")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime FundWycenaDate { get; set; }

        [Display(Name ="Wycena kurs")]
        [DisplayFormat(DataFormatString ="{0:c}")]
        public decimal FundWycenaCena { get; set; }

        [Display (Name ="Wycena kurs minimalny")]
        [DisplayFormat(DataFormatString ="{0:c}")]
        public decimal FundWycenaCenaMin { get; set; }
        [Display(Name = "Wycena kurs maksymalny")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal FundWycenaCenaMax { get; set; }

        public int FundID { get; set; }

        public virtual Fund Fund { get; set; }
    }
}