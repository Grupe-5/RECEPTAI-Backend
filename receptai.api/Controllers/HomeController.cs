using Microsoft.AspNetCore.Mvc;

namespace receptai.api;

public class HomeController : Controller
{
    // private readonly string _serve_path;
    public HomeController() {
        // _serve_path = configuration.GetValue<string?>("ServeFrontend:ServeDir") ?? "";
    }

    public IActionResult Index()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html");
        return PhysicalFile(path, "text/HTML");
    }
}
