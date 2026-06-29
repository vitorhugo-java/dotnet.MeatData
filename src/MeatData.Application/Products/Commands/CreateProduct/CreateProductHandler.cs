using MeatData.Application.Common;
using MeatData.Application.Interfaces;
using MeatData.Application.Interfaces.Repositories;
using MeatData.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeatData.Application.Products.Commands.CreateProduct
{
    public sealed class CreateProductHandler
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductHandler(IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateProductCommand cmd, CancellationToken ct = default)
        {
            var skuExists = await _repository.ExistsBySkuAsync(cmd.SKU, ct);
            if (skuExists) return Result<Guid>.Failure($"SKU '{cmd.SKU}' já existe.", "DUPLICATE_SKU");

            var product = Product.Create(cmd.Name, cmd.Description, cmd.SKU, cmd.WeightGrams, cmd.CategoryId);

            await _repository.AddAsync(product, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result<Guid>.Success(product.Id);
        }
    }
}
