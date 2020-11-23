using System;

namespace cll.Models
{
    public class ToDo
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        
        public string user { get; set; }
        public DateTime? due { get; set; }
        public bool isComplete { get; set; }
    }
}
