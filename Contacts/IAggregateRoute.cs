using ProductCatalogue.Contacts;
using System;
using System.Threading.Tasks;

namespace ProductCatalogue.AggregateRoute
{    
    public interface IAggregateRoute
    {
        Task<BaseResponse> Execute(ProductContract obj);
    }
}
