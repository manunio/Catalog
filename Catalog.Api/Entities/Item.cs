using System;

namespace Catalog.Api.Entities
{
    public record Item
    {
        // for init
        // you can do this
        // Item item = new()
        // {
        //      Id = Guid.NewGuid();
        // };
        // But not this
        // item.Id = Guid.NewGuid();
        public Guid Id { get; init; }
        public string Name { get; init; }
        public decimal Price { get; init; }
        public DateTimeOffset CreatedDate { get; init; }

        public DateTimeOffset UpdatedDate { get; init; }
    }
}