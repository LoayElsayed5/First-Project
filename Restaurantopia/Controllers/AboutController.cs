using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Restaurantopia.Controllers
{
    public class AboutController : Controller
    {
        // GET: AboutController
        public ActionResult Index ()
        {
            return View ();
        }

    }
}
