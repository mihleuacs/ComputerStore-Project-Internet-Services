using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProektComputerStore.Models.Domain;
using ProektComputerStore.Models.DTOs;
using ProektComputerStore.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProektComputerStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var productDtos = new List<ProductDto>();

                foreach (var product in products)
                {
                    // Retrieve categories for each product
                    var categories = await _categoryRepository.GetByProductIdAsync(product.Id);

                    // Convert categories to DTOs
                    var categoryDtos = categories.Select(category => new CategoryDto
                    {
                        Name = category.Name,
                        Description = category.Description
                    }).ToList();

                    // Create ProductDto and add it to the list
                    productDtos.Add(new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Categories = categoryDtos,
                        Quantity = product.Quantity
                    });
                }

                return Ok(productDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving products: {ex.Message}");
            }
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categories = await _categoryRepository.GetByIdsAsync(productDTO.CategoryIds);

            var product = new Product
            {
                
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                Quantity = productDTO.Quantity,
                Categories = categories
            };

           

            await _productRepository.AddAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto productDTO)
        {
            if (id != productDTO.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = productDTO.Name;
            existingProduct.Description = productDTO.Description;
            existingProduct.Price = productDTO.Price;
            existingProduct.Quantity = productDTO.Quantity;

            
            if (existingProduct.Categories == null)
            {
                existingProduct.Categories = new List<Category>();
            }
            else
            {
                
                existingProduct.Categories.Clear();
            }

            
            var categories = await _categoryRepository.GetByIdsAsync(productDTO.CategoryIds);
            foreach (var category in categories)
            {
                existingProduct.Categories.Add(category);
            }

            await _productRepository.UpdateAsync(existingProduct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productRepository.DeleteAsync(id);
            return NoContent();
        }



    }
}
