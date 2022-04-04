using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Service;
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
    public partial class RacksPage : ContentPage
    {
        List<string> OneToAHundred = new List<string>();
        List<string> FrontBack = new List<string>();
        List<string> Orientation = new List<string>();
        public RacksPage()
        {
            InitializeComponent();


            // Populate Drop down number
            for(int i = 0; i < 100; i++)
            {
                OneToAHundred.Add(i.ToString());
            }
            FrontBack.Add("Front");
            FrontBack.Add("Back");

            Orientation.Add("Horizontal");
            Orientation.Add("Vertical");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            pRackNum.ItemsSource = OneToAHundred;
            pFrontBack.ItemsSource = FrontBack;
            pOrientation.ItemsSource = Orientation;

        }


        string sRackNumber, sOrientation;
        string selectedType, selectedManuf, selectedModel;
        string ModelDetailSelected, height, depth, width;

        private async void ExitPage(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void OnIndexChanged(object sender, EventArgs e)
        {
            sRackNumber = pRackNum.SelectedIndex != -1 ? "0" : pRackNum.SelectedIndex.ToString();
            sOrientation = pOrientation.SelectedIndex != -1 ? "0" : pOrientation.SelectedIndex.ToString();

            if (pType.SelectedIndex != -1)
            {
                var selected = pType.SelectedItem as RackType;
                selectedType = selected.RackTypeKey;
            }
            if (pManufacturer.SelectedIndex != -1)
            {
                var selected = pManufacturer.SelectedItem as Manufacturer;
                selectedManuf = selected.ManufKey;
            }

            if (pModel.SelectedIndex != -1)
            {
                var selected = pModel.SelectedItem as ModelDetail;
                ModelDetailSelected = selected.ModelKey;
                entryWidth.Text = selected.width;
                entryDepth.Text = selected.depth;
                entryHeight.Text = selected.height;
            }
        }

        List<KeyValuePair<string, string>> keyvaluepair()
        {

            var keyValues = new List<KeyValuePair<string, string>>{

                // Session params
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),  // 2
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // 2
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), // 6
                new KeyValuePair<string, string>("jobnum",Session.jobnum), //  7 
                new KeyValuePair<string, string>("owner_key", Session.ownerkey),
                new KeyValuePair<string, string>("jobkey", Session.jobkey),
                new KeyValuePair<string, string>("tag",Session.tag_number), //8

         //
        //var values={"type":type,"rnumber":rnumber,"orientation":ori,"xpos":xpos,
        //"ypos":ypos,"manufacturer":mfr,"model":mod,"height":height,"width":width,"depth":depth,
        //"time":getCurtime()};

                new KeyValuePair<string, string>("type", selectedType),  /// site_id
                new KeyValuePair<string, string>("racknumber", sRackNumber),  /// code_site_type.key
                new KeyValuePair<string, string>("orientation", sOrientation),

                new KeyValuePair<string, string>("xpos", Ypos.Text),  
                new KeyValuePair<string, string>("ypos", Xpos.Text),
                new KeyValuePair<string, string>("manufacturer", selectedManuf), 
                new KeyValuePair<string, string>("model", selectedModel),

                new KeyValuePair<string, string>("height", entryHeight.Text),
                new KeyValuePair<string, string>("width", entryWidth.Text),
                new KeyValuePair<string, string>("depth", entryDepth.Text)
            };


            return keyValues;

        }



        private async void OnClicked(object sender, EventArgs e)
        {
            var KVPair = keyvaluepair();

            var result = await CloudDBService.PostSaveRacks(KVPair);
            if (result.Equals("OK"))
            {
                await DisplayAlert("Success", "Uploading Data Done", "OK");
            }
            else
            {
                await DisplayAlert("Warning", result, "OK");
            }
        }


    }
}