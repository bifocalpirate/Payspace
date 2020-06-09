using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payspace.API.Models;
using Payspace.Domain;

namespace Payspace.API.Controllers
{
    public class ProgressiveTaxRateController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<ProgressiveTaxRateController> _logger;
        public ProgressiveTaxRateController(AppDbContext dbContext, ILogger<ProgressiveTaxRateController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProgressiveTaxRate>>> GetTaxTables()
        {
            //Linq doesn't work that well with the Sqllite driver for the below to have deferred execution
            //return await _dbContext.ProgressiveTaxRates.OrderBy(x=>x.From).ToListAsync();
            var table = await _dbContext.ProgressiveTaxRates.ToListAsync();
            return table.OrderBy(x=>x.From).ToList();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SYSTEM, ADMIN")]
        public async Task<ActionResult>DeleteProgressiveTaxRate(long id)
        {
            var taxRate = _dbContext.ProgressiveTaxRates.Find(id);
            if (taxRate == null)
                return NotFound();
            _dbContext.ProgressiveTaxRates.Remove(taxRate);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "SYSTEM, ADMIN")]
        public async Task<ActionResult<ProgressiveTaxRate>> PostProgressiveTaxRate(ProgressiveTaxRate progressiveTaxRate)
        {
            if (progressiveTaxRate.From < 0 || progressiveTaxRate.To<0 || progressiveTaxRate.Rate < 0)
            {
                return BadRequest("Record failed validation.");
            }
            _dbContext.Add(progressiveTaxRate);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTaxTables),null);
        }

        [HttpPut]
        [Authorize(Roles = "SYSTEM, ADMIN")]
        public async Task<ActionResult> UpdateProgressiveTaxRate(ProgressiveTaxRate progressiveTaxRate)
        {
            var trackedEntity = _dbContext.ProgressiveTaxRates.Find(progressiveTaxRate.Id);
            //use automap?
            trackedEntity.From = progressiveTaxRate.From;
            trackedEntity.To = progressiveTaxRate.To;
            trackedEntity.Rate = progressiveTaxRate.Rate;
            _dbContext.Update(trackedEntity);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}