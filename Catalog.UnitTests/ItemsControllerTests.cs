using System;
using System.Threading.Tasks;
using Catalog.Api.Controllers;
using Catalog.Api.Dtos;
using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Catalog.UnitTests
{
    public class ItemsControllerTests
    {
        private readonly Mock<IItemsRepository> _repositoryStub = new();
        private readonly Mock<ILogger<ItemsController>> _loggerStub = new();
        private readonly Random _rand = new();

        [Fact]
        // UnitOfWork_StateUnderTests_ExpectedBehaviour()
        public async Task GetItemAsync_WithUnExistingItem_ReturnsNotFound()
        {
            // Arrange

            // whenever controller invokes GetItemAsync with any Guid,
            // which moq is going to provide. It have to return a null value.
            _repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Item) null);

            //.Object passes the object of type instead of moq
            var controller = new ItemsController(_repositoryStub.Object, _loggerStub.Object);

            // Act
            var result = await controller.GetItemAsync(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetItemAsync_WithUnExistingItem_ReturnsExpectedItem()
        {
            //Arrange
            var expectedItem = CreateRandomItem();
            _repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedItem);
            var controller = new ItemsController(_repositoryStub.Object, _loggerStub.Object);

            // Act
            var result = await controller.GetItemAsync(Guid.NewGuid());

            //Assert
            Assert.IsType<ItemDto>(result.Value); //Value property has the dto.
            var dto = ((ActionResult<ItemDto>) result).Value;
            Assert.Equal(expectedItem.Id, dto.Id);
            Assert.Equal(expectedItem.Name, dto.Name);
            Assert.Equal(expectedItem.Name, dto.Name);
            
        }

        private Item CreateRandomItem()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Price = _rand.Next(1000),
                CreatedDate = DateTimeOffset.UtcNow,
                UpdatedDate = DateTimeOffset.UtcNow
            };
        }
    }
}