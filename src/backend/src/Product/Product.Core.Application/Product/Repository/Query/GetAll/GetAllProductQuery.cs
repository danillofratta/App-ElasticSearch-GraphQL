using MediatR;


namespace Product.Core.Application.Product.Repository.Query.GetAll;
public class GetAllProductQuery : IRequest<PagedResult<GetAllProductQueryResult>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? OrderBy { get; set; }
    public bool IsDescending { get; set; }
}
