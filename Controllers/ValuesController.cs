using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using NUglify;
using System;
using System.Linq;
using carouselExample.Models;
using System.Collections.ObjectModel;

namespace carouselExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values           
        [HttpGet]
        public ResponseModel Get(string Url)

        {
            if (!Url.StartsWith("http://") && !Url.StartsWith("https://"))
                Url = "http://" + Url;

            string html = HtmlContent(Url);
            List<string> imageUrls = GetListOfImageUrls(Url, html);
            string[] words = GetWords(html);
            Dictionary<string, int> wordCountMap = BuildWordCount(words);
            int totalWordCount = words.Length;

            return new ResponseModel(imageUrls, wordCountMap, totalWordCount);
        }



        public static string HtmlContent(string Url)
        {
            string html = string.Empty;
            using (WebClient client = new WebClient())
            {
                html = client.DownloadString(Url);
            }
            return html;
        }

        private static string[] GetWords(string html)
        {
            string sInput = Uglify.HtmlToText(html).Code;
            Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

            sInput = sInput.Replace(",", "").Replace(".", "");
            string[] arr = sInput.Split(' ');

            return arr;
        }

        private static Dictionary<string, int> BuildWordCount(string[] words)
        {

            var wordEnumerator = words.Where(s => s.Length >= 3)
            .GroupBy(s => s)
            .OrderByDescending(g => g.Count());

            return wordEnumerator.ToDictionary(g => g.Key, g => g.Count());
        }

        private static List<string> GetListOfImageUrls(string baseUrl, string html)
        {
            List<string> imageList = new List<string>();
            string pattern = @"<img[^>]*?src\s*=\s*[""""']?([^'"""" >]+?)[ '""""][^>]*?>";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(html);

            string imageSrcCode = @"src=""";

            for (int i = 0; i < matches.Count; i++)
            {
                string imgTag = matches[i].Value;
                int index = imgTag.IndexOf(imageSrcCode);
                if (index != -1)
                {
                    int brackedEnd = imgTag.IndexOf('>'); //make sure data will be inside img tag
                    int start = index + imageSrcCode.Length;
                    int end = imgTag.IndexOf('"', start + 1);

                    //Extract the line
                    if (end > start && start < brackedEnd)
                    {
                        string loc = imgTag.Substring(start, end - start);
                        if (!loc.StartsWith("http://") && !loc.StartsWith("https://") && !loc.StartsWith("www"))
                            loc = baseUrl + loc;

                        imageList.Add(loc);
                    }
                }

            }

            return imageList;

        }

        // POST api/values
        [HttpPost]
        public void GetUrl([FromBody] string urlValue)
        {

        }
    }
}
