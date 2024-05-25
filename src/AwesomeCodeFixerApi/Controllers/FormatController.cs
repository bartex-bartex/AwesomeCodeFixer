using AwesomeCodeFixerApi.Models;
using AwesomeCodeFixerLibrary;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeCodeFixerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormatController : ControllerBase
    {
        [HttpPost(Name = "Format")]
        [Route("Format")]
        public IActionResult Format([FromBody] ContentModel content)
        {
            string formattedContent = ExtensionManager.FormatCode(content.Text);

            return Ok(formattedContent);
        }
    }
}
