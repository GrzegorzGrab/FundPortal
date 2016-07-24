using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Repozytorium.Models
{
    public class FundWaluta
    {
        public FundWaluta()
        {

        }
        public int FundWalutaID { get; set; }

        [MaxLength(3, ErrorMessage="Kod waluty nie może być dłuższy niż 3 znaki")]  
        [Index(IsUnique=true)]
        [Required(ErrorMessage ="Proszę podać kod waluty")]
        [Display(Name ="Kod waluty")]
        public string FundWalutaKod { get; set; }

        [Required(ErrorMessage ="Proszę podać pełną nazwę waluty")]
        [Display(Name ="Pełna nazwa waluty")]
        public string FundWalutaPelnaNazwa { get; set; }

        [DataType(DataType.Date)]
        [Display(Name ="Data dodania waluty")]
        public System.DateTime FundWalutaDataDodania { get; set; }

        public virtual ICollection<Fund> Funds { get; set; }
    }
}