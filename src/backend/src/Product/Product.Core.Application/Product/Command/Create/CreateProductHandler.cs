using MediatR;
using FluentValidation;
using Product.Core.Domain.Repository;
using AutoMapper;
using Base.Infrastructure.Messaging;
using Product.Core.Application.Product.Event.Create;
using Product.Core.Application.Product.Elastic.EventElastic.Create;

namespace Product.Core.Application.Product.Command.Create
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateProductHandler(IMediator mediator, IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateProductCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (validationResult != null && !validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var record = _mapper.Map<ProductCoreDomainEntities.Product>(command);

            var created = await _repository.SaveAsync(record);
            var result = _mapper.Map<CreateProductResult>(created);

            await _mediator.Publish(new CreateProductResult
            {
                Id = result.Id,
                Price = result.Price,
                Name = record.Name
            });

            //could use mediator with inotification
            await _mediator.Publish(new ProductCreatedEvent(result.Id, result.Name, result.Price));

            return result;
        }
    }
}
