
using MediatR;
using BusinessModel.Payment;
using BusinessModel.Common;

namespace Api.Repository.Bo.Payment
{
    public record PayRequestCommand(PaymentBo pay) : IRequest<BaseResponse>
    {
    }
}
