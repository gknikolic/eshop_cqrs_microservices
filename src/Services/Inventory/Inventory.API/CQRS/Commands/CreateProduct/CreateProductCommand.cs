using BuildingBlocks.CQRS;
using Inventory.API.Dtos;

namespace Inventory.API.CQRS.Commands.CreateProduct;

public record CreateProductCommand(ProductDto Product) : ICommand<CreateProductResult>;


public record CreateProductResult(Guid Id);
