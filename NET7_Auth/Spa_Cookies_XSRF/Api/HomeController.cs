using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Api;

public class HomeController : ControllerBase
{
    [HttpPost("/ctr/login")]
    public IActionResult Login([FromForm] LoginForm form)
    {
        if (!HttpContext.Request.Headers.Origin.First().StartsWith("https://app.company.local"))
        {
            return BadRequest();
        }
        
        HttpContext.SignInAsync(new ClaimsPrincipal(new[]
        {
            new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, form.Username),
                },
                CookieAuthenticationDefaults.AuthenticationScheme
            )
        }));

        return Ok();
    }
}