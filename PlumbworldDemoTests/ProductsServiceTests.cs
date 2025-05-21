using Microsoft.EntityFrameworkCore;
using Moq;
using PlumbworldDemo.Models;
using PlumbworldDemo.Models.Products;
using PlumbworldDemo.Services;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace PlumbworldDemoTests
{
    /// <summary>
    /// Testing - void AddProduct(Product product) <br/>
    /// void UpdateProduct(Product product);<br/>
    /// void DeleteProduct(int id);<br/>
    /// void SaveChanges();<br/>
    /// Product? GetProduct(int id);<br/>
    /// IQueryable.Product. Products { get; }
    /// </summary>
    public class ProductsServiceTestSuite
    {
        //Moq context
        private Mock<IProductsContext> moqContext = new Mock<IProductsContext>();

        // Dummy data
        private List<Product> dataMaster = new List<Product>()
        {
            new Product
            {
                Id = 1,
                Name = "O-ring",
                Description = "A rubber o-ring, 5mm diameter",
                Price = 0.55m,
                Stock = 99,
                IsActive = true
            },
            new Product
            {
                Id = 2,
                Name = "Plunger",
                Description = "Ye olde fashioned",
                Price = 9.99m,
                Stock = 123,
                IsActive = true
            }, new Product
            {
                Id = 3,
                Name = "Marble Tiles",
                Description = "Tiles for the fancy bathroom",
                Price = 12m,
                Stock = 0,
                IsActive = false
            }, new Product
            {
                Id = 4,
                Name = "Wrench Set",
                Description = null,
                Price = 25.55m,
                Stock = 12,
                IsActive = true
            }, new Product
            {
                Id = 5,
                Name = "Dummy's Guide To Plumbing",
                Description = "A book of questionable advice but it sells",
                Price = 19.00m,
                Stock = 5,
                IsActive = false
            }
        };

        private Product dummy = new()
        {
            Id = 6,
            Name = "New item",
            Description = "Shiny",
            Price = 100m,
            Stock = 1,
            IsActive = true
        };

        //Subject Under Test
        private IProductsService _sut;

        [SetUp]
        public void Setup()
        {
            var dataCopy = new List<Product>(dataMaster);
            var queryableData = dataCopy.AsQueryable();
            var moqSet = new Mock<DbSet<Product>>();
            moqSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            moqSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            moqSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            moqSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(() => queryableData.GetEnumerator());
            moqSet.Setup(p => p.Add(It.IsAny<Product>())).Callback<Product>((s) => dataCopy.Add(s));
            moqSet.Setup(p => p.Remove(It.IsAny<Product>())).Callback<Product>((s) => dataCopy.Remove(s));
            moqSet.Setup(p => p.Find(It.IsAny<int>())).Returns(dataCopy.First());

            moqContext
                .Setup(moq => moq.Products)
                .Returns(moqSet.Object);
            moqContext
                .Setup(moq => moq.SaveChanges())
                .Returns(0);

            _sut = new ProductsService(moqContext.Object);
        }

        /// <summary>
        /// Very basic initial test to ensure the test suite and basic infrastructure is up and running
        /// </summary>
        [Test]
        public void Service_Products_HasCountAbove0()
        {
            Assert.That(_sut.Products.Count() > 0, Is.EqualTo(true));
        }

        [Test]
        public void Service_GetProduct_RetrievesCorrectly()
        {
            //Arrange - already done in [Setup]
            //Act
            Product result = _sut.GetProduct(1)!;
            //Assert
            Assert.That(result.Name, Is.EqualTo("O-ring"));
            Assert.That(result.Description, Is.EqualTo("A rubber o-ring, 5mm diameter"));
            Assert.That(result.Price, Is.EqualTo(.55m));
            Assert.That(result.Stock, Is.EqualTo(99));
            Assert.That(result.IsActive, Is.True);
        }

        [Test]
        public void Service_AddProduct_IncreasesProductCountBy1()
        {
            //Arrange
            int preCount = dataMaster.Count;
            //Act
            _sut.AddProduct(dummy);
            
            //Assert precount is 1 less than post count - DAMP test programming
            int postCount = dataMaster.Count;
            Assert.That(preCount, Is.LessThan(postCount).Within(1));
        }

        [Test]
        public void Service_UpdateProduct_UpdatesProductWithCorrectData()
        {
            //Arrange
            Product updated = new Product()
            {
                Id = dataMaster.First().Id,
                Name = "New o-ring",
                Description = "A rubber o-ring, 10mm diameter",
                Price = .99m,
                Stock = 1,
                IsActive = false
            };

            //Act
            _sut.UpdateProduct(updated);

            //Assert
            Product result = _sut.GetProduct(1)!;
            Assert.That(result.Name, Is.EqualTo("New o-ring"));
            Assert.That(result.Description, Is.EqualTo("A rubber o-ring, 10mm diameter"));
            Assert.That(result.Price, Is.EqualTo(.99m));
            Assert.That(result.Stock, Is.EqualTo(1));
            Assert.That(result.IsActive, Is.False);
        }

        [Test]
        public void Service_DeleteProduct_LowersCountBy1()
        {
            //Arrange
            int preCount = dataMaster.Count;
            //Act
            _sut.DeleteProduct(dataMaster.First().Id);

            //Assert precount is 1 less than post count - DAMP test programming
            int postCount = dataMaster.Count;
            Assert.That(preCount, Is.GreaterThan(postCount).Within(1));
        }
    }
}
