﻿using BuildingBlocks.Messaging.Events.ProductEvents;
using Catalog.Read.API.Repositories;
using MassTransit;

namespace Catalog.Read.API.EventHandlers;
public class ProductUpdatedIntegrationEventHandler(ICatalogRepository repository)
    : IConsumer<ProductUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductUpdatedIntegrationEvent> context)
    {
        var product = await repository.GetProductByIdAsync(context.Message.Id);
        if (product == null)
        {
            throw new InvalidOperationException($"Product with id {context.Message.Id} not found.");
        }

        product.Name = context.Message.Name;
        product.Sku = context.Message.Sku;
        product.Description = context.Message.Description;
        product.Price = context.Message.Price;
        product.PeicesInStock = context.Message.Quantity;
        product.ImageFiles = context.Message.ImageFiles;
        product.Categories = context.Message.Categories;
        product.Color = context.Message.Color;
        product.ProductAttributes = context.Message.ProductAttributes.Select(x => new ProductAttribute { Name = x.Name, Value = x.Value }).ToList();
        product.ProductReviews = context.Message.ProductReviews.Select(x => new ProductReview 
        { 
            Comment = x.Comment, 
            CustomerId = x.CustomerId, 
            CustomerName = x.CustomerName, 
            Rating = x.Rating, 
            CreatedDate = x.CreatedDate,
            Id = x.Id,
            ProductId = x.ProductId
        }).ToList();
        product.IsActive = context.Message.IsActive;

        await repository.UpdateProductAsync(product);
    }
}
