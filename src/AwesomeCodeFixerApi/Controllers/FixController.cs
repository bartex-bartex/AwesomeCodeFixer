using System.Net.Mime;
using AwesomeCodeFixerApi.DTO;
using AwesomeCodeFixerLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeCodeFixerApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FixController : ControllerBase
    {
        private readonly ILogger<FixController> _logger;
        public FixController(ILogger<FixController> logger)
        {
            _logger = logger;
        }

        [Consumes(MediaTypeNames.Text.Plain)]
        [HttpPost(Name = "Format")]
        public IActionResult Format([FromBody] string content)
        {
            _logger.LogInformation("Formatting code... {message}", content);

            string formattedContent = ExtensionManager.FormatCode(content);

            return Ok(formattedContent);
        }

        [Consumes(MediaTypeNames.Text.Plain)]
        [HttpPost(Name = "Lint")]
        public IActionResult Lint([FromBody] string content)
        {
            var issues = ExtensionManager.LintCode(content);

            return Ok(issues);
        }
    }
}
