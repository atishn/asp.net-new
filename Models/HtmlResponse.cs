using System;
namespace carouselExample.Models
{
    public class HtmlResponse
    {
        public string content { get; set; }
        public string url { get; set; }

        public HtmlResponse(string content, string url)
        {
            this.content = content;
            this.url = url;

        }
    }
}