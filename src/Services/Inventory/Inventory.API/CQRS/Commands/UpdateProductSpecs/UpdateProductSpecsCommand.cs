using BuildingBlocks.CQRS;
using Inventory.API.Dtos;

namespace Inventory.API.CQRS.Commands.UpdateProductSpecs;

public record UpdateProductSpecsCommand(ProductDto Product) : ICommand;

