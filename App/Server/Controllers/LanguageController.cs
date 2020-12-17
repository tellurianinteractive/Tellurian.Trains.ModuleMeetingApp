using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Markdig;
using System.Globalization;
using Tellurian.Trains.MeetingApp.Contract;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Tellurian.Trains.MeetingApp.Server.Controllers
{
    /// <summary>
    /// Endpoint for getting language specific texts.
    /// </summary>
    [Route("api/languages")]
    public class LanguageController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [Route("{language}/{page}")]
        public async Task<IActionResult> GetContent(string language, string page)
        {
            var culture = CultureInfo.GetCultures(CultureTypes.NeutralCultures).SingleOrDefault(c => c.TwoLetterISOLanguageName.Equals(language, StringComparison.OrdinalIgnoreCase));
            if (culture is null) return NotFound($"Language {language} is not valid or unsupported.");

            var md = await culture.GetMarkdownAsync("Markdown", page);
            if (md.Length > 0) return Ok(Markdown.ToHtml(md, Pipeline()));
            return NotFound($"Page {page} does not exist.");

            static MarkdownPipeline Pipeline() {
                var builder = new MarkdownPipelineBuilder();
                builder.UseAdvancedExtensions();
                return builder.Build();
            }

        }
    }
}
