using System.Collections.Generic;

namespace carouselExample.Models
{
    public class ResponseModel
    {
        public List<string> ImageUrls { get; set; }
        public Dictionary<string, int> WordCount { get; set; }
        public int TotalWordCount { get; set; }

        public ResponseModel(List<string> imageUrls, Dictionary<string, int> WordCount, int TotalWordCount)
        {
            this.ImageUrls = imageUrls;
            this.WordCount = WordCount;
            this.TotalWordCount = TotalWordCount;
        }
    }
}
