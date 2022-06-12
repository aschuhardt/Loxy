using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Loxy.Pages;

public class UsageModel : PageModel
{
    public void OnGet()
    {
        ViewData["Title"] = "Loxy Proxy Usage";
    }
}