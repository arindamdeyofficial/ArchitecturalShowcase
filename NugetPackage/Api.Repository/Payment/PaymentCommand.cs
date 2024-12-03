using Api.Repository.Models;
using AutoMapper;
using BusinessModel.Common;
using BusinessModel.Context;
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
    public class PaymentCommand : BaseCommand<PaymentBo, BaseResponse>
    {
        private IConfigurationRoot _configRoot;
        private readonly ILoggerHelper _logger;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMongoDbHelper<PaymentBo> _mongoHelper;
        private readonly IMediator _mediator;
        private readonly ICommandContext<PaymentDbContext> _dbPaymentCommandContext;
        private readonly PaymentDbContext _dbPaymentContext;
        private readonly IMapper _mapper;
        private readonly IValidator<PaymentBo> _validator;

        public PaymentCommand(ILoggerHelper logger, IConfiguration configRoot
            , IExceptionHelper exceptionHelper, IMongoDbHelper<PaymentBo> mongoHelper
            , IMediator mediator, IMapper mapper, IValidator<PaymentBo> validator
            , ICommandContext<PaymentDbContext> dbPaymentContext): base(validator)
        {
            _configRoot = (IConfigurationRoot)configRoot;
            _logger = logger;
            _exceptionHelper = exceptionHelper;
            _mongoHelper = mongoHelper;
            _mediator = mediator;
            _dbPaymentCommandContext = dbPaymentContext;
            _dbPaymentContext = _dbPaymentCommandContext.GetDbContext();
            _mapper = mapper;
            _validator = validator;
        }

        protected override async Task<BaseResponse> Handle(PaymentBo request, CancellationToken cancellationToken)
        {
            var res = new BaseResponse { IsSuccess = true };

            var entry = await _dbPaymentContext.Payments.AddAsync(_mapper.Map <PaymentBo, Api.Repository.Models.Payment>(request));
            if (entry.State != EntityState.Added)
            {
                res.IsSuccess = false;
                res.Message = "Failed to track entity in DbContext.";
            }
            int affectedRows = await _dbPaymentContext.SaveChangesAsync();
            return (affectedRows > 0)? new BaseResponse { Status = System.Net.HttpStatusCode.OK, Message = "Success"}
                : new BaseResponse { Status = System.Net.HttpStatusCode.ExpectationFailed, Message = "Failure" }; 
        }
    }
}
