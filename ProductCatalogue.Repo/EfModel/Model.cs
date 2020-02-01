using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ProductCatalogue.Repository.EfModel
{
    public partial class ProductContext : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(@"Data Source=BIPLABHOMEPC\SQLSERVER2017;Initial Catalog=Arcadis;User Id=biplabhome;Password=Nakshal!01051987;");       
    }

    public partial class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Title { get; set; }
        public decimal Cost { get; set; }

        public int Quantity { get; set; }
        public decimal TotalCost { get; set; }
    }
}
