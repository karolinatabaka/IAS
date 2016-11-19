using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HUB
{
    public class HubMovie
    {
        public int Id { get; set; }

        public string Name { get;set;}

        public string Description { get; set;}

        public DateTime PublishDate { get; set; }

        public Provider Provider { get; set; }
    }
}
