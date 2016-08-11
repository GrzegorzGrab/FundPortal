using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Repozytorium.Models
{
    public class FundTowarzystwo
    {
        
        public int FundTowarzystwoID { get; set; }
        [Display(Name="Nazwa towarzystwa:")]
        [Required(ErrorMessage ="Proszę podać nazwę towarzystwa")]
        public string FundTowarzystwoNazwa { get; set; }

        [Display(Name ="Adres URL Towarzystwa")]
        public string FundTowarzystwoAdresUrl { get; set; }

        [Display(Name="Data dodania towarzystwa")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString="{0:yyyy-MM-dd}",ApplyFormatInEditMode=true)]
        public System.DateTime FundTowarzystwoDataDodania { get; set; }
        
        public virtual ICollection<Fund>Funds { get; set; }
    }
}