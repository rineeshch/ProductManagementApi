using Microsoft.EntityFrameworkCore;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Interfaces;
using ProductManagement.Infrastructure.Data;

namespace ProductManagement.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;
        private static readonly Random _random = new Random();

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> AddAsync(Product product)
        {
            product.Id = await GenerateUniqueIdAsync();
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            var existingProduct = await _context.Products.FindAsync(product.Id);
            if (existingProduct == null)
                throw new KeyNotFoundException($"Product with ID {product.Id} not found.");


            _context.Entry(existingProduct).CurrentValues.SetValues(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DecrementStockAsync(int id, int quantity)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null || product.StockAvailable < quantity)
                return false;

            product.StockAvailable -= quantity;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddToStockAsync(int id, int quantity)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            product.StockAvailable += quantity;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(p => p.Id == id);
        }

        public async Task<int> GenerateUniqueIdAsync()
        {
            int id;
            do
            {
                // Generate a random 6-digit number between 100000 and 999999
                id = _random.Next(100000, 1000000);
            } while (await ExistsAsync(id));

            return id;
        }
    }
}
