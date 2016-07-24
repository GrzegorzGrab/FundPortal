using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundPortal.Models
{
    public class FundsCurrentPrice
    {
        public DateTime DataWycenyNajnowsza { get; set; }
        public int WycenaId { get; set; }
        public decimal FundWycenaCena
        {
            get; set;
        }
        public int FundId { get; set; }
        public string FundName { get; set; }

        public FundsCurrentPrice() { }

        public FundsCurrentPrice(string fundName)
        {
            this.FundName = fundName;
            FundWycenaCena = 0;
            DataWycenyNajnowsza = DateTime.Now;
        }
    }
}
