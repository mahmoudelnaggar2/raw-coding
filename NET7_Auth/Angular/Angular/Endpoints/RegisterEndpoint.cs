using Microsoft.AspNetCore.Identity;

namespace Angular.Endpoints;

public class RegisterEndpoint
{
    public class RegisterForm
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public static async Task<IResult> Handler(
        RegisterForm form,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager
    )
    {
        if (form.Password != form.ConfirmPassword)
        {
            return Results.BadRequest();
        }

        var user = new IdentityUser() { UserName = form.Username };
        var createUserResult = await userManager.CreateAsync(user, form.Password);
        if (!createUserResult.Succeeded)
        {
            return Results.BadRequest();
        }

        await signInManager.SignInAsync(user, true);
        return Results.Ok();
    }
}