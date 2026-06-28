using MeatData.Application.Interfaces.Repositories;
using MeatData.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeatData.Infrastructure.Persistence.Repositories
{
    public sealed class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product, CancellationToken ct = default)
            => await _context.Products.AddAsync(product, ct);

        public void Delete(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsBySkuAsync(string sku, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Product>> GetByCategoryAsync(Guid categoryId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default) 
            => await _context.Products
                .Include(p => p.NutritionProfile)
                .FirstOrDefaultAsync(p => p.Id == id, ct);

        public Task<Product?> GetBySkuAsync(string sku, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public void Update(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
