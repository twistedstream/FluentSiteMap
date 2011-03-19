using System.Collections.Generic;

namespace FluentSiteMap.Sample.Models
{
    public class ProductRepository : IProductRepository
    {
        public IEnumerable<Product> FetchProducts()
        {
            yield return new Product
                             {
                                 Id = 100,
                                 Name = "Foo Widget",
                                 Description = "Foo Widgets are spendy",
                                 Price = 100
                             };

            yield return new Product
                             {
                                 Id = 101,
                                 Name = "Bar Widget",
                                 Description = "Bar Widgets are really spendy",
                                 Price = 150
                             };

            yield return new Product
                             {
                                 Id = 102,
                                 Name = "Baz Widget",
                                 Description = "Baz Widgets are pretty cheap",
                                 Price = 25
                             };
        }
    }
}