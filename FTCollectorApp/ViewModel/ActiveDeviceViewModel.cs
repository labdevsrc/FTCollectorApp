using CommunityToolkit.Mvvm.ComponentModel;
using FTCollectorApp.Model.Reference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SQLite;
using FTCollectorApp.Model;
using System.Web;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using FTCollectorApp.Service;

namespace FTCollectorApp.ViewModel
{
    public partial class ActiveDeviceViewModel : ObservableObject
    {
        [ObservableProperty]
        string selectedPosition;

        [ObservableProperty]
        string selectednumber;

        [ObservableProperty]
        string selectedSlotBladeTray;

        [ObservableProperty]
        ChassisType selectedCT;

        [ObservableProperty]
        RackNumber selectedRackNumber;


        Manufacturer selectedManufacturer;
        public Manufacturer SelectedManufacturer
        {
            get => selectedManufacturer;
            set
            {
                SetProperty(ref selectedManufacturer, value);
                _modelDetailList.Where(a => a.ManufKey == value.ManufKey);
                OnPropertyChanged(nameof(ModelDetailList));
                Console.WriteLine();
            }
        }


        [ObservableProperty]
        ModelDetail selectedModelDetail;

        [ObservableProperty]
        RackType selectedRackType;

        [ObservableProperty]
        bool isDisplayed;

        [ObservableProperty]
        bool isShow;

        [ObservableProperty]
        string comment;

        [ObservableProperty]
        string _IP1;

        [ObservableProperty]
        string _IP2;

        [ObservableProperty]
        string _IP3;

        [ObservableProperty]
        string _IP4;
        [ObservableProperty]
        string subnet1;

        [ObservableProperty]
        string subnet2;

        [ObservableProperty]
        string subnet3;

        [ObservableProperty]
        string subnet4;
        [ObservableProperty]
        string _GWIP1;

        [ObservableProperty]
        string _GWIP2;

        [ObservableProperty]
        string _GWIP3;

        [ObservableProperty]
        string _GWIP4;

        [ObservableProperty]
        string _MCast1;
        [ObservableProperty]
        string _MCast2;
        [ObservableProperty]
        string _MCast3;
        [ObservableProperty]
        string _MCast4;

        [ObservableProperty]
        string protocol;
        [ObservableProperty]
        string videoProtocol;
        [ObservableProperty]
        string _VLAN;

        [ObservableProperty]
        string selectedManufDate;
        [ObservableProperty]
        string selectedInstallDate;


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


        ObservableCollection<ModelDetail> _modelDetailList;
        public ObservableCollection<ModelDetail> ModelDetailList
        {
            get
            {

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ModelDetail>();
                    var table = conn.Table<ModelDetail>().ToList();
                    foreach (var col in table)
                    {
                        col.ModelNumber = HttpUtility.HtmlDecode(col.ModelNumber); // should use for escape char 
                        if (col.ModelCode1 == "") // sometimes this model entri is null
                            col.ModelCode1 = col.ModelCode2;
                        if (col.ModelCode2 == "")
                            col.ModelCode2 = col.ModelCode1;
                    }
                    _modelDetailList = new ObservableCollection<ModelDetail>(table);
                    return _modelDetailList;
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
        }
        public ICommand ToggleWebViewCommand { get; set; }
        public ICommand ToggleIPEntriesCommand { get; set; }
        public ICommand SaveContinueCommand { get; set; }

        public ActiveDeviceViewModel()
        {
            ToggleWebViewCommand = new Command(() => isDisplayed = !isDisplayed);
            ToggleIPEntriesCommand = new Command(() => isShow = !isShow);
            SaveContinueCommand = new Command(() => ExecuteSaveContinueCommand());
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


                new KeyValuePair<string, string>("comment", Comment),
                new KeyValuePair<string, string>("manufactured_date", SelectedManufDate), //Manufactured),
                new KeyValuePair<string, string>("installed2",SelectedInstallDate), //InstalledAt),


                new KeyValuePair<string, string>("ipaddr", IsIPAddressValid(IP1 + "." + IP2 + "." + IP3 + "." + IP4)),
                new KeyValuePair<string, string>("subnet", IsIPAddressValid(Subnet1 + "." + Subnet2 + "." + Subnet3 + "." + Subnet4)),
                new KeyValuePair<string, string>("protocol", Protocol),
                new KeyValuePair<string, string>("vidioproto", VideoProtocol),
                new KeyValuePair<string, string>("vlan", VLAN),

                new KeyValuePair<string, string>("getway", IsIPAddressValid(GWIP1 + "." + GWIP2 + "." + GWIP3 + "." + GWIP4)),
                new KeyValuePair<string, string>("multicastip", IsIPAddressValid(MCast1 + "." + MCast2 + "." + MCast3 + "." + MCast4)),

                new KeyValuePair<string, string>("slotblade", SelectedSlotBladeTray),
                new KeyValuePair<string, string>("position", SelectedPosition),
                new KeyValuePair<string, string>("rack_number", SelectedRackNumber.Racknumber),


            };


            return keyValues;

        }
        string IsIPAddressValid(string ipaddress)
        {
            //  Split string by ".", check that array length is 4
            string[] arrOctets = ipaddress.Split('.');
            if (arrOctets.Length != 4)
                return "0.0.0.0";

            //Check each substring checking that parses to byte
            byte obyte = 0;
            foreach (string strOctet in arrOctets)
                if (!byte.TryParse(strOctet, out obyte))
                    return "0.0.0.0";

            return ipaddress;
        }

        private async void ExecuteSaveContinueCommand()
        {
            var KVPair = keyvaluepair();
            var result = await CloudDBService.PostActiveDevice(KVPair);
            if(result.Equals("OK"))
            {
                // Do something
                Console.WriteLine();
            }
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
