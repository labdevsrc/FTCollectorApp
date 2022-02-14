using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTCollectorApp.Model;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OffsetGPSPopUp
    {
        public OffsetGPSPopUp()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            btnClose.Clicked += (s, e) => PopupNavigation.Instance.PopAsync(true);
           

        }

        private void btnSave_Clicked(object sender, EventArgs e)
        {
            Session.OffsetBearing = entryBearing.Text;
            Session.OffsetDistance = entryDistance.Text;
            PopupNavigation.Instance.PopAsync(true);
        }
    }
}