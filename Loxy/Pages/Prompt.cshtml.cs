using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Loxy.Pages;

public class PromptModel : PageModel
{
    public string Message { get; set; } = "Input required";
    public string ReturnUrl { get; set; }

    public void OnGet(string message, string returnUrl)
    {
        if (!string.IsNullOrWhiteSpace(message))
            Message = message;

        ReturnUrl = returnUrl;
    }
}