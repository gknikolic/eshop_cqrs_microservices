using BuildingBlocks.CQRS;
using Inventory.API.Models;

namespace Inventory.API.CQRS.Queries.GetInventory;


public record GetInventoryQuery() : IQuery<GetInventoryResult>;

public record GetInventoryResult(List<Product> Products);