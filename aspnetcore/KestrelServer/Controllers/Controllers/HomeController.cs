using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet("/")]
    public string Get() => "Hello World!";
}