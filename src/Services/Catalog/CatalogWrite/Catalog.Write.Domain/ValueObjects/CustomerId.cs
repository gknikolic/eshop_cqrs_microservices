﻿namespace Catalog.Write.Domain.ValueObjects;
public class CustomerId
{
    public Guid Value { get; private set; }

    public CustomerId(Guid value)
    {
        if (value == Guid.Empty)
            throw new DomainException("Customer ID cannot be empty");

        Value = value;
    }

    // Implicit conversion to Guid
    public static implicit operator Guid(CustomerId self) => self.Value;
}
