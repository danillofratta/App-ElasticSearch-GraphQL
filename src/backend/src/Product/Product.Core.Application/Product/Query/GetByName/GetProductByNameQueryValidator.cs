using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;


namespace Product.Core.Application.Product.Query.GetByName
{
    public class GetProductByNameQueryValidator : AbstractValidator<GetProductByNameQuery>
    {
        public GetProductByNameQueryValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
