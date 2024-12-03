using Api.Repository.Bo.Payment;
using BusinessModel.Common;
using BusinessModel.Interface;
using BusinessModel.Interface.Common;
using BusinessModel.Payment;
using MediatR;

namespace Api.Orchestrator.AR.Payment
{
    public class PayCommandHandler(IBaseCommand<PaymentBo, BaseResponse> cmd) : IRequestHandler<PayRequestCommand, BaseResponse>
    {
        private readonly IBaseCommand<PaymentBo, BaseResponse> _cmd = cmd;

        public async Task<BaseResponse> Handle(PayRequestCommand request, CancellationToken cancellationToken)
        {
            return _cmd.Execute(request.pay, cancellationToken).Result;
        }
    }
}
