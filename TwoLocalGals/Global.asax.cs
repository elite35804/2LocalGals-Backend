using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TwoLocalGals
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //AreaRegistration.RegisterAllAreas();
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //Database.LogThis("Application_Start", null);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //Database.LogThis("Session_Start", null);
        }

        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    //Database.LogThis("Application_BeginRequest", null);
        //}

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.StatusCode = 200;
                HttpContext.Current.Response.End();
            }


        }


        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            var authHeader = HttpContext.Current.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();

                try
                {
                    var key = Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes("@#2LocalGals1qaz!QAZ"))); 
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateLifetime = true
                    };

                    SecurityToken validatedToken;
                    var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                    // Set the principal for the current request
                    HttpContext.Current.User = principal;
                    Thread.CurrentPrincipal = principal;
                }
                catch (Exception)
                {
                    // Token validation failed, user will remain unauthenticated
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Database.LogThis("Application_Error", null);
        }

        protected void Session_End(object sender, EventArgs e)
        {
            //Database.LogThis("Session_End", null);
        }

        protected void Application_End(object sender, EventArgs e)
        {
            //Database.LogThis("Application_End", null);
        }
    }
}