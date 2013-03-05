using System.Collections.Generic;

namespace TS.FluentSiteMap.Sample.Models
{
    public interface IProductRepository
    {
        IEnumerable<Product> FetchProducts();
    }
}