using Api.Repository.Bo.Product;
using BusinessModel.Common;
using BusinessModel.Product;
using CustomLoggerHelper;
using ExceptionHandlerCustom;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using MongoConnect;
using System.Runtime.CompilerServices;

namespace Api.Repository.Product
{
    public class PrdGetAllQuery : BaseStreamQuery<GetPrd, ProductBo>
    {
        private IConfigurationRoot _configRoot;
        private readonly ILoggerHelper _logger;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMongoDbHelper<ProductBo> _mongoHelper;
        private readonly IMediator _mediator;
        private readonly IValidator<GetPrd> _validator;

        public PrdGetAllQuery(ILoggerHelper logger, IConfiguration configRoot
            , IExceptionHelper exceptionHelper, IMongoDbHelper<ProductBo> mongoHelper
            , IMediator mediator, IValidator<GetPrd> validator) : base(validator)
        {
            _configRoot = (IConfigurationRoot)configRoot;
            _logger = logger;
            _exceptionHelper = exceptionHelper;
            _mongoHelper = mongoHelper;
            _mediator = mediator;
            _validator = validator;
        }

        protected async override IAsyncEnumerable<ProductBo> HandleStreamQuery(GetPrd request, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var prd in _mongoHelper.GetAllAsync(cancellationToken))
            {
                yield return prd;
            }
        }
    }
}
