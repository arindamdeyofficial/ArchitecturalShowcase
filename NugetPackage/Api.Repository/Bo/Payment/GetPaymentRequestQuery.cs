
using MediatR;
using BusinessModel.Payment;

namespace Api.Repository.Bo.Payment
{
    public record GetPaymentRequestQuery(PaymentBo pay) : IRequest<PaymentBo>
    {
    }
}
