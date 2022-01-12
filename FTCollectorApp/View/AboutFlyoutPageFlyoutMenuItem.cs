using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTCollectorApp.Page
{
    public class AboutFlyoutPageFlyoutMenuItem
    {
        public AboutFlyoutPageFlyoutMenuItem()
        {
            TargetType = typeof(AboutFlyoutPageFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}