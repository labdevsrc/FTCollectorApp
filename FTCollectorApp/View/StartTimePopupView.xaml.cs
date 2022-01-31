using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using FTCollectorApp.Service;
using FTCollectorApp.Model;
using System.Net.Http;
using Xamarin.Essentials;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartTimePopupView
    {

        public string HourMins;
        int _countCrew;
        public StartTimePopupView(int countCrew)
        {
            InitializeComponent();
            _countCrew = countCrew;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            //HourMins = DateTime.Now.ToString("HH:mm");
            entryStartTime.Text = DateTime.Now.ToString("H:m");
            btnClose.Clicked += (s,e) => PopupNavigation.Instance.PopAsync(true);
            
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            Session.event_type = Session.LunchOut;
            await OnJobSaveEvent();


        }

        async Task OnJobSaveEvent()
        {

            Session.event_type = Session.ClockIn;
            var perDiemChoice = pickPerDiem.Items[pickPerDiem.SelectedIndex];

            try
            {
                DateTime dt = DateTime.Parse(entryStartTime.Text.Trim());
                int user_hours = int.Parse(dt.ToString("HH"));
                int user_minutes = int.Parse(dt.ToString("mm"));

                await CloudDBService.PostJobEvent(user_hours.ToString(), user_minutes.ToString());
                if (Session.crewCnt == _countCrew)
                {
                    await PopupNavigation.Instance.PopAsync(true);
                    await Navigation.PushAsync(new SiteInputPage());
                }
                else
                    await PopupNavigation.Instance.PopAsync(true);

            }
            catch {
                
                DisplayAlert("Warning", "Time format must be HH:MM", "OK");
            }

        }

        private void entryStartTime_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}