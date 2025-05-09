using ProductManagement.Core.Entities;

namespace ProductManagement.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> AddAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
        Task<bool> DecrementStockAsync(int id, int quantity);
        Task<bool> AddToStockAsync(int id, int quantity);
        Task<bool> ExistsAsync(int id);
        Task<int> GenerateUniqueIdAsync();
    }
}
