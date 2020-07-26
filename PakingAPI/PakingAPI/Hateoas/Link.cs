using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI.Hateoas
{
    public class Link
    {
        public string Href { get; set; }
        public string Rel { get; set;}
        public string Type { get; set; }
        public Link(string href,  string rel, string type)
        {
            this.Href = href;
            this.Rel = rel;
            this.Type = type;
        }
    }
}
