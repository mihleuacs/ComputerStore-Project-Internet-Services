using Microsoft.EntityFrameworkCore;
using ProektComputerStore.Data;
using ProektComputerStore.Models.Domain;
using ProektComputerStore.Repositories.Interface;

namespace ProektComputerStore.Repositories.Repos
{
    public class CategoryRepository : ICategoryRepository
    {
       
        private readonly StoreDbContext _context;

        public CategoryRepository(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<List<Category>> GetByIdsAsync(List<int> ids)
        {
            return await _context.Categories.Where(c => ids.Contains(c.Id)).ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Category>> GetByProductIdAsync(int productId)
        {
            
            return await _context.Categories
                .Where(c => c.Products.Any(p => p.Id == productId))
                .ToListAsync();
        }
    }
}
