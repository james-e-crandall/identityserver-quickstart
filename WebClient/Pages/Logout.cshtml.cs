using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    [Authorize]
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult > OnGet()
        {
            await HttpContext.SignOutAsync();

            // Redirects to Privacy.cshtml
            return RedirectToPage("Index"); 

        }
    }
}
