using BusinessModel.Product;
using MediatR;

namespace Api.Repository.Bo.Product
{
    public record GetPrdAllRequestQuery(GetPrd prd) : IStreamRequest<ProductBo>
    {
    }
}
