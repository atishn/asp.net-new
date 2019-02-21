using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using NUglify;
using System;
using System.Linq;
using carouselExample.Models;
using System.IO;

namespace carouselExample.Helpers
{
    public class LoadUrlHelper
    {
        public static HtmlResponse HtmlContent(string Url)
        {
            try
            {
                if (!Url.StartsWith("http://") && !Url.StartsWith("https://"))
                    Url = "http://" + Url;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.AllowAutoRedirect = true;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();

                string lastRedirect = response.ResponseUri.ToString();

                StreamReader reader = new StreamReader(dataStream);
                string strResponse = reader.ReadToEnd();
                response.Close();

                return new HtmlResponse(strResponse, lastRedirect);

            }
            catch (WebException ex)
            {
                //Todo - Handle it exception strategy. Do not EAT the exception. Use global Application_Error.
                ex.Data.ToString();
                return new HtmlResponse(null, null);
            }

        }

        public static string[] GetWords(string html)
        {
            if (string.IsNullOrEmpty(html)) return null;

            string sInput = Uglify.HtmlToText(html).Code.ToLower();

            if (string.IsNullOrEmpty(sInput)) return null;

            sInput = sInput.Replace(",", "").Replace(".", "");
            string[] arr = sInput.Split(' ');
            return arr;
        }

        public static Dictionary<string, int> BuildWordCount(string[] words)
        {
            if (words == null || words.Length == 0) return null;
            var wordEnumerator = words.Where(s => s.Length >= 4 && !Regex.IsMatch(s, @"^\d+$"))
            .GroupBy(s => s)
            .OrderByDescending(g => g.Count());

            return wordEnumerator.ToDictionary(g => g.Key, g => g.Count());
        }

        public static List<string> GetListOfImageUrls(string baseUrl, string html)
        {
            List<string> imageList = new List<string>();
            string pattern = @"<img[^>]*?src\s*=\s*[""""']?([^'"""" >]+?)[ '""""][^>]*?>";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(html);

            string imageSrcCode = @"src=""";

            for (int i = 0; i < matches.Count; i++)
            {
                string imgTag = matches[i].Value;
                int index = imgTag.IndexOf(imageSrcCode, StringComparison.OrdinalIgnoreCase);
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
    }
}

