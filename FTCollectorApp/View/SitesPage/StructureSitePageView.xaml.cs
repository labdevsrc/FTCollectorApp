using FTCollectorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StructureSitePageView : ContentPage
    {
        public StructureSitePageView(string minorType, string tagNumber)
        {
            InitializeComponent();
            var MajorMinorType = $"Structure - {minorType}";
            BindingContext = new StructureSitePageViewModel(MajorMinorType, tagNumber);
        }
    }
}