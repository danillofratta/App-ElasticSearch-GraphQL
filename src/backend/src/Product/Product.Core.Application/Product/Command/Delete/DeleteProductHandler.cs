using MediatR;
using FluentValidation;
using AutoMapper;
using Product.Core.Domain.Repository;
using Product.Core.Application.Product.Elastic.EventElastic.Create;

namespace Product.Core.Application.Product.Command.Delete
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, DeleteProductResult>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public DeleteProductHandler(IProductRepository repository, IMapper mapper, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var validator = new DeleteProductCommandValidator(_repository);
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (validationResult != null && !validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var record = await _repository.GetByIdAsync(command.Id);
            var update = await _repository.DeleteAsync(record);

            //could use mediator with inotification
            await _mediator.Publish(new ProductDeleteEvent(command.Id));

            return _mapper.Map<DeleteProductResult>(update); ;
        }
    }
}
