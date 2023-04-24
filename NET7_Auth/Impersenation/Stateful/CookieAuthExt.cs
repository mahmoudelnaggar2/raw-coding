using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Stateful;

public static class CookieAuthExt
{
    public static AuthenticationBuilder AddMyCookie(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<CookieAuthenticationOptions> configureOptions)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<CookieAuthenticationOptions>, PostConfigureCookieAuthenticationOptions>());
        builder.Services.AddOptions<CookieAuthenticationOptions>(authenticationScheme).Validate(o => o.Cookie.Expiration == null, "Cookie.Expiration is ignored, use ExpireTimeSpan instead.");
        return builder.AddScheme<CookieAuthenticationOptions, CookieAuthenticationHandlerExt>(authenticationScheme, displayName, configureOptions);
    }
}