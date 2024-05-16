using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProektComputerStore.Controllers;
using ProektComputerStore.Models.Domain;
using ProektComputerStore.Repositories.Interface;
using Xunit;

namespace ProektComputerStore.Tests.Controllers
{
    public class CategoryControllerTests
    {
        [Fact]
        public async Task GetCategories_ReturnsOkResultWithListOfCategories()
        {
           
            var expectedCategories = new List<Category>
            {
                new Category { Id = 1, Name = "Laptops", Description = "laptops and notebooks" },
                new Category { Id = 2, Name = "Smartphones", Description = "Smartphones and mobile devices" }
            };

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedCategories);

            var controller = new CategoryController(categoryRepositoryMock.Object);

            
            var result = await controller.GetCategories();

            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Category>>(okResult.Value);
            Assert.Equal(expectedCategories, model);
        }

        [Fact]
        public async Task GetCategory_WithValidId_ReturnsOkResultWithCategory()
        {
           
            var expectedCategory = new Category { Id = 1, Name = "Laptop", Description = "laptops and notebooks" };

            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(expectedCategory);

            var controller = new CategoryController(categoryRepositoryMock.Object);

            
            var result = await controller.GetCategory(1);

            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<Category>(okResult.Value);
            Assert.Equal(expectedCategory, model);
        }

    }
}
