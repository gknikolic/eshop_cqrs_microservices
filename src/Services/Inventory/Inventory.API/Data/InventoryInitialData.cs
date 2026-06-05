using Inventory.API.Models;
using Marten;
using Marten.Schema;

namespace Inventory.API.Data;

public class InventoryInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();

        if (await session.Query<Product>().AnyAsync())
            return;

        // Marten UPSERT will cater for existing records
        //session.Store(GetPreconfiguredInventoryItems());
        await session.SaveChangesAsync();
    }

    private static IEnumerable<Product> GetPreconfiguredInventoryItems() => new List<Product>()
            {
                new Product()
                {
                    Id = new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"),
                    Name = "IPhone X",
                    Quantity = 10
                },
                new Product()
                {
                    Id = new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"),
                    Name = "Samsung 10",
                    Quantity = 20
                },
                new Product()
                {
                    Id = new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8"),
                    Name = "Huawei Plus",
                    Quantity = 5
                },
                new Product()
                {
                    Id = new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27"),
                    Name = "Xiaomi Mi 9",
                    Quantity = 10
                },
                new Product()
                {
                    Id = new Guid("b786103d-c621-4f5a-b498-23452610f88c"),
                    Name = "HTC U11+ Plus",
                    Quantity = 10
                },
                new Product()
                {
                    Id = new Guid("c4bbc4a2-4555-45d8-97cc-2a99b2167bff"),
                    Name = "LG G7 ThinQ",
                    Quantity = 10
                },
                new Product()
                {
                    Id = new Guid("93170c85-7795-489c-8e8f-7dcf3b4f4188"),
                    Name = "Panasonic Lumix",
                    Quantity = 10 
                }
            };
}
