using System.Web.Mvc;

namespace FluentSiteMap.Sample.Controllers
{
    [HandleError]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}