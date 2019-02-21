using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using carouselExample.Models;
using Microsoft.AspNetCore.Cors;
using static carouselExample.Helpers.LoadUrlHelper;

namespace carouselExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadUrlController : ControllerBase
    {
        [HttpGet]
        [EnableCors("AllowAllOrigins")]
        public ResponseModel LoadUrl(string Url)

        {
            if (string.IsNullOrEmpty(Url)) return null;
            HtmlResponse resp = HtmlContent(Url);
            List<string> imageUrls = GetListOfImageUrls(resp.Url, resp.Content);
            string[] words = GetWords(resp.Content);
            Dictionary<string, int> wordCountMap = BuildWordCount(words);
            int totalWordCount = words != null? words.Length: 0;

            return new ResponseModel(imageUrls, wordCountMap, totalWordCount);
        }

    }
}
