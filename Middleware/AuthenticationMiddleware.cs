

using System.Text;

namespace BankAPI.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _relm;
        public AuthenticationMiddleware(RequestDelegate next,string relm) 
        {
             _next = next;
            _relm = relm;
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

            if (uid != "john" || password != "password")
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await _next(context);
        }
    }
}
