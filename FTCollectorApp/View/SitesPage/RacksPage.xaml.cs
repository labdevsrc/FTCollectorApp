using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Service;
using FTCollectorApp.View.Utils;
using FTCollectorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using System.Web;
using System.Windows.Input;
using System.ComponentModel;

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RacksPage : ContentPage
    {

        Command ResultCommand { get; set; }
        public RacksPage()
        {
            InitializeComponent();
            BindingContext = new RacksViewModel();
        }


        private void btnCamera(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CameraViewPage());
        }


        private async void ExitPage(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }


        /*List<KeyValuePair<string, string>> keyvaluepair()
        {

            var keyValues = new List<KeyValuePair<string, string>>{

                // Session params
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),  
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), 
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD),
                new KeyValuePair<string, string>("jobnum",Session.jobnum), 
                new KeyValuePair<string, string>("owner_key", Session.ownerkey),
                new KeyValuePair<string, string>("jobkey", Session.jobkey),
                new KeyValuePair<string, string>("tag",Session.tag_number),
                new KeyValuePair<string, string>("sitekey",Session.site_key), 
         //racks.php
        //var values={"type":type,"rnumber":rnumber,"orientation":ori,"xpos":xpos,
        //"ypos":ypos,"manufacturer":mfr,"model":mod,"height":height,"width":width,"depth":depth,
        //"time":getCurtime()};

                new KeyValuePair<string, string>("type", SelectedRackType.RackTypeKey),  
                new KeyValuePair<string, string>("racknumber", SelectedRackNumber),
                new KeyValuePair<string, string>("orientation", SelectedOrientation.Equals("Horizontal") ? "H" : "V"),

                new KeyValuePair<string, string>("xpos", Ypos.Text),  
                new KeyValuePair<string, string>("ypos", Xpos.Text),
                new KeyValuePair<string, string>("manufacturer", SelectedManufacturer.ManufKey),
                //new KeyValuePair<string, string>("manufacturer", ManufacturerKeySelected), 
                new KeyValuePair<string, string>("model", SelectedModelDetail.ModelKey),

                new KeyValuePair<string, string>("height", SelectedModelDetail.height),
                new KeyValuePair<string, string>("width", SelectedModelDetail.width),
                new KeyValuePair<string, string>("depth", SelectedModelDetail.depth)
            };


            return keyValues;

        }



        private async void OnClicked(object sender, EventArgs e)
        {
            var KVPair = keyvaluepair();
            ResultCommand?.Execute("OK");
            var result = await CloudDBService.PostSaveRacks(KVPair);
            if (result.Equals("OK"))
            {
                await DisplayAlert("Success", "Uploading Data Done", "OK");
            }
            else
            {
                await DisplayAlert("Warning", result, "OK");
            }
        }*/

    }
}