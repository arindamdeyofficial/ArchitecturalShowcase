using Api.Repository.Bo.Product;
using BusinessModel.Interface;
using BusinessModel.Interface.Common;
using BusinessModel.Product;
using MediatR;
using System.Runtime.CompilerServices;

namespace Api.Orchestrator.AR.Product
{
    public class GetPrdQueryAllHandler(IBaseStreamQuery<GetPrd, ProductBo> queryAll) : IStreamRequestHandler<GetPrdAllRequestQuery, ProductBo>
    {
        private readonly IBaseStreamQuery<GetPrd, ProductBo> _queryAll = queryAll;

        public async IAsyncEnumerable<ProductBo> Handle(GetPrdAllRequestQuery request, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var item in _queryAll.ExecuteStreamQueryAsync(new GetPrd(), cancellationToken))
            {
                yield return item;
            }
        }
    }
}
