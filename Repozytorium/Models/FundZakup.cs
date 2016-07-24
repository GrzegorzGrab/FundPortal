using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Repozytorium.Models
{
    public class FundZakup
    {
        public int FundZakupID { get; set; }

        [Required(ErrorMessage="Proszę podać ilość kupionych jednostek")]
        [Display(Name ="Ilość kupionych jednostek")]
        public decimal FundZakupIlosc { get; set; }

        [Required(ErrorMessage ="Proszę podać cenę kupionych jednostek")]
        [Display(Name ="Cena kupionych jednostek")]
        [DisplayFormat(DataFormatString ="{0:c}")]
        public decimal FundZakupCena { get; set; }


        public int FundID { get; set; }

        [DataType(DataType.Date)]
        [Display(Name ="Data zakupu")]
        public System.DateTime FundZakupDataZakupu { get; set; }
        public string UzytkownikID { get; set; }
        public virtual Fund Fund { get; set; }
        public virtual Uzytkownik Uzytkownik { get; set; }
        [DataType(DataType.Date)]
        public System.DateTime FundZakupDataDodaniaZakupu { get; set; }

        [Required(ErrorMessage ="Proszę podać typ opłaty")]
        [Display(Name ="Typ opłaty")]
        public int FundOplataTypID { get; set; }

        //GG_2016-04-09
        
        //[Required(ErrorMessage = "Proszę podać typ opłaty1")]
        [Display(Name = "Typ opłaty nazwa")]
        public  virtual FundOplataTyp FundOplataTyp { get; set; }
        

        [Display(Name ="Wysokość opłaty")]
        [DisplayFormat(DataFormatString ="{0:c}")]
        public decimal FundOplataWysokosc { get; set; }

        [NotMapped]
        [Display(Name ="Aktualna wycena")]
        public decimal CurrentPrice { get; set; }

        [Display(Name ="Wartość transakcji")]
        [DisplayFormat(DataFormatString ="{0:c}")]
        public decimal WartoscZakupu
        {
            get { return FundZakupCena * FundZakupIlosc + FundOplataWysokosc; }
        }

        //GG_2016-04-08 : dodanie aktualnej ceny jednostki z wyceny
        [NotMapped]
        public decimal FundZakupCurrenPrice { get; set; }


    }


}