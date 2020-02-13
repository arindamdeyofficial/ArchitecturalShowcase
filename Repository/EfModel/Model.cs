using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace ProductCatalogue.Repository.EfModel
{
    public class ProductContext : DbContext
    {
        public const int SqlServerViolationOfUniqueIndex = 2601;
        public const int SqlServerViolationOfUniqueConstraint = 2627;

        public virtual DbSet<Product> Products { get; set; }

        public ProductContext()
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlServer(@"Data Source=BIPLABHOMEPC\SQLSERVER2017;Initial Catalog=Arcadis;User Id=biplabhome;Password=Nakshal!01051987;");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasKey((k) => new { k.Title, k.BusinessName });
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            //DbContext is NOT Thread safe though to free up UI it's needed
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var result = 0;
                try
                {
                    result = await base.SaveChangesAsync(cancellationToken);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    //log
                }
                catch (DbUpdateException ex)
                {
                    var sqlEx = ex?.InnerException as SqlException;
                    if (sqlEx.Number == SqlServerViolationOfUniqueIndex || sqlEx.Number == SqlServerViolationOfUniqueConstraint)
                    {
                        //log
                    }
                }

                scope.Complete();
                return result;
            }
        }
        //remove-migration
        //EntityFrameworkCore\Add-Migration InitialCreate
        //EntityFrameworkCore\Update-Database
    }

    public class Product
    {
        public string Title { get; set; }
        public string BusinessName { get; set; }
        public decimal Cost { get; set; }
        public int Quantity { get; set; }
        public decimal TotalCost { get; set; }
    }
}
