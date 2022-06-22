using Microsoft.AspNetCore.Mvc;

namespace SenseMining.FPG.API.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return Redirect("~/help");
    }
}