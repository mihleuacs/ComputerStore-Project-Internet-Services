using Microsoft.AspNetCore.Mvc;
using Moq;
using ProektComputerStore.Controllers;
using ProektComputerStore.Models.Domain;
using ProektComputerStore.Models.DTOs;
using ProektComputerStore.Repositories.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProektComputerStore.Tests.Controllers
{
    public class ProductControllerTests
    {
        [Fact]
        public async Task GetProducts_Returns_OkResult_With_ProductDtos()
        {
            
            var mockProductRepository = new Mock<IProductRepository>();
            var mockCategoryRepository = new Mock<ICategoryRepository>();

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "MSI ", Description = "Monitor", Price = 100, Quantity = 5 },
                new Product { Id = 2, Name = "Acer", Description = "Laptop", Price = 200, Quantity = 10 }
            };

            mockProductRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Laptops", Description = "Laptops" },
                new Category { Id = 2, Name = "PC", Description = "Monitor" }
            };

            mockCategoryRepository.Setup(repo => repo.GetByProductIdAsync(It.IsAny<int>())).ReturnsAsync(categories);

            var controller = new ProductController(mockProductRepository.Object, mockCategoryRepository.Object);

           
            var result = await controller.GetProducts();

            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResult.Value);
            Assert.Equal(2, model.Count());
        }


        [Fact]
        public async Task GetProduct_Returns_OkObjectResult_With_Product()
        {
            
            int productId = 1;
            var mockProductRepository = new Mock<IProductRepository>();
            var mockCategoryRepository = new Mock<ICategoryRepository>();

            var product = new Product
            {
                Id = productId,
                Name = "MSI",
                Description = "Monitor",
                Price = 100,
                Quantity = 10
                
            };

            mockProductRepository.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(product);

            var controller = new ProductController(mockProductRepository.Object, mockCategoryRepository.Object);

            
            var result = await controller.GetProduct(productId);

           
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(productId, model.Id);
            Assert.Equal("MSI", model.Name);
            Assert.Equal("Monitor", model.Description);
            Assert.Equal(100, model.Price);
            Assert.Equal(10, model.Quantity);
            
        }
    }
}
