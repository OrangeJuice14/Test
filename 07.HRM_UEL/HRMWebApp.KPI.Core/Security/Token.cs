using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HRMWebApp.KPI.Core.Security
{
    public class Token
    {
        public string GenerateToken(string Name, string UserId)
        {
            try
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["JWT:IssuerSigningKey"]));

                var claims = new Claim[] {
                    new Claim(ClaimTypes.Name, Name),
                    new Claim(ClaimTypes.Upn, UserId)
                };

                var token = new JwtSecurityToken(
                    issuer: ConfigurationManager.AppSettings["JWT:ValidIssuer"],
                    audience: ConfigurationManager.AppSettings["JWT:ValidAudience"],
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
                );

                string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

                return jwtToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
