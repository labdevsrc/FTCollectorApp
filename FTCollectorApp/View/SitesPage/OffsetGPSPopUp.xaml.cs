using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTCollectorApp.Model;
using FTCollectorApp.Utils;
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

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            Position pos1 = new Position();
            pos1.Latitude = double.Parse(Session.lattitude2);
            pos1.Longitude = double.Parse(Session.longitude2);

            Haversine hv = new Haversine();
            Position newPos = new Position();
            newPos = hv.NewCoordsCalc(pos1, double.Parse(entryBearing.Text), int.Parse(entryDistance.Text),
                DistanceType.Kilometers);
            Session.lattitude_offset = newPos.Latitude.ToString();
            Session.longitude_offset = newPos.Longitude.ToString();

            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}