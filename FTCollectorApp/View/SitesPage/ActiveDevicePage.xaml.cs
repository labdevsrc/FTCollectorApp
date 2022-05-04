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
using CommunityToolkit.Mvvm.ComponentModel;

namespace FTCollectorApp.View.SitesPage
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActiveDevicePage : ContentPage
    {


        /*string _selectedPosition;
        public string SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                _selectedPosition = value;
                OnPropertyChanged(nameof(SelectedPosition));
                Console.WriteLine();
            }
        }

        string _selectednumber;
        public string SelectedNumber
        {
            get => _selectednumber;
            set
            {
                _selectednumber = value;
                OnPropertyChanged(nameof(SelectedNumber));
                Console.WriteLine();
            }
        }


        //// ////// //////
        
        string _selectedSlotBladeTray;
        public string SelectedSlotBladeTray
        {
            get => _selectedSlotBladeTray;
            set
            {
                _selectedSlotBladeTray = value;
                OnPropertyChanged(nameof(SelectedSlotBladeTray));
                Console.WriteLine();
            }
        }
        ChassisType _selectedCT;
        public ChassisType SelectedCT
        {
            get => _selectedCT;
            set
            {
                _selectedCT = value;
                OnPropertyChanged(nameof(SelectedCT));
                Console.WriteLine();
            }
        }

        RackNumber _selectedRackNumber;
        public RackNumber SelectedRackNumber
        {
            get => _selectedRackNumber;
            set
            {
                _selectedRackNumber = value;
                OnPropertyChanged(nameof(SelectedRackNumber));
                Console.WriteLine();
            }
        }

        Manufacturer _manufacturerSelected;
        public Manufacturer SelectedManufacturer
        {
            get => _manufacturerSelected;
            set
            {
                _manufacturerSelected = value;
                OnPropertyChanged(nameof(SelectedManufacturer));
                Console.WriteLine();
                pModel.ItemsSource = ModelDetailList;// populate model
                Console.WriteLine();
            }
        }

        ModelDetail _modeldetailSelected;
        public ModelDetail SelectedModelDetail
        {
            get => _modeldetailSelected;
            set
            {
                if (SelectedManufacturer == null)
                {
                    DisplayAlert("Warning", "Select Manufacturer First", "OK");
                    return;
                }
                _modeldetailSelected = value;
                OnPropertyChanged(nameof(SelectedModelDetail));
                Console.WriteLine();
            }
        }



        public ObservableCollection<ChassisType> ChassisTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ChassisType>();
                    var table = conn.Table<ChassisType>().ToList();
                    return new ObservableCollection<ChassisType>(table);
                }
            }
        }


        public ObservableCollection<RackNumber> RackRailShelfs
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<RackNumber>();
                    var table = conn.Table<RackNumber>().Where(a => a.SiteId == Session.tag_number).ToList();
                    Console.WriteLine();
                    return new ObservableCollection<RackNumber>(table);
                }
            }
        }


        public ObservableCollection<ModelDetail> ModelDetailList
        {
            get
            {

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ModelDetail>();
                    var table = conn.Table<ModelDetail>().Where(a => a.ManufKey == SelectedManufacturer.ManufKey).ToList();
                    Console.WriteLine();
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

        public ObservableCollection<Manufacturer> ManufacturerList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Manufacturer>();
                    var table = conn.Table<Manufacturer>().ToList();
                    foreach (var col in table)
                    {
                        col.ManufName = HttpUtility.HtmlDecode(col.ManufName); // should use for escape char "
                    }
                    Console.WriteLine();
                    return new ObservableCollection<Manufacturer>(table);
                }
            }
        }*/

        public ActiveDevicePage()
        {
           //Session.tag_number = "54501"; // for test

            InitializeComponent();
            //BindingContext = new DropDownViewModel();
            BindingContext = new ActiveDeviceViewModel();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            txtHostTagNumber.Text = Session.tag_number;
            //pNumber.ItemsSource = OneToHundred;
            //pRackNumber.ItemsSource = OneToHundred;
            //pPosition.ItemsSource = OneToHundred;
            //pSlotBladeTray.ItemsSource = OneToHundred;

            
        }

        string InstalledAt, Manufactured;
        string SerialNum, selectedWebUrl;

        bool IsIPvisible = false;
        private void btnAddIP(object sender, EventArgs e)
        {

            ipAddressEntries.IsVisible = IsIPvisible;
            IsIPvisible = !IsIPvisible;
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

        bool displayed = false;
        private void SeePic(object sender, EventArgs e)
        {
            //webview.IsVisible = displayed;
            //webview.Source = SelectedModelDetail.PictUrl;
            displayed = !displayed;

        }

        private void btnUpdateChassis(object sender, EventArgs e)
        {

        }

        private void btnSaveContinue(object sender, EventArgs e)
        {

        }

        private void btnCamera(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CameraViewPage());
        }

        /*
        
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
        
        List<KeyValuePair<string, string>> keyvaluepair()
        {
            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), 
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), 
                new KeyValuePair<string, string>("host_tag_number", Session.tag_number), 
                new KeyValuePair<string, string>("stage", Session.stage),  
                new KeyValuePair<string, string>("tag", Session.tag_number), 


                new KeyValuePair<string, string>("manufacturer", SelectedManufacturer.ManufKey),  
                new KeyValuePair<string, string>("model", SelectedModelDetail.ModelKey),  
                new KeyValuePair<string, string>("rack_number", SelectedRackNumber.Racknumber), 


                new KeyValuePair<string, string>("comment", sComment.Text),  
                new KeyValuePair<string, string>("manufactured_date", Manufactured),
                new KeyValuePair<string, string>("installed2", InstalledAt),


                new KeyValuePair<string, string>("ipaddr", IsIPAddressValid(IP1.Text + "." + IP2.Text + "." + IP3.Text + "." + IP4.Text)), 
                new KeyValuePair<string, string>("subnet", ""),
                new KeyValuePair<string, string>("protocol", txtProtocol.Text),
                new KeyValuePair<string, string>("getway", IsIPAddressValid(GWIP1.Text + "." + GWIP2.Text + "." + GWIP3.Text + "." + GWIP4.Text)),

                new KeyValuePair<string, string>("slotblade", SelectedSlotBladeTray),
                new KeyValuePair<string, string>("position", SelectedPosition),
                new KeyValuePair<string, string>("rack_number", SelectedRackNumber.Racknumber),
                

            };


            return keyValues;

        }

        private async void btnSaveContinue(object sender, EventArgs e)
        {
            var KVPair = keyvaluepair();
            await CloudDBService.PostActiveDevice(KVPair);
            await Navigation.PopAsync();
        }*/
    }
}