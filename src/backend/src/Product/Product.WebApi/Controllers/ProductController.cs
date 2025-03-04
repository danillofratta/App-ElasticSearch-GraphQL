using AutoMapper;
using Base.WebApi;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Product.Core.Application.Product.Elastic.Query.GetById;
using Product.Core.Application.Product.Repository.Command.Create;
using Product.Core.Application.Product.Repository.Command.Delete;
using Product.Core.Application.Product.Repository.Command.Modify;
using Product.Core.Application.Product.Repository.Query.Get;
using Product.Core.Application.Product.Repository.Query.GetName;
using Product.Core.Domain.Repository;
using System.Threading;

namespace ApiStock.Controller
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly IElasticClient _elasticClient;

        public ProductController(IElasticClient elasticClient, IMapper mapper, IMediator mediator, IProductRepository repository)
        {
            this._mediator = mediator;
            this._repository = repository;
            this._mapper = mapper;
            this._elasticClient = elasticClient;
        }

        /// <summary>
        /// TODO return with ApiResponseWithData
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //todo 
            return Ok(await _repository.GetAllAsync());            
        }

        /// <summary>
        /// TODO return with ApiResponseWithData
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {                
                var response = await _mediator.Send(new Product.Core.Application.Product.Repository.Query.Get.GetProductByIdQuery(id), cancellationToken);

                return Ok(new ApiResponseWithData<GetProductByIdQueryResult>
                {
                    Success = true,
                    Message = "Product retrieved successfully",
                    Data = _mapper.Map<GetProductByIdQueryResult>(response)
                });
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponseWithData<GetProductByIdQueryResult>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// TODO return with ApiResponseWithData
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("getbyname/{name}")]
        public async Task<IActionResult> GetByName([FromRoute] string name, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _mediator.Send(new Product.Core.Application.Product.Repository.Query.GetName.GetProductByNameQuery(name), cancellationToken);

                return Ok(new ApiResponseWithData<List<GetProductByNameQueryResult>>
                {
                    Success = true,
                    Message = "Product retrieved successfully",
                    Data = _mapper.Map<List<GetProductByNameQueryResult>>(response)
                });
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponseWithData<GetProductByNameQueryResult>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// Apenas para testar
        /// </summary>
        /// <returns></returns>
        [HttpPost("loadelastic")]
        public async Task<IActionResult> LoadElastic()
        {
            var products = await _repository.GetAllAsync();
            if (products == null || !products.Any())
            {
                return BadRequest("Nenhum produto encontrado no banco de dados.");
            }

            var bulkIndexResponse = await _elasticClient.IndexManyAsync(products);
            //await _elasticClient.Indices.RefreshAsync("products");

            if (bulkIndexResponse.Errors)
            {
                foreach (var item in bulkIndexResponse.ItemsWithErrors)
                {
                    Console.WriteLine($"Erro ao indexar {item.Id}: {item.Error}");
                }
                return StatusCode(500, "Erro ao indexar produtos no Elasticsearch.");
            }

            return Ok($"Indexados {products.Count} produtos no Elasticsearch.");
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);

                return Created(string.Empty, new ApiResponseWithData<CreateProductResult>
                {
                    Success = true,
                    Message = "Product created successfully",
                    Data = _mapper.Map<CreateProductResult>(response)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseWithData<CreateProductResult>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(ModifyProductCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);

                return Created(string.Empty, new ApiResponseWithData<ModifyProductResult>
                {
                    Success = true,
                    Message = "Product modified successfully",
                    Data = _mapper.Map<ModifyProductResult>(response)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseWithData<ModifyProductResult>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// TODO return with ApiResponseWithData
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var obj = new DeleteProductCommand { Id = id };
            var result = await _mediator.Send(obj);
            return Ok(result);
        }
    }
}
