using Product.Core.Application.Product.Event.Create;

namespace Product.WebApi.GraphQL.ProductType;

public class ProductType : ObjectType<ProductDocument>
{
    protected override void Configure(IObjectTypeDescriptor<ProductDocument> descriptor)
    {
        descriptor.Field(f => f.Id).Type<IdType>();
        descriptor.Field(f => f.Name).Type<StringType>();
        descriptor.Field(f => f.Price).Type<FloatType>();
    }
}
