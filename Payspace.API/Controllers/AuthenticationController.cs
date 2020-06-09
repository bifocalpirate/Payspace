using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Payspace.API.Models;
using Payspace.API.Models.DTO;
using Payspace.Domain.Configuration;

namespace Payspace.API.Controllers
{
    public class AuthenticationController : BaseController
    {
        private readonly IOptionsSnapshot<AppSettings> _appSettings;
        private readonly AppDbContext _dbContext;
        private readonly IOptionsSnapshot<JWTOptions> _jwtOptions;

        public AuthenticationController(IOptionsSnapshot<AppSettings> appSettings,
                                IOptionsSnapshot<JWTOptions> jwtOptions,
                                AppDbContext context)
        {
            _dbContext = context;
            _appSettings = appSettings;
            _jwtOptions = jwtOptions;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return Ok("Please login.");
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserForAuthentication user)
        {
            return Ok();
        }
    }
}