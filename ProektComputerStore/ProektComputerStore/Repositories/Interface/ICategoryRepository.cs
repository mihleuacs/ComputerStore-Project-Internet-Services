using ProektComputerStore.Models.Domain;

namespace ProektComputerStore.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(int id);
        Task<List<Category>> GetAllAsync();
        Task<Category> AddAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task DeleteAsync(int id);
        Task<List<Category>> GetByIdsAsync(List<int> ids);

        Task<List<Category>> GetByProductIdAsync(int productId);
    }
}
