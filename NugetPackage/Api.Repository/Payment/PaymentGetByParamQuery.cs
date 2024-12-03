using Api.Repository.Models;
using AutoMapper;
using BusinessModel.Common;
using BusinessModel.Payment;
using CustomLoggerHelper;
using ExceptionHandlerCustom;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoConnect;

namespace Api.Repository.Payment
{
    public class PaymentGetByParamQuery : BaseQuery<PaymentBo, PaymentBo>
    {
        private IConfigurationRoot _configRoot;
        private readonly ILoggerHelper _logger;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMongoDbHelper<PaymentBo> _mongoHelper;
        private readonly IMediator _mediator;
        private readonly IValidator<PaymentBo> _validator;
        private readonly PaymentDbContext _context;
        private readonly IMapper _mapper;

        public PaymentGetByParamQuery(ILoggerHelper logger, IConfiguration configRoot
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

        protected async override Task<PaymentBo> Handle(PaymentBo request, CancellationToken cancellationToken)
        {
            return _mapper.Map<Api.Repository.Models.Payment, PaymentBo>(await _context.Payments.FirstOrDefaultAsync(a => a.OutToken.Equals(request.OutToken), cancellationToken));
        }
    }
}
