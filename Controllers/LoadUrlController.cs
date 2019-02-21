using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using carouselExample.Models;
using Microsoft.AspNetCore.Cors;
using static carouselExample.Helpers.LoadUrlHelper;

namespace carouselExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class LoadUrlController : ControllerBase
    {
        [HttpGet]
        [EnableCors("AllowAllOrigins")]
        public ResponseModel LoadUrl(string Url)

        {
            if (string.IsNullOrEmpty(Url)) return null;

            if (!Url.StartsWith("http://") && !Url.StartsWith("https://"))
                Url = "http://" + Url;

            HtmlResponse resp = HtmlContent(Url);
            List<string> imageUrls = GetListOfImageUrls(resp.url, resp.content);
            string[] words = GetWords(resp.content);
            Dictionary<string, int> wordCountMap = BuildWordCount(words);
            int totalWordCount = words != null? words.Length: 0;

            return new ResponseModel(imageUrls, wordCountMap, totalWordCount);
        }

    }
}
