using Api.Repository.Bo.Payment;
using BusinessModel.Interface.Common;
using BusinessModel.Payment;
using MediatR;
using System.Runtime.CompilerServices;

namespace Api.Orchestrator.AR.Payment
{
    public class GetPaymentQueryAllHandler(IBaseStreamQuery<PaymentBo, PaymentBo> queryAll) : IStreamRequestHandler<GetPaymentAllRequestQuery, PaymentBo>
    {
        private readonly IBaseStreamQuery<PaymentBo, PaymentBo> _queryAll = queryAll;

        public async IAsyncEnumerable<PaymentBo> Handle(GetPaymentAllRequestQuery request, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var item in _queryAll.ExecuteStreamQueryAsync(request.pay, cancellationToken))
            {
                yield return item;
            }
        }
    }
}
