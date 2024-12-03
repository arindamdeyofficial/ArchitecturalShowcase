using Api.Repository.Bo.Product;
using BusinessModel.Interface.Common;
using BusinessModel.Product;
using MediatR;

namespace Api.Orchestrator.AR.Product
{
    public class CreatePrdCommandHandler(IBaseCommand<ProductBo, bool> cmd) : IRequestHandler<CreatePrdRequestQuery, bool>
    {
        private readonly IBaseCommand<ProductBo, bool> _cmd = cmd;

        public async Task<bool> Handle(CreatePrdRequestQuery request, CancellationToken cancellationToken)
        {
            return _cmd.Execute(request.prd, cancellationToken).Result;
        }
    }
}
