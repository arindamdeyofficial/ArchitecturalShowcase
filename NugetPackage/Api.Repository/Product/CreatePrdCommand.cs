using BusinessModel.Common;
using BusinessModel.Product;
using CustomLoggerHelper;
using ExceptionHandlerCustom;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using MongoConnect;

namespace Api.Repository.Product
{
    public class CreatePrdCommand : BaseCommand<ProductBo, bool>
    {
        private IConfigurationRoot _configRoot;
        private readonly ILoggerHelper _logger;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMongoDbHelper<ProductBo> _mongoHelper;
        private readonly IMediator _mediator;
        private readonly IValidator<ProductBo> _validator;

        public CreatePrdCommand(ILoggerHelper logger, IConfiguration configRoot
            , IExceptionHelper exceptionHelper, IMongoDbHelper<ProductBo> mongoHelper
            , IMediator mediator, IValidator<ProductBo> validator) : base(validator)
        {
            _configRoot = (IConfigurationRoot)configRoot;
            _logger = logger;
            _exceptionHelper = exceptionHelper;
            _mongoHelper = mongoHelper;
            _mediator = mediator;
            _validator = validator;
        }

        protected async override Task<bool> Handle(ProductBo prd, CancellationToken cancellationToken)
        {
            bool isInserted = true;
            try
            {
                isInserted = await _mongoHelper.InsertAsync(prd);
            }
            catch (Azure.RequestFailedException ex)
            {
                isInserted = false;
                await _exceptionHelper.HandleExceptionAsync(ex, _logger);
            }
            return isInserted;
        }
    }
}
