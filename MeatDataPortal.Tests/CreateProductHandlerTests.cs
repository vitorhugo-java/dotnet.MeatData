using MeatData.Application.Common;
using MeatData.Application.Interfaces;
using MeatData.Application.Interfaces.Repositories;
using MeatData.Application.Products.Commands.CreateProduct;
using MeatData.Domain.Entities;
using NSubstitute;

namespace MeatDataPortal.Tests;

public class CreateProductHandlerTests
{
    private readonly IProductRepository _repository = Substitute.For<IProductRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly CreateProductHandler _sut;

    public CreateProductHandlerTests()
    {
        _sut = new CreateProductHandler(_repository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_WhenSkuIsNew_ReturnsSuccessWithProductId()
    {
        // Arrange
        var cmd = new CreateProductCommand("Alcatra", "Corte bovino", "BOV-ALC-001", 500, Guid.NewGuid());

        _repository.ExistsBySkuAsync(cmd.SKU).Returns(false);

        // Act
        var result = await _sut.Handle(cmd);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);
        await _repository.Received(1).AddAsync(Arg.Any<Product>());
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_WhenSkuAlreadyExists_ReturnsFailure()
    {
        // Arrange
        var cmd = new CreateProductCommand("Alcatra", null, "BOV-ALC-DUP", 500, Guid.NewGuid());

        _repository.ExistsBySkuAsync(cmd.SKU).Returns(true);

        // Act
        var result = await _sut.Handle(cmd);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("DUPLICATE_SKU", result.ErrorCode);
        await _repository.DidNotReceive().AddAsync(Arg.Any<Product>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync();
    }
}