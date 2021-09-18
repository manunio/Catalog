using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Dtos;
using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.Api.Controllers
{
    // GET /items
    // [Route("[controller]")]
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository _repository;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(IItemsRepository repository, ILogger<ItemsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            // await is wrap into parenthesis,
            // or else it Select can't be used.
            // as await wait for the GetItemsAsync,
            // for it to used by Select Linq.
            var items = (await _repository.GetItemsAsync())
                .Select(item => item.AsDto());

            var itemsAsync = items.ToList();

            _logger.LogInformation($"{DateTime.UtcNow:hh:mm:ss}: Retrieved {itemsAsync.Count} items");

            return itemsAsync;
        }

        // GET /items/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await _repository.GetItemAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            return item.AsDto();
        }

        // POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow,
                UpdatedDate = DateTimeOffset.UtcNow
            };

            await _repository.CreateItemAsync(item);

            // breaking change in from core 3.1
            // prevents dotnet from removing Async from method name,
            // ex GetItemAsync -> GetItem
            // options.SuppressAsyncSuffixInActionNames = false;
            //https://youtu.be/ZFPMNSPzuTY?t=1302
            // https://stackoverflow.com/questions/62228281/rider-cant-resolve-nameofotherclass-method
            return CreatedAtAction(actionName: "GetItem", new {id = item.Id}, item.AsDto());
        }

        // PUT /items/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateItem(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = await _repository.GetItemAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            // `with-expression` takes a record, the "existingitem"
            // and creates a copy of it,
            // with these properties.
            var updateItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price,
                UpdatedDate = DateTimeOffset.UtcNow
            };

            await _repository.UpdateItemAsync(updateItem);

            return NoContent();
        }

        // DELETE /items/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteItem(Guid id)
        {
            var existingItem = await _repository.GetItemAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            await _repository.DeleteItemAsync(id);

            return NoContent();
        }
    }
}