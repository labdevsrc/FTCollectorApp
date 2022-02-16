using FTCollectorApp.Service;
using FTCollectorApp.View.SitesPage;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OdometerPopup
    {
        public OdometerPopup()
        {
            InitializeComponent();
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            IsBusy = true;
            try
            {
                await CloudDBService.PostJobEvent(entryOdometer.Text);
                bool answer = await DisplayAlert("Confirm", "Confirm and go to Site menu page?", "OK", "Cancel");
                if (answer)
                {
                    await Navigation.PushAsync(new AsBuiltDocMenu());
                }
                else
                    await PopupNavigation.Instance.PopAsync(true);
            }
            catch
            {
                await DisplayAlert("Error", "Update JobEvent table failed", "OK");
            }
            IsBusy = false;
        }
    }
}