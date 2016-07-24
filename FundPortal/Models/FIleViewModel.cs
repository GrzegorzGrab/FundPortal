using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FundPortal.Models
{
    public class FileViewModel
    {
        [Display(Name="Plik")]
        public HttpPostedFileBase File { get; set; }
        public int? FundId { get; set; }
    }
}