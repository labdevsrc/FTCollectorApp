using FTCollectorApp.Model.Reference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SQLite;
using System.Web;
using FTCollectorApp.Model;
using FTCollectorApp.Service;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace FTCollectorApp.ViewModel
{
    public partial class RacksViewModel : ObservableObject
    {

        public RacksViewModel()
        {

            SaveandBackCommand = new Command(async _ => await ExecuteSaveandBackCommand());
            SaveCommand = new Command(async _ => await ExecuteSaveCommand());

        }


        [ObservableProperty]
        string selectedRackNumber;

        [ObservableProperty]
        string selectedOrientation;

        [ObservableProperty]
        string selectedFrontBack;

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

        /*string _selectedRackNumber;
        public string SelectedRackNumber
        {
            get => _selectedRackNumber;
            set => SetProperty(ref _selectedRackNumber, value);
        }


        string _selectedOrientation;
        public string SelectedOrientation
        {
            get => _selectedOrientation;
            set => SetProperty(ref _selectedOrientation, value);
        }
        string _selectedFrontBack;
        public string SelectedFrontBack
        {
            get => _selectedFrontBack;
            set => SetProperty(ref _selectedFrontBack, value);
        }

        Manufacturer _manufacturerSelected;
        public Manufacturer SelectedManufacturer
        {
            get => _manufacturerSelected;
            set
            {
                SetProperty(ref _manufacturerSelected, value);
                _modelDetailList.Where(a => a.ManufKey == value.ManufKey);
                OnPropertyChanged(nameof(ModelDetailList));
                Console.WriteLine();
            }
        }

        ModelDetail _modeldetailSelected;
        public ModelDetail SelectedModelDetail
        {
            get => _modeldetailSelected;
            set
            {
                SetProperty(ref _modeldetailSelected, value);
                Console.WriteLine();
            }
        }

        RackType _selectedRackType;
        public RackType SelectedRackType
        {
            get => _selectedRackType;
            set
            {
                SetProperty(ref _selectedRackType, value);
                Console.WriteLine();
            }
        }*/


        public ObservableCollection<RackType> RackTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<RackType>();
                    var table = conn.Table<RackType>().ToList();
                    return new ObservableCollection<RackType>(table);
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

        List<KeyValuePair<string, string>> keyvaluepair()
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

                new KeyValuePair<string, string>("xpos", ""),
                new KeyValuePair<string, string>("ypos", ""),
                new KeyValuePair<string, string>("manufacturer", SelectedManufacturer.ManufKey),
                //new KeyValuePair<string, string>("manufacturer", ManufacturerKeySelected), 
                new KeyValuePair<string, string>("model", SelectedModelDetail.ModelKey),

                new KeyValuePair<string, string>("height", SelectedModelDetail.height),
                new KeyValuePair<string, string>("width", SelectedModelDetail.width),
                new KeyValuePair<string, string>("depth", SelectedModelDetail.depth)
            };

            Console.WriteLine();

            return keyValues;

        }

        public ICommand SaveandBackCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        private async Task ExecuteSaveandBackCommand()
        {
            save();
            Console.WriteLine();
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async Task ExecuteSaveCommand()
        {
            save();
        }

        async void save()
        {
            var KVPair = keyvaluepair();

            var result = await CloudDBService.PostSaveRacks(KVPair);
            if (result.Equals("OK"))
            {
                Console.WriteLine();
                //ResultCommand?.Execute("OK");
                //await DisplayAlert("Success", "Uploading Data Done", "OK");
            }
            else
            {
                Console.WriteLine();
                //ResultCommand?.Execute("FAIL");
                //await DisplayAlert("Warning", result, "OK");
            }
        }
    }
}
