using Api.Repository.Bo.Product;
using BusinessModel.Interface.Common;
using BusinessModel.Product;
using MediatR;

namespace Api.Orchestrator.AR.Product
{
    public class GetPrdQueryHandler(IBaseQuery<ProductBo, ProductBo> prdQuery) : IRequestHandler<GetPrdRequestQuery, ProductBo>
    {
        private readonly IBaseQuery<ProductBo, ProductBo> _query = prdQuery;

        public async Task<ProductBo> Handle(GetPrdRequestQuery request, CancellationToken cancellationToken)
        {
            return _query.Execute(request.prdi, cancellationToken).Result;
        }
    }
}
