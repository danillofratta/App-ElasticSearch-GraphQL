
using MediatR;
using Product.Core.Application.Product.Elastic.Query.GetAll;
using Product.Core.Application.Product.Event.Create;
using Product.Core.Application.Product.Repository.Query.Get;
using Product.Core.Application.Product.Repository.Query.GetName;

namespace Product.WebApi.GraphQL;

public class ProductQuery
{
    private readonly IMediator _mediator;

    public ProductQuery(IMediator mediator)
    {
        _mediator = mediator;
    }

    //TODO pagination
    [GraphQLName("getElasticAllProducts")]
    public async Task<IReadOnlyList<ProductDocument>> GetElasticAllProducts() =>
        await _mediator.Send(new GetAllProductQuery());

    [GraphQLName("getElasticProductById")]
    public async Task<ProductDocument> GetElasticProductById(Guid id) =>
        await _mediator.Send(new Core.Application.Product.Elastic.Query.GetById.GetProductByIdQuery(id));

    [GraphQLName("getElasticProductsByName")]
    public async Task<IReadOnlyList<ProductDocument>> GetElasticProductsByName(string name) =>
        await _mediator.Send(new Core.Application.Product.Elastic.Query.GetByName.GetProductByNameQuery(name));

    //TODO pagination
    //[GraphQLName("getAllProducts")]
    //public async Task<IReadOnlyList<PagedResult<GetListProductQueryResult>>> GetAllProducts() =>
    //(IReadOnlyList<PagedResult<GetListProductQueryResult>>)await _mediator.Send(new GetListProductQuery());

    [GraphQLName("getProductById")]
    public async Task<GetProductByIdQueryResult> GetProductById(Guid id) =>
        await _mediator.Send(new Core.Application.Product.Repository.Query.Get.GetProductByIdQuery(id));

    [GraphQLName("getProductsByName")]
    public async Task<IReadOnlyList<GetProductByNameQueryResult>> GetProductsByName(string name) =>
        await _mediator.Send(new Core.Application.Product.Repository.Query.GetName.GetProductByNameQuery(name));
}