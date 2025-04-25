using KarmaWebAPI.Data;
using KarmaWebAPI.Serveis;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;

namespace KarmaWebAPI.Configurations
{
    public static class ServiceExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            //Ací falta afegir totes les comprobacions sobre l'usuari que 
            var builder = services.AddIdentityCore<ApiUser>(q => q.User.RequireUniqueEmail = true);

            builder = new Microsoft.AspNetCore.Identity.IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddRoles<IdentityRole>()
                   .AddEntityFrameworkStores<DatabaseContext>()
                   .AddDefaultTokenProviders();
            builder.AddSignInManager<SignInManager<ApiUser>>();
        }

        public static void ConfigureAuthentication(this IServiceCollection services) { 
            
            var builder = services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme; //CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme; //CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
                })
                .AddCookie(IdentityConstants.ApplicationScheme, options =>
                {
                        options.LoginPath = "/api/account/login";
                        options.AccessDeniedPath = "/api/account/accessdenied";
                    });
        }

        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            var builder = services.AddAuthorization(options =>
            {
                // Configura tus políticas de autorización aquí
            });
        }


        public static void ConfigureServices(this IServiceCollection services)
        {
            var builder = services.AddScoped<IAnyEscolarService, AnyEscolarService>()
                                   .AddScoped<IPrivilegiService, PrivilegiService>();
            // Otros servicios
        }



    }
}
