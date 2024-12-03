using BusinessModel.Product;
using MediatR;

namespace Api.Repository.Bo.Product
{
    public record CreatePrdRequestQuery(ProductBo prd) : IRequest<bool>
    {
    }
}
