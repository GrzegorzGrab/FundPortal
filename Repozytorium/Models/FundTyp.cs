using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Repozytorium.Models
{
    public class FundTyp
    {
        public int FundTypID { get; set; }

        [Display(Name="Typ Funduszu")]
        [Required (ErrorMessage ="Proszę podać nazwę dla wprowadzanego typu funduszu")]
        public string FundTypName { get; set; }

        [Display(Name="Opis typu funduszu")]
        public string FundTypOpis { get; set; }

        public virtual ICollection<Fund>Funds { get; set; }
    }
}