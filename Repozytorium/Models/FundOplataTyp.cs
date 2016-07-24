using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Repozytorium.Models
{
    public class FundOplataTyp
    {
        public int FundOplataTypID { get; set; }

        [Required(ErrorMessage="Proszę podać nazwę opłaty")]
        [Display(Name ="Nazwa opłaty")]
        [Index(IsUnique =true)]
        [MaxLength(100, ErrorMessage = "Kod waluty nie może być dłuższy niż 100 znaki")]
        public string FundOplataTypNazwa { get; set; }

        [Display(Name ="Opis opłaty")]
        public string FundOplataTypOpis { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name ="Data dodania opłaty")]
        public System.DateTime FundOplataTypDataDodania { get; set; }

        public virtual ICollection<FundZakup> FundZakup { get; set; }
    }
}