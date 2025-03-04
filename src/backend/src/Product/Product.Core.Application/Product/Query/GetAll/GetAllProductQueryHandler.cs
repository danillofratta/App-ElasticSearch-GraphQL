using AutoMapper;
using MediatR;
using Product.Core.Domain.Repository;


namespace Product.Core.Application.Product.Query.GetAll;
public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, PagedResult<GetAllProductQueryResult>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public GetAllProductQueryHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResult<GetAllProductQueryResult>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        var (Products, totalCount) = await _repository.GetPagedAsync(
            pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            orderBy: request.OrderBy,
            isDescending: request.IsDescending,
            cancellationToken: cancellationToken
        );

        var dtos = _mapper.Map<List<GetAllProductQueryResult>>(Products);

        return new PagedResult<GetAllProductQueryResult>
        {
            Items = dtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }
}


