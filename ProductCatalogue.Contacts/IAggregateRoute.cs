using ProductCatalogue.Contacts;
using System;

namespace ProductCatalogue.AggregateRoute
{    
    public interface IAggregateRoute
    {
        BaseResponse Execute(ProductContract obj);
    }
}
