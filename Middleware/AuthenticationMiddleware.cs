

using System.Text;

namespace BankAPI.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthenticationMiddleware(RequestDelegate next) 
        {
             _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            var header = context.Request.Headers["Authorization"];
            var encodedCredentials = header.ToString().Substring(6);
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            string[] uidpwd = credentials.Split(':');
            var uid = uidpwd[0];
            var password = uidpwd[1];

            if (uid != "karthik" || password != "karthik123")
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await _next(context);
        }
    }
}
