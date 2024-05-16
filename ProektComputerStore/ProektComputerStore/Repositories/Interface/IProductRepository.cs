using ProektComputerStore.Models.Domain;

namespace ProektComputerStore.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(int id);
        Task<List<Product>> GetAllAsync();
        Task<Product> AddAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task UpdateWithCategoriesAsync(Product product);
        Task DeleteAsync(int id);



    }
}
