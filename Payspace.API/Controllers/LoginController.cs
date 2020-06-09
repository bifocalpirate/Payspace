using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Payspace.API.Models;
using Payspace.API.Models.DTO;
using Payspace.Domain.Configuration;
using Payspace.Domain.Security;

namespace Payspace.API.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IOptionsSnapshot<AppSettings> _appSettings;
        private readonly AppDbContext _context;
        private readonly IOptionsSnapshot<JWTOptions> _jwtOptions;

        public LoginController(IOptionsSnapshot<AppSettings> appSettings, AppDbContext context,
                                IOptionsSnapshot<JWTOptions> jwtOptions)
        {
            _context = context;
            _appSettings = appSettings;
            _jwtOptions = jwtOptions;
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserForAuthentication login)
        {
            IActionResult response = Unauthorized("Invalid username or password.");
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = new JsonResult(new
                {
                    user.Id,
                    JsonToken = tokenString  ,
                    Roles = user.Roles
                });
            }

            return response;
        }
        private void SetRoles(UserForAuthentication user, List<Claim> claims)
        {
            var userRoles = _context.UserRoles.Include("Role").Where(x => x.UserId == user.Id);
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }
        private IEnumerable<Claim> GetClaims(UserForAuthentication user)
        {

            var claims = new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.Email, user.EmailAddress),
                new Claim(JwtRegisteredClaimNames.NameId,user.Id.ToString()),               
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            SetRoles(user, claims);
            return claims;
        }
        private string GenerateJSONWebToken(UserForAuthentication userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_jwtOptions.Value.Issuer,
              _jwtOptions.Value.Issuer,
              GetClaims(userInfo),
              expires: DateTime.Now.AddHours(_appSettings.Value.TokenTimeOutInHours),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserForAuthentication AuthenticateUser(UserForAuthentication login)
        {
            UserForAuthentication user = null;
            var userAccount = _context.Users.FirstOrDefault(u => u.EmailAddress == login.EmailAddress && u.PasswordHash == login.PasswordHash);
            
            if (userAccount != null)
            {
                var userRoles = _context.UserRoles.Include("Role").Where(r => r.UserId == userAccount.Id);
                user = new UserForAuthentication { EmailAddress = userAccount.EmailAddress, Id = userAccount.Id };
                user.Roles = userRoles.Select(x => x.Role.RoleName).ToArray();
            }
            return user;
        }
    }
}
