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

        [HttpPost(Name = "Format")]
        public IActionResult Format([FromBody] ContentDTO content)
        {
            _logger.LogInformation("Formatting code... {message}", content.Text);

            string formattedContent = ExtensionManager.FormatCode(content.Text);

            return Ok(formattedContent);
        }

        [HttpPost(Name = "Lint")]
        public IActionResult Lint([FromBody] ContentDTO content)
        {
            var issues = ExtensionManager.LintCode(content.Text);

            return Ok(issues);
        }
    }
}
