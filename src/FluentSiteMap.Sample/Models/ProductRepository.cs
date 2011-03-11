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
                                 Description = "Foo Widgets are big",
                                 ImageName = "Foo.png"
                             };

            yield return new Product
                             {
                                 Id = 101,
                                 Name = "Bar Widget",
                                 Description = "Bar Widgets are really big",
                                 ImageName = "Bar.png"
                             };

            yield return new Product
                             {
                                 Id = 102,
                                 Name = "Baz Widget",
                                 Description = "Baz Widgets are kinda small",
                                 ImageName = "Baz.png"
                             };
        }
    }
}