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
            => _context.Products.Remove(product);

        public Task<bool> ExistsBySkuAsync(string sku, CancellationToken ct = default)
            => _context.Products.AnyAsync(p => p.SKU == sku, ct);

        public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default)
            => await _context.Products
                .Include(p => p.Category)
                .Include(p => p.NutritionProfile)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<IReadOnlyList<Product>> GetByCategoryAsync(Guid categoryId, CancellationToken ct = default)
            => await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Category)
                .Include(p => p.NutritionProfile)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _context.Products
                .Include(p => p.Category)
                .Include(p => p.NutritionProfile)
                .FirstOrDefaultAsync(p => p.Id == id, ct);

        public async Task<Product?> GetBySkuAsync(string sku, CancellationToken ct = default)
            => await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.SKU == sku, ct);

        public void Update(Product product)
            => _context.Products.Update(product);
    }
}
