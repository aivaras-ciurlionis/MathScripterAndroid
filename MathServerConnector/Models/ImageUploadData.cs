using System.Collections.Generic;

namespace MathServerConnector.Models
{
    public class ImageUploadData
    {
        public string SegmentName { get; set; }
        public IEnumerable<int> Pixels { get; set; }
    }
}