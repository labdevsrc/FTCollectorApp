using System;
using System.Collections.Generic;
using System.Text;

namespace FTCollectorApp.Model
{
    public class CodeSiteType
    {
        public int id { get; set; }
        public string CodeKey { get; set; }

        public string SiteType { get; set; }
        public string MajorType { get; set; }
        public string MinorType { get; set; }
        public string ITSFM { get; set; }
    }
}
