using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlumbworldDemo.Models;
using PlumbworldDemo.Models.Products;
using PlumbworldDemo.Services;

namespace PlumbworldDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ProductsContext>();
            builder.Services.AddScoped<IProductsService, ProductsService>();
            builder.Services.AddScoped<IProductsContext, ProductsContext>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                //Is development, and seed data is needed in the db
                SeedDatabase();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        /// <summary>
        /// It would be better for the seeding to belong to a separate utility class but this is the one and only time such a service is used and this is a demo project so here it is
        /// </summary>
        private static void SeedDatabase()
        {
            using (var context = new ProductsContext())
            {
                // Check if db has been seeded
                if (context.Products.Any()) return;

                context.Products.AddRange(
                    new Product
                    {
                        Name = "O-ring",
                        Description = "A rubber o-ring, 5mm diameter",
                        Price = 0.55m,
                        Stock = 99,
                        IsActive = true
                    },
                    new Product
                    {
                        Name = "Plunger",
                        Description = "Ye olde fashioned",
                        Price = 9.99m,
                        Stock = 123,
                        IsActive = true
                    }, new Product
                    {
                        Name = "Marble Tiles",
                        Description = "Tiles for the fancy bathroom",
                        Price = 12m,
                        Stock = 0,
                        IsActive = false
                    }, new Product
                    {
                        Name = "Wrench Set",
                        Description = null,
                        Price = 25.55m,
                        Stock = 12,
                        IsActive = true
                    }, new Product
                    {
                        Name = "Dummy's Guide To Plumbing",
                        Description = "A book of questionable advice but it sells",
                        Price = 19.00m,
                        Stock = 5,
                        IsActive = false
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
