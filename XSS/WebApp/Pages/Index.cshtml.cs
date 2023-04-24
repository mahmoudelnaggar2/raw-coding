using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class Index : PageModel
{
    public void OnGet(string? message = "")
    {
        Message = message;
    }

    public string? Message { get; set; }
}