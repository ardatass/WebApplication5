using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class aktivite
    {
        [Key]
        public int AktiviteID { get; set; }

        [Required]
        public int MusteriID { get; set; }

        [ForeignKey("MusteriID")]
        public virtual Musteri Musteri { get; set; }

        [Required]
        public string AktiviteTipi { get; set; }

        [Required]
        public DateTime AktiviteTarihi { get; set; }

        public string Aciklama { get; set; }

       
        

        
    }
}