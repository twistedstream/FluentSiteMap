using System.Collections.Generic;

namespace FluentSiteMap.Sample.Models
{
    public interface IProductRepository
    {
        IEnumerable<Product> FetchProducts();
    }
}