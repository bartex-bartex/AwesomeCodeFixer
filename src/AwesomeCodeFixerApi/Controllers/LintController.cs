using AwesomeCodeFixerApi.Models;
using AwesomeCodeFixerLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeCodeFixerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LintController : ControllerBase
    {
        [HttpPost(Name = "Lint")]
        [Route("Lint")]
        public IActionResult Lint([FromBody] ContentModel content)
        {
            var issues = ExtensionManager.LintCode(content.Text);

            return Ok(issues);
        }
    }
}
