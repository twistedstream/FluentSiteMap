using System.Linq;
using System.Web.Mvc;
using TS.FluentSiteMap.Sample.Models;

namespace TS.FluentSiteMap.Sample.Controllers
{
    [HandleError]
    public class ProductsController : Controller
    {
        private IProductRepository _repository;

        public ProductsController()
        {
            _repository = new ProductRepository();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult View(int id)
        {
            var product = _repository
                .FetchProducts()
                .Where(p => p.Id == id)
                .Single();

            return View(product);
        }
    }
}