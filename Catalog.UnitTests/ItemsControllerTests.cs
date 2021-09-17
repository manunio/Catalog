using System;
using System.Threading.Tasks;
using Catalog.Api.Controllers;
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
        [Fact]
        // UnitOfWork_StateUnderTests_ExpectedBehaviour()
        public async Task GetItemAsync_WithUnExistingItem_ReturnsNotFound()
        {
            // Arrange
            var repositoryStub = new Mock<IItemsRepository>();
            // whenever controller invokes GetItemAsync with any Guid,
            // which moq is going to provide. It have to return a null value.
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Item) null);

            var loggerStub = new Mock<ILogger<ItemsController>>();
            var controller =
                new ItemsController(repositoryStub.Object,
                    loggerStub.Object); //.Object passes the object of type instead of moq

            // Act
            var result = await controller.GetItemAsync(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetItemAsync_WithUnExistingItem_ReturnsExpectedItem()
        {
        }
    }
}