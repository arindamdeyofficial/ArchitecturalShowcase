
using MediatR;
using BusinessModel.Payment;

namespace Api.Repository.Bo.Payment
{
    public record GetPaymentAllRequestQuery(PaymentBo pay) : IStreamRequest<PaymentBo>
    {
    }
}
