using Base.Core.Domain.Common;

namespace ProductCoreDomainEntities;

public class Product : BaseEntity
{    
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }
}