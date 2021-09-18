using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Entities;

namespace Catalog.Api.Repositories
{
    public class InMemItemsRepository : IItemsRepository
    {
        private readonly List<Item> _items = new() // target-typed new expression
        {
            new Item {Id = Guid.NewGuid(), Name = "Potion", Price = 9, CreatedDate = DateTimeOffset.UtcNow},
            new Item {Id = Guid.NewGuid(), Name = "Iron Sword", Price = 20, CreatedDate = DateTimeOffset.UtcNow},
            new Item {Id = Guid.NewGuid(), Name = "Bronze Shield", Price = 18, CreatedDate = DateTimeOffset.UtcNow},
        };

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            // Task.FromResult creates a Task,
            // that has been already completed,
            // and we want to introduce value of,
            // items collection in to that task.
            return await Task.FromResult(_items);
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            var item = _items.SingleOrDefault(i => i.Id == id);
            return await Task.FromResult(item);
        }

        public async Task CreateItemAsync(Item item)
        {
            _items.Add(item);
            // Task.CompletedTask just creates some task,
            // that is already completed and returns it,
            // Without returning anything inside it.
            await Task.CompletedTask;
        }

        public async Task UpdateItemAsync(Item item)
        {
            var index = _items.FindIndex(existingItem => existingItem.Id == item.Id);
            _items[index] = item;
            await Task.CompletedTask;
        }

        public async Task DeleteItemAsync(Guid id)
        {
            var index = _items.FindIndex(existingItem => existingItem.Id == id);
            _items.RemoveAt(index);
            await Task.CompletedTask;
        }
    }
}