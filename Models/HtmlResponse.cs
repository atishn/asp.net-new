using System;
namespace carouselExample.Models
{
    public class HtmlResponse
    {
        public string Content { get; set; }
        public string Url { get; set; }

        public HtmlResponse(string content, string url)
        {
            this.Content = content;
            this.Url = url;

        }
    }
}