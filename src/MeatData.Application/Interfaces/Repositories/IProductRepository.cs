using MeatData.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeatData.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Product?> GetBySkuAsync(string sku, CancellationToken ct = default);
        Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default);
        Task<IReadOnlyList<Product>> GetByCategoryAsync(Guid categoryId, CancellationToken ct = default);
        Task<bool> ExistsBySkuAsync(string sku, CancellationToken ct = default);
        Task AddAsync(Product product, CancellationToken ct = default);
        void Update(Product product);
        void Delete(Product product);
    }
}
