using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FundPortal.Models
{
    public class FundsSummaryViewModel
    {
        [Display(Name = "Nazwa funduszu")]
        public string FundSummaryNazwa { get; set; }
        [Display(Name = "Średnia wartość zakupu")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal FundSummaryAvgPrice { get; set; } 
        [Display(Name = "Minimalna cena zakupu")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal FundSummaryMinPrice { get; set; }
        [Display(Name = "Maksymalna cena zakupu")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal FundSummaryMaxPrice { get; set; }
        [Display(Name = "Suma kupionych jednostek")]
        public decimal FundSummarySumJednostek { get; set; }

        public int FundSummryFundsQuantity { get; set; }
        public decimal FundSummarySumAllFund { get; set; }

        //2016-04-07;GG: utowrzene na potrzeby dodania aktualnej wyceny
        [Display (Name ="Aktualne cena jednsotki")]
        [DisplayFormat(DataFormatString ="{0:c}")]
        public decimal CurrentPrice { get; set; }

        public FundsSummaryViewModel()
        {
            CurrentPrice = 0;
        }
    }
}