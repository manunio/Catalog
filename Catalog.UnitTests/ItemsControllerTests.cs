using System;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Controllers;
using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using FluentAssertions;
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
            result.Result.Should().BeOfType<NotFoundResult>();
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
            // compares properties of both object
            // options.ComparingByMembers<Item>() is used for handling,
            // conversion between record types.
            result.Value.Should().BeEquivalentTo(expectedItem, options =>
                options.ComparingByMembers<Item>());
        }

        [Fact]
        public async Task GetItemsAsync_WithExistingItem_ReturnsAllItems()
        {
            //Arrange
            var expectedItems = Enumerable.Range(0, 3)
                .Select(_ => CreateRandomItem())
                .ToArray();
            
            _repositoryStub.Setup(repo => repo.GetItemsAsync())
                .ReturnsAsync(expectedItems);
            var controller = new ItemsController(_repositoryStub.Object, _loggerStub.Object);

            //Act
            var actualItems = await controller.GetItemsAsync();

            //Assert
            actualItems.Should().BeEquivalentTo(expectedItems,
                options => options.ComparingByMembers<Item>());
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