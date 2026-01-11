using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payspace.API.Models;
using Payspace.Domain;

namespace Payspace.API.Controllers
{
    public class TaxEventController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<TaxEventController> _logger;

        public TaxEventController(AppDbContext dbContext, ILogger<TaxEventController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "SYSTEM")]
        public async Task<ActionResult<IEnumerable<TaxCalculationEvent>>> GetCalculationEvents()
        {
            //we'll just return everything, with no filter
            return await _dbContext.TaxCalculationEvents.ToListAsync();
        }
    }
}