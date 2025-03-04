using MediatR;
using FluentValidation;
using AutoMapper;
using Product.Core.Domain.Repository;
using Product.Core.Application.Product.Elastic.EventElastic.Create;
using Base.Infrastructure.Messaging;

namespace Product.Core.Application.Product.Command.Modify
{
    public class ModifyProductHandler : IRequestHandler<ModifyProductCommand, ModifyProductResult>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public ModifyProductHandler(IMediator mediator, IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ModifyProductResult> Handle(ModifyProductCommand command, CancellationToken cancellationToken)
        {
            var validator = new ModifyProductCommandValidator(_repository);
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (validationResult != null && !validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var record = _mapper.Map<ProductCoreDomainEntities.Product>(command);

            var update = await _repository.UpdateAsync(record);
            var result = _mapper.Map<ModifyProductResult>(update);

            //could use mediator with inotification
            await _mediator.Publish(new ProductModifyEvent(update.Id, update.Name, update.Price));

            return result;
        }
    }
}
