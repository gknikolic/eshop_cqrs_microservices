using BuildingBlocks.DDD_Abstractions;
using Catalog.Write.Domain.Models;

namespace Catalog.Write.Domain.Events;
public class ProductUpdatedEvent(Product Product) : IDomainEvent;
