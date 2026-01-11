using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Payspace.API.Models;
using Payspace.Domain;
using Payspace.Domain.Configuration;
using Payspace.Messaging.Http;
using Payspace.UI.Controllers;
using Payspace.UI.Facades;
using Payspace.UI.Models.DTO;
using PayspaceDemo.UI.Models;

namespace PayspaceDemo.UI.Controllers
{
    //i'll just put everything into the same controller
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepositoryFacade _apiFacade;
        private readonly string _jwtToken;

        public HomeController(ILogger<HomeController> logger, 
            IRepositoryFacade apiFacade,
            IOptionsSnapshot<AppSettings> appSettings, 
            IMessager messager, 
            IHttpContextAccessor contextAccessor):base(appSettings,messager,contextAccessor)
        {
            _logger = logger;
            var sid = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid);

            _apiFacade = apiFacade;
            
            if (sid != null)
            {
                _jwtToken = sid.Value;
                _apiFacade.SetJWTToken(_jwtToken);
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles ="ADMIN, SYSTEM")]
        public IActionResult AddTaxRate([FromForm] ProgressiveTaxRate taxRate)
        {
            if (taxRate.From < 0 || taxRate.Rate <= 0 || taxRate.To < 0)
            {
                //only update if data is valid. no time to send message.
                _logger.LogWarning("Invalid Tax Data for Add.");
                ViewBag.Message = "Record not added as it failed validation.";
            }
            else
            {
                _apiFacade.AddProgressiveTaxRate(taxRate); 
            }
            return RedirectToAction("ShowTaxTables");
        }
        [HttpPost("{id}")]
        [Authorize(Roles ="ADMIN, SYSTEM")]
        public IActionResult UpdateTaxRate([FromForm] ProgressiveTaxRate taxRate, long id)
        {
            if (taxRate.From < 0 || taxRate.Rate <= 0 || taxRate.To < 0)
            {
                //only update if data is valid. no time to send message.
                _logger.LogWarning($"Attempt to update tax rate {id} to invalid field values.");
                ViewBag.Message = "Record not updated as it failed validation.";
            }
            else
            {
                _apiFacade.UpdateProgressiveTaxRate(taxRate);
            }
            
            return RedirectToAction("ShowTaxTables");
        }
        [HttpGet("{id}")]
        [Authorize(Roles ="ADMIN, SYSTEM")]
        public IActionResult DeleteProgressiveTaxRate(long id)
        {
            _apiFacade.DeleteProgressiveTaxRate(id);
            return RedirectToAction(nameof(ShowTaxTables));
        }
        [Authorize]
        [Authorize(Roles = "SYSTEM")]
        public IActionResult TaxCalculationEventLogs()
        {
            //here i should be using AutoMap
            var data = _apiFacade.GetTaxCalculationEvents().Select(x => new Payspace.UI.Models.DTO.TaxCalculationEvent()
            {
                AssessedAmount = x.AssessedAmount,           
                PostalCode = x.PostalCode,
                TaxableAmount = x.TaxableAmount,
                TaxRegime = x.TaxRegime,             
                UserId = x.UserId
            });
            return View(data); 
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Calculation()
        {
            return View();
        }
        [HttpGet]    
        public IActionResult ShowTaxTables()
        {
            var data = _apiFacade.GetProgressiveTaxRates();
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcessCalculation([FromForm] TaxCalculationRequest request)
        {
            var result = _apiFacade.CalculateTax(new CalculationRequest()
            {
                //user id gets set via the auth token (if i get that far)
                PostalCode = request.PostalCode,
                TaxableAmount = request.TaxableAmount,
                UserId = UserId
            });

            ViewBag.Message = string.IsNullOrWhiteSpace(result.ErrorMessage)? $"The assessed tax is {result.AssessedAmount.ToString("##0.00")}":result.ErrorMessage;
            
            return View("Calculation");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
