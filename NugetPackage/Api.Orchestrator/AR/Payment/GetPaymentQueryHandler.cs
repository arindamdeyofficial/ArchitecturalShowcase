using Api.Repository.Bo.Payment;
using Api.Repository.Models;
using BusinessModel.Interface.Common;
using BusinessModel.Payment;
using MediatR;

namespace Api.Orchestrator.AR.Payment
{
    public class GetPaymentQueryHandler(IBaseQuery<PaymentBo, PaymentBo> query) : IRequestHandler<GetPaymentRequestQuery, PaymentBo>
    {
        private readonly IBaseQuery<PaymentBo, PaymentBo> _query = query;

        public async Task<PaymentBo> Handle(GetPaymentRequestQuery request, CancellationToken cancellationToken)
        {
            return _query.Execute(request.pay, cancellationToken).Result;
        }
    }
}
