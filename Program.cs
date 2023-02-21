using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;


namespace AspClaimsApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie();

            var app = builder.Build();

            app.UseAuthentication();

            app.MapGet("/login", async (HttpContext context) =>
            {
                ClaimsIdentity identity = new ClaimsIdentity("AuthUser");
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                await context.SignInAsync(principal);
                return Results.Redirect("/");
            });

            app.MapGet("/logout", async (HttpContext context) =>
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            });

            //app.MapGet("/", (HttpContext context) =>
            //{
            //    var user = context.User;
            //    var identity = user.Identity;
            //    if (user is not null && identity.IsAuthenticated)
            //        return $"User indentity. Type: {identity.AuthenticationType}";
            //    else
            //        return $"User not identity";
            //});

            app.MapGet("/", (ClaimsPrincipal principal) =>
            {
                var identity = principal.Identity;
                if (identity is not null && identity.IsAuthenticated)
                    return $"User indentity. Type: {identity.AuthenticationType}";
                else
                    return $"User not identity";
            });

            app.Run();
        }
    }
}