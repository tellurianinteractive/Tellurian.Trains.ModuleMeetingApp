using Microsoft.AspNetCore.Mvc;
using Markdig;
using System.Globalization;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Tellurian.Trains.MeetingApp.Contracts.Extensions;

namespace Tellurian.Trains.MeetingApp.Server.Controllers
{
    /// <summary>
    /// Endpoint for getting language specific texts.
    /// </summary>
    [Route("api/content")]
    public class ContentController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{language}/{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        public async Task<IActionResult> GetContentAsync(string language, string id)
        {
            var culture = CultureInfo.GetCultures(CultureTypes.NeutralCultures).SingleOrDefault(c => c.TwoLetterISOLanguageName.Equals(language, StringComparison.OrdinalIgnoreCase));
            if (culture is null) return NotFound($"Language {language} is not valid or unsupported.");

            var md = await culture.GetMarkdownAsync("Markdown", id);
            if (md.Length > 0) return Ok(Markdown.ToHtml(md, Pipeline()));
            return NotFound($"Page {id} does not exist.");

            static MarkdownPipeline Pipeline() {
                var builder = new MarkdownPipelineBuilder();
                builder.UseAdvancedExtensions();
                return builder.Build();
            }

        }
    }
}
