using System;
using System.Collections.Generic;
using System.Text;

namespace FTCollectorApp.Model
{
    public class Site
    {
        public int id { get; set; }

        public string MajorSites { get; set; }
        public string MinorSites { get; set; }
        public string  tag_number { get; set; }
        public string stage { get; set; }
    }
}
