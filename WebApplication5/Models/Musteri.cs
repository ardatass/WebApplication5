using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Models
{
    public class Musteri
    {
        [Key]
        public int MusteriID { get; set; }

        public string AdSoyad { get; set; }

        public string Emaıl { get; set; }

        public string Telefon { get; set; }

        public DateTime KayıtTarihi { get; set; } = DateTime.Now;

      
        

    }
}