using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI.Hateoas
{
    public abstract class HateoasLinkBase
    {
        public List<Link> Links { get; set; } = new List<Link>();
    }
}
