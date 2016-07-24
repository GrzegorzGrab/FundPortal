using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Repozytorium.Models
{
    public class FundMessages
    {
        public int FundMessagesID { get; set; }

        [Required (ErrorMessage ="Proszę podać swoją nazwę")]
        public string  UsernName { get; set; }
        public string UserEmail { get; set; }
        public string Message { get; set; }
        [DataType(DataType.Date)]
        public System.DateTime SentDate { get; set; }
        public bool IsRead { get; set; }
    }
}