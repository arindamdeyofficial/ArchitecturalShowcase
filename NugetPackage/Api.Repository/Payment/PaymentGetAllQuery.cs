using Api.Repository.Models;
using AutoMapper;
using BusinessModel.Common;
using BusinessModel.Payment;
using CustomLoggerHelper;
using ExceptionHandlerCustom;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using MongoConnect;
using System.Runtime.CompilerServices;

namespace Api.Repository.Payment
{
    public class PaymentGetAllQuery : BaseStreamQuery<PaymentBo, PaymentBo>
    {
        private IConfigurationRoot _configRoot;
        private readonly ILoggerHelper _logger;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMongoDbHelper<PaymentBo> _mongoHelper;
        private readonly IMediator _mediator;
        private readonly IValidator<PaymentBo> _validator;
        private readonly PaymentDbContext _context;
        private readonly IMapper _mapper;

        public PaymentGetAllQuery(ILoggerHelper logger, IConfiguration configRoot
            , IExceptionHelper exceptionHelper, IMongoDbHelper<PaymentBo> mongoHelper
            , IMediator mediator, IValidator<PaymentBo> validator
            , PaymentDbContext context, IMapper mapper) : base(validator)
        {
            _configRoot = (IConfigurationRoot)configRoot;
            _logger = logger;
            _exceptionHelper = exceptionHelper;
            _mongoHelper = mongoHelper;
            _mediator = mediator;
            _validator = validator;
            _context = context;
            _mapper = mapper;
        }

        protected async override IAsyncEnumerable<PaymentBo> HandleStreamQuery(PaymentBo request, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var pay in _context.Payments)
            {
                yield return _mapper.Map<Api.Repository.Models.Payment, PaymentBo>(pay);
            }
        }
    }
}
