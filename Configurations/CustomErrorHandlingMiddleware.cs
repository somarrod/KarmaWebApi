using Newtonsoft.Json;
using System.Net;

namespace KarmaWebAPI.Configurations
{
    public class CustomErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);


                if (context.Response.StatusCode == (int)HttpStatusCode.Found)
                {
                    await HandleRedirectAsync(context);
                }

                if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                {
                    await HandleForbiddenAsync(context);
                }
        
                if (context.Response.StatusCode == (int)HttpStatusCode.MethodNotAllowed)
                {
                    await HandleMethodNotAllowedAsync(context);
                }

            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleMethodNotAllowedAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new { error = "Mètode no permés" });
            return context.Response.WriteAsync(result);
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = JsonConvert.SerializeObject(new { error = exception.Message });
            return context.Response.WriteAsync(result);
        }

        private static Task HandleForbiddenAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new { error = "No té permisos suficients per a realitzar l'acció." });
            return context.Response.WriteAsync(result);
        }


        private static Task HandleRedirectAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new { error = "No té permisos per a accedir al recurs." });
            return context.Response.WriteAsync(result);
        }
    }

}



