using Catalog.Write.Domain.Models;
using Catalog.Write.Domain.ValueObjects;

namespace Catalog.Write.Infrastructure.Data.Extensions;
public class InitialData
{
    public static IEnumerable<Product> Products => GetProducts();
    public static IEnumerable<Category> Categories => new List<Category>()
    {
        new Category(
            name: "Smart Phone",
            description: "Mobile phones with smart operating systems and advanced features."
        ),
        new Category(
            name: "White Appliances",
            description: "Large home appliances such as refrigerators and washing machines."
        ),
        new Category(
            name: "Home Kitchen",
            description: "Appliances and products for kitchen use at home."
        ),
        new Category(
            name: "Camera",
            description: "Cameras and photography equipment."
        )
    };

    private static IEnumerable<Product> GetProducts()
    {
        var categories = new List<Category>
        {
            new Category(
                name: "Smart Phone",
                description: "Mobile phones with smart operating systems and advanced features."
            ),
            new Category(
                name: "White Appliances",
                description: "Large home appliances such as refrigerators and washing machines."
            ),
            new Category(
                name: "Home Kitchen",
                description: "Appliances and products for kitchen use at home."
            ),
            new Category(
                name: "Camera",
                description: "Cameras and photography equipment."
            )
        };

        var products = new List<Product>();

        var product = new Product(
            id: new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"),
            sku: new Sku("IPH-X"),
            name: "IPhone X",
            description: "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
            price: new Price(950.00M),
            color: Domain.Enum.Color.Black
        );
        product.ChangeCategory(categories.FirstOrDefault(x => x.Name == "Smart Phone"));
        product.UpdateStock(10);
        product.AddImage("product-1.png", "IPhone X", 1);
        product.AddAttribute("Storage", "256GB");
        product.AddAttribute("Screen Size", "5.8 inches");
        product.AddAttribute("Battery Life", "12 hours");
        products.Add(product);

        product = new Product(
            id: new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"),
            sku: new Sku("SAM-10"),
            name: "Samsung 10",
            description: "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
            price: new Price(840.00M),
            color: Domain.Enum.Color.White
        );
        product.ChangeCategory(categories.FirstOrDefault(x => x.Name == "Smart Phone"));
        product.UpdateStock(20);
        product.AddImage("product-2.png", "Samsung 10", 1);
        product.AddAttribute("Storage", "128GB");
        product.AddAttribute("Camera", "16MP");
        product.AddAttribute("Water Resistance", "IP68");
        products.Add(product);

        product = new Product(
            id: new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8"),
            sku: new Sku("HUA-P"),
            name: "Huawei Plus",
            description: "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
            price: new Price(650.00M),
            color: Domain.Enum.Color.Blue
        );
        product.ChangeCategory(categories.FirstOrDefault(x => x.Name == "White Appliances"));
        product.UpdateStock(5);
        product.AddImage("product-3.png", "Huawei Plus", 1);
        product.AddAttribute("Storage", "64GB");
        product.AddAttribute("RAM", "6GB");
        product.AddAttribute("Processor", "Kirin 970");
        products.Add(product);

        product = new Product(
            id: new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27"),
            sku: new Sku("XIA-M9"),
            name: "Xiaomi Mi 9",
            description: "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
            price: new Price(470.00M),
            color: Domain.Enum.Color.Red
        );
        product.ChangeCategory(categories.FirstOrDefault(x => x.Name == "White Appliances"));
        product.UpdateStock(10);
        product.AddImage("product-4.png", "Xiaomi Mi 9", 1);
        product.AddAttribute("Storage", "64GB");
        product.AddAttribute("Battery Life", "15 hours");
        product.AddAttribute("Fast Charging", "Yes");
        products.Add(product);

        product = new Product(
            id: new Guid("b786103d-c621-4f5a-b498-23452610f88c"),
            sku: new Sku("HTC-U11"),
            name: "HTC U11+ Plus",
            description: "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
            price: new Price(380.00M),
            color: Domain.Enum.Color.Silver
        );
        product.ChangeCategory(categories.FirstOrDefault(x => x.Name == "Smart Phone"));
        product.UpdateStock(10);
        product.AddImage("product-5.png", "HTC U11+ Plus", 1);
        product.AddAttribute("Storage", "128GB");
        product.AddAttribute("Screen Resolution", "1440 x 2880 pixels");
        product.AddAttribute("Audio", "BoomSound");
        products.Add(product);

        product =  new Product(
            id: new Guid("c4bbc4a2-4555-45d8-97cc-2a99b2167bff"),
            sku: new Sku("LG-G7"),
            name: "LG G7 ThinQ",
            description: "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
            price: new Price(240.00M),
            color: Domain.Enum.Color.Black
        );
        product.ChangeCategory(categories.FirstOrDefault(x => x.Name == "Home Kitchen"));
        product.UpdateStock(10);
        product.AddImage("product-6.png", "LG G7 ThinQ", 1);
        product.AddAttribute("Storage", "64GB");
        product.AddAttribute("Camera", "Dual 16MP");
        product.AddAttribute("AI Integration", "ThinQ AI");
        products.Add(product);

        product = new Product(
            id: new Guid("93170c85-7795-489c-8e8f-7dcf3b4f4188"),
            sku: new Sku("PAN-LUMIX"),
            name: "Panasonic Lumix",
            description: "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
            price: new Price(240.00M),
            color: Domain.Enum.Color.Black
        );
        product.ChangeCategory(categories.FirstOrDefault(x => x.Name == "Camera"));
        product.UpdateStock(10);
        product.AddImage("product-6.png", "Panasonic Lumix", 1);
        product.AddAttribute("Storage", "32GB");
        product.AddAttribute("Zoom", "10x Optical Zoom");
        product.AddAttribute("Display", "3-inch LCD");
        products.Add(product);

        return products;
    }
}
