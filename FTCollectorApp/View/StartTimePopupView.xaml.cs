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
using FTCollectorApp.View.SitesPage;
using System.Windows.Input;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartTimePopupView
    {

        public string HourMins;

        string crewname;
        public StartTimePopupView(string cname)
        {
            InitializeComponent();
            crewname = cname;
            //_countCrew = countCrew;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            //HourMins = DateTime.Now.ToString("HH:mm");
            entryStartTime.Text = DateTime.Now.ToString("H:m");
            btnClose.Clicked += (s,e) => PopupNavigation.Instance.PopAsync(true);
            pickPerDiem.SelectedIndex = 0;
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {


            Session.event_type = Session.LunchOut;
            Session.sessioncrew.Remove(crewname);
            Session.crewCnt = Session.sessioncrew.Count;
            if (pickPerDiem.SelectedIndex == -1)
            {                
                return;
            }
            await OnJobSaveEvent();


        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Console.WriteLine("OnDisappearing()");
        }
        async Task OnJobSaveEvent()
        {
            var perDiemIdx = pickPerDiem.SelectedIndex.ToString();
            Session.event_type = Session.ClockIn;

            try
            {
                DateTime dt = DateTime.Parse(entryStartTime.Text.Trim());
                int user_hours = int.Parse(dt.ToString("HH"));
                int user_minutes = int.Parse(dt.ToString("mm"));
                


                await CloudDBService.PostJobEvent(user_hours.ToString(), user_minutes.ToString(), perDiemIdx);
                if (Session.crewCnt == 0)
                {
                    // navigate directly to BeginWorkPate

                    await Task.Delay(1000);
                    await Navigation.PushAsync(new BeginWorkPage());


                    MessagingCenter.Send<StartTimePopupView>(this, "OnAppearing");
                }
                
                FinishTimeSetCommand?.Execute(CommandParam);
                
                await PopupNavigation.Instance.PopAsync(true);

            }
            catch { 
                
                await DisplayAlert("Warning", "Time format must be HH:MM", "OK");
            }

        }

        
        string commandParam;
        public string CommandParam
        {
            get => commandParam;
            set
            {
                commandParam = value;
                OnPropertyChanged(nameof(CommandParam));
            }
        }
        public ICommand FinishTimeSetCommand { get; set; }

        private void entryStartTime_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}