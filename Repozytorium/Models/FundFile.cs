using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Repozytorium.Models
{
    public class FundFile
    {
        public int FundFileID { get; set; }
        public string FundFileName { get; set; }
        public string FundType { get; set; }
        public byte[] FundContent { get; set; }

    }
}