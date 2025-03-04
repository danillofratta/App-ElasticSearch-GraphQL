
using MediatR;
using Product.Core.Application.Product.Event.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Core.Application.Product.Elastic.Query.GetAll;

public class GetAllProductQuery : IRequest<IReadOnlyList<ProductDocument>>
{

}