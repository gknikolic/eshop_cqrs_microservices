﻿namespace Catalog.API.CQRS.Commands.UpdateProductQuantity;

public record UpdateProductQuantityCommand(Guid Id, int Quantity) : ICommand;
