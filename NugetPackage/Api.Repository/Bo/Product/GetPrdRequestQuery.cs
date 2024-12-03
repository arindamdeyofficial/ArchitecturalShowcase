using BusinessModel.Product;
using MediatR;

namespace Api.Repository.Bo.Product
{
    public record GetPrdRequestQuery(ProductBo prdi) : IRequest<ProductBo>
    {
    }
}
