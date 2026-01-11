using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Payspace.API.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error() => Problem();
        [Route("/error-local-development")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ErrorLocalDevelopment([FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException("This should not be invoked in non-development environments.");
            }
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            return Problem(
                detail: context.Error.StackTrace,
                title: context.Error.Message
            );
        }
    }
}