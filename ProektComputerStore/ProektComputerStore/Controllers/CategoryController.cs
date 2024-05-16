using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProektComputerStore.Models.Domain;
using ProektComputerStore.Models.DTOs;
using ProektComputerStore.Repositories.Interface;

namespace ProektComputerStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

       
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto categoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           
            var category = new Category
            {
                Name = categoryDTO.Name,
                Description = categoryDTO.Description
            };

            await _categoryRepository.AddAsync(category);
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto categoryDTO)
        {
            if (id != categoryDTO.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            
            existingCategory.Name = categoryDTO.Name;
            existingCategory.Description = categoryDTO.Description;

            await _categoryRepository.UpdateAsync(existingCategory);
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}

