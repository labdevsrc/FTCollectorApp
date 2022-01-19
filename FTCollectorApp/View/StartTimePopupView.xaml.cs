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
        private HttpClient httpClient;
        public StartTimePopupView()
        {
            InitializeComponent();

            try
            {
                httpClient = new HttpClient()
                {
                    BaseAddress = new Uri(Constants.BaseUrl)
                };
            }
            catch
            {

            }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            btnClose.Clicked += (s,e) => PopupNavigation.Instance.PopAsync(true);
            
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            Session.event_type = Session.LunchOut;
            await OnJobSaveEvent();
            await PopupNavigation.Instance.PopAsync(true);
        }

        async Task OnJobSaveEvent()
        {

            if (Session.gps_sts == "1")
            {
                Session.manual_latti = string.Empty;
                Session.manual_longi = string.Empty;
            }
            DateTime dt = DateTime.Parse(entryStartTime.Text);
            var user_hours = dt.ToString("h");
            var user_minutes = dt.ToString("m");

            //await CloudDBService.PostJobEvent(); still working on it

            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("jobnum",Session.jobnum),
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),

                new KeyValuePair<string, string>("min", user_hours),
                new KeyValuePair<string, string>("hr", user_minutes),


                new KeyValuePair<string, string>("gps_sts", Session.gps_sts),
                
                // xSaveJobEvents.php Line 59 : $longitude=$_POST['longitude2'];
                // xSaveJobEvents.php Line 60 : $latitude =$_POST['lattitude2'];
                new KeyValuePair<string, string>("manual_latti", Session.manual_latti),
                new KeyValuePair<string, string>("manual_longi", Session.manual_longi),

                // xSaveJobEvents.php Line 73 : $longitude=$_POST['longitude2'];
                // xSaveJobEvents.php Line 74 : $latitude =$_POST['lattitude2'];
                new KeyValuePair<string, string>("lattitude2", Session.lattitude2),
                new KeyValuePair<string, string>("longitude2", Session.longitude2),

                new KeyValuePair<string, string>("evtype", Session.LunchOut), 

                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),

                new KeyValuePair<string, string>("ajaxname", Constants.InsertJobEvents)
            };
            // this Httpconten will work for Content-type : x-wwww-url-formencoded REST
            HttpContent content = new FormUrlEncodedContent(keyValues);

            HttpResponseMessage response = null;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                response = await httpClient.PostAsync(Constants.InsertJobEvents, content);
                if (response.IsSuccessStatusCode)
                {
                    var isi = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("OK 200 " + isi);

                }
            }
            else
            {
                // Put to Pending Sync

            }
        }
    }
}