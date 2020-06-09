using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payspace.API.Models;
using Payspace.Domain;

namespace Payspace.API.Controllers
{
    public class PostalCodeController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<PostalCodeController> _logger;

        public PostalCodeController(AppDbContext dbContext, ILogger<PostalCodeController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostalCode>>> GetPostalCodes()
        {
            return await _dbContext.PostalCodeMappings.ToListAsync();
        }

        [HttpGet("{postalCode}")]
        public async Task<ActionResult<PostalCode>> GetPostalCode(string postalCode)
        {
            return await _dbContext.PostalCodeMappings.FirstOrDefaultAsync(x => x.Code == postalCode);
        }

        [HttpDelete]
        [Authorize(Roles = "SYSTEM")]
        public async Task<ActionResult> DeletePostalCode(long id)
        {
            var postalCode = _dbContext.PostalCodeMappings.Find(id);
            if (postalCode == null)
                return NotFound();
            _dbContext.PostalCodeMappings.Remove(postalCode);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "SYSTEM")]
        public async Task<ActionResult<PostalCode>> PostPostalCode(PostalCode postalCode)
        {
            _dbContext.Add(postalCode);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetPostalCodes", null);
        }
    }
}