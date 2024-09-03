using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace WebApplication5.Models
{
    public class AppDbContext : DbContext
    {

        public virtual DbSet<Musteri> MusteriTablosu { get; set; }

        public DbSet<User> Users { get; set; } // User modelini içeren DbSet
        public  DbSet<aktivite> aktivites { get; set; }

       

    }
}

