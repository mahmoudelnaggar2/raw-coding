using Microsoft.AspNetCore.Identity;

namespace Angular.Endpoints;

public class LoginEndpoint
{
    public class LoginForm
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public static async Task<IResult> Handler(
        LoginForm form,
        SignInManager<IdentityUser> signInManager
    )
    {
        var result = await signInManager.PasswordSignInAsync(form.Username, form.Password, true, false);
        if (result.Succeeded)
        {
            return Results.Ok();
        }

        return Results.BadRequest();
    }
}