using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security;
using Owin;
using Microsoft.IdentityModel.Tokens;
using System.Text;

[assembly: OwinStartup(typeof(TwoLocalGals.Startup))]
namespace TwoLocalGals
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var issuer = "2LocalGals";
            var audience = "Contrators";
            var secretKey = Convert.FromBase64String("@#2LocalGals1qaz!QAZ");

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey)
                }
            });
        }
    }
}
