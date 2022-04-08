using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
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
using FTCollectorApp.View.Utils;
using System.Web;
using FTCollectorApp.Service;
using System.Net;

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActiveDevicePage : ContentPage
    {
        List<string> OneToHundred = new List<string>();
        public ObservableCollection<ModelDetail> ModelDetailList
        {
            get
            {

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ModelDetail>();
                    var table = conn.Table<ModelDetail>().Where(a => a.ManufKey == ManufacturerKeySelected).ToList();
                    foreach (var col in table)
                    {
                        col.ModelNumber = HttpUtility.HtmlDecode(col.ModelNumber); // should use for escape char 
                        if (string.IsNullOrEmpty(col.ModelCode1.ToString())) // sometimes this model entri is null
                            col.ModelCode1 = col.ModelCode2;
                        if (string.IsNullOrEmpty(col.ModelCode2.ToString()))
                            col.ModelCode2 = col.ModelCode1;
                    }
                    return new ObservableCollection<ModelDetail>(table);
                }
            }
        }
        public ActiveDevicePage()
        {
            InitializeComponent();
            //BindingContext = new DropDownViewModel();

            for (int i = 1; i <100 ; i++)
            {
                OneToHundred.Add(i.ToString());
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            txtHostTagNumber.Text = Session.tag_number;
            pNumber.ItemsSource = OneToHundred;
            pRackNumber.ItemsSource = OneToHundred;
            pPosition.ItemsSource = OneToHundred;
            pSlotBladeTray.ItemsSource = OneToHundred;


        }

        string ManufacturerKeySelected, ModelDetailSelected;
        string selectedCTType, rackNumber, selectedPos, selectedSlotBladeTray, selectedNum;
        string InstalledAt, Manufactured;
        string SerialNum, selectedWebUrl;

        private void btnAddIP(object sender, EventArgs e)
        {

        }

        private void btnGotoPortPage(object sender, EventArgs e)
        {

        }

        private void btnPortConn(object sender, EventArgs e)
        {

        }

        private void btnExit(object sender, EventArgs e)
        {

        }

        private void OnIndexChanged(object sender, EventArgs e)
        {
            selectedSlotBladeTray = pSlotBladeTray.SelectedIndex != -1 ? "0" : pSlotBladeTray.SelectedIndex.ToString();
            selectedPos = pPosition.SelectedIndex != -1 ? "0" : pPosition.SelectedIndex.ToString();
            rackNumber = pRackNumber.SelectedIndex != -1 ? "0" : pRackNumber.SelectedIndex.ToString();
            selectedNum = pNumber.SelectedIndex != -1 ? "0" : pNumber.SelectedIndex.ToString();


            if (pChassisType.SelectedIndex != -1)
            {
                var selected = pChassisType.SelectedItem as ChassisType;
                selectedCTType = selected.CTKey;
            }
            if (pManufacturer.SelectedIndex != -1)
            {
                var selected = pManufacturer.SelectedItem as Manufacturer;
                ManufacturerKeySelected = selected.ManufKey;
                pModel.ItemsSource = ModelDetailList;
            }

        }
        bool displayed = false;
        private void SeePic(object sender, EventArgs e)
        {
            webview.IsVisible = displayed;
            webview.Source = selectedWebUrl;
            displayed = !displayed;

        }

        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            InstalledAt = dateInstalled.Date.ToString("yyyy-MM-dd");
            Manufactured = dateManufactured.Date.ToString("yyyy-MM-dd");
        }


        private void OnModelChanged(object sender, EventArgs e)
        {
            if (pModel.SelectedIndex != -1)
            {
                var selected = pModel.SelectedItem as ModelDetail;
                modelDescription.Text = selected.ModelDescription;
                ModelDetailSelected = selected.ModelKey;
                selectedWebUrl = selected.PictUrl;
            }
        }

        /* Some community said IPAdress tryparse is not good practise
         * 
         * string IsIPAddressValid(string ipaddress)
        {
            //string ipaddress = IP1.Text + "." + IP2.Text + "." + IP3.Text + "." + IP4.Text;
            IPAddress ip;
            bool ValidateIP = IPAddress.TryParse(ipaddress, out ip);
            if (ValidateIP)
                return ipaddress;
            else
                return "0.0.0.0";
        }*/

        string IsIPAddressValid(string ipaddress)
        {
            //  Split string by ".", check that array length is 4
            string[] arrOctets = ipaddress.Split('.');
            if (arrOctets.Length != 4)
                return "0.0.0.0" ;

            //Check each substring checking that parses to byte
            byte obyte = 0;
            foreach (string strOctet in arrOctets)
                if (!byte.TryParse(strOctet, out obyte))
                    return "0.0.0.0";

            return ipaddress;
        }


        private void btnCamera(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CameraViewPage());
        }

        List<KeyValuePair<string, string>> keyvaluepair()
        {
            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), 
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), 
                new KeyValuePair<string, string>("host_tag_number", Session.tag_number), 
                new KeyValuePair<string, string>("stage", Session.stage),  
                new KeyValuePair<string, string>("tag", Session.tag_number), 


                new KeyValuePair<string, string>("manufacturer", ManufacturerKeySelected),  
                new KeyValuePair<string, string>("model", ModelDetailSelected),  
                new KeyValuePair<string, string>("rack_number", rackNumber), 


                new KeyValuePair<string, string>("comment", sComment.Text),  
                new KeyValuePair<string, string>("manufactured_date", Manufactured),
                new KeyValuePair<string, string>("installed2", InstalledAt),


                new KeyValuePair<string, string>("ipaddr", IsIPAddressValid(IP1.Text + "." + IP2.Text + "." + IP3.Text + "." + IP4.Text)), 
                new KeyValuePair<string, string>("subnet", ""),
                new KeyValuePair<string, string>("protocol", txtProtocol.Text),
                new KeyValuePair<string, string>("getway", IsIPAddressValid(GWIP1.Text + "." + GWIP2.Text + "." + GWIP3.Text + "." + GWIP4.Text)),

                new KeyValuePair<string, string>("slotb", selectedSlotBladeTray)
            };


            return keyValues;

        }

        private async void btnSaveContinue(object sender, EventArgs e)
        {
            var KVPair = keyvaluepair();
            await CloudDBService.PostActiveDevice(KVPair);
            await Navigation.PopAsync();
        }
    }
}