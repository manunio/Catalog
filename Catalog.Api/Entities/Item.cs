using System;

namespace Catalog.Api.Entities
{
    public class Item
    {
        // for init
        // you can do this
        // Item item = new()
        // {
        //      Id = Guid.NewGuid();
        // };
        // But not this
        // item.Id = Guid.NewGuid();
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }
    }
}