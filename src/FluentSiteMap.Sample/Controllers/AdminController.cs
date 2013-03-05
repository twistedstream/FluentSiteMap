using System.Web.Mvc;

namespace TS.FluentSiteMap.Sample.Controllers
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