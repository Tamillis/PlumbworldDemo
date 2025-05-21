using PlumbworldDemo.Models;
using PlumbworldDemo.Models.Products;

namespace PlumbworldDemo.Services
{
    public interface IProductsService
    {
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
        Product? GetProduct(int id);
        IQueryable<Product> Products { get; }
    }

    public class ProductsService : IProductsService
    {
        public ProductsService(IProductsContext context)
        {
            _context = context;
        }
        private IProductsContext _context;

        private IQueryable<Product> _products = null!;
        public IQueryable<Product> Products
        {
            get
            {
                if (_products == null) _products = _context.Products;
                return _products;
            }
        }

        public Product? GetProduct(int id)
        {
            return _context.Products.Find(id);
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            var p = GetProduct(product.Id);
            p.Name = product.Name;
            p.Description = product.Description;
            p.Price = product.Price;
            p.Stock = product.Stock;
            p.IsActive = product.IsActive;
            _context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            var p = GetProduct(id);
            if(p != null) _context.Products.Remove(p);
            _context.SaveChanges();
        }
    }
}
