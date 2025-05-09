using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManagement.Api.Controllers;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Interfaces;

namespace ProductManagement.Tests;

public class ProductsControllerTests
{
    private readonly Mock<IProductRepository> _mockRepo;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _mockRepo = new Mock<IProductRepository>();
        _controller = new ProductsController(_mockRepo.Object);
    }

    [Fact]
    public async Task GetProducts_ReturnsOkResult_WithListOfProducts()
    {
        // Arrange
        var expectedProducts = new List<Product>
        {
            new() { Id = 100001, Name = "Product 1", Price = 10.99m, StockAvailable = 50 },
            new() { Id = 100002, Name = "Product 2", Price = 20.99m, StockAvailable = 30 }
        };
        _mockRepo.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(expectedProducts);

        // Act
        var result = await _controller.GetProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProducts = Assert.IsType<List<Product>>(okResult.Value);
        Assert.Equal(2, returnedProducts.Count);
    }

    [Fact]
    public async Task GetProduct_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var expectedProduct = new Product { Id = 100001, Name = "Test Product", Price = 15.99m, StockAvailable = 10 };
        _mockRepo.Setup(repo => repo.GetByIdAsync(100001))
            .ReturnsAsync(expectedProduct);

        // Act
        var result = await _controller.GetProduct(100001);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProduct = Assert.IsType<Product>(okResult.Value);
        Assert.Equal(expectedProduct.Id, returnedProduct.Id);
    }

    [Fact]
    public async Task GetProduct_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Product)null);

        // Act
        var result = await _controller.GetProduct(999999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateProduct_WithValidProduct_ReturnsCreatedAtAction()
    {
        // Arrange
        var productToCreate = new Product { Name = "New Product", Price = 25.99m, StockAvailable = 100 };
        var createdProduct = new Product { Id = 100001, Name = "New Product", Price = 25.99m, StockAvailable = 100 };
        
        _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Product>()))
            .ReturnsAsync(createdProduct);

        // Act
        var result = await _controller.CreateProduct(productToCreate);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(ProductsController.GetProduct), createdAtActionResult.ActionName);
        var returnedProduct = Assert.IsType<Product>(createdAtActionResult.Value);
        Assert.Equal(createdProduct.Id, returnedProduct.Id);
    }

    [Fact]
    public async Task UpdateProduct_WithValidIdAndProduct_ReturnsNoContent()
    {
        // Arrange
        var product = new Product { Id = 100001, Name = "Updated Product", Price = 30.99m, StockAvailable = 75 };
        _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Product>()))
            .ReturnsAsync(product);

        // Act
        var result = await _controller.UpdateProduct(100001, product);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DecrementStock_WithValidQuantity_ReturnsOkResult()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.DecrementStockAsync(100001, 5))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DecrementStock(100001, 5);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DecrementStock_WithInvalidQuantity_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.DecrementStock(100001, 0);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task AddToStock_WithValidQuantity_ReturnsOkResult()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.AddToStockAsync(100001, 10))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.AddToStock(100001, 10);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task AddToStock_WithInvalidQuantity_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.AddToStock(100001, -5);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}
