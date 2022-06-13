using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Loxy.Pages;

public class LoxyModel : PageModel
{
    public void OnGet()
    {
        ViewData["Title"] = "Loxy Proxy";
    }
}