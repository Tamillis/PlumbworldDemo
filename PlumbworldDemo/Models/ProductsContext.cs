using Microsoft.EntityFrameworkCore;
using PlumbworldDemo.Models.Products;

namespace PlumbworldDemo.Models
{
    public interface IProductsContext
    {
        DbSet<Product> Products { get; set; }
        int SaveChanges();
    }

    /// <summary>
    /// Note this name is TBC
    /// </summary>
    public class ProductsContext : DbContext, IProductsContext
    {
        private string path;
        public DbSet<Product> Products { get; set; }
        public ProductsContext()
        {
            // a temporary SQLite database kept in the local app data folder
            path = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "plumbworld_products.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={path}");

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
