using Microsoft.AspNetCore.Mvc;

namespace VaultEdge.Api.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
