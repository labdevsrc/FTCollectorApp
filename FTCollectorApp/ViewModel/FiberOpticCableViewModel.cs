using CommunityToolkit.Mvvm.ComponentModel;
using FTCollectorApp.Model.Reference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SQLite;
using System.Web;
using FTCollectorApp.Model;
using System.Linq;
using System.Windows.Input;
using FTCollectorApp.Service;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace FTCollectorApp.ViewModel
{


    public partial class FiberOpticCableViewModel : ObservableObject
    {


        public FiberOpticCableViewModel()
        {
            Console.WriteLine();
            SaveandBackCommand = new Command(async _ => ExecuteSaveandBackCommand());
            SaveCommand = new Command(async _ => ExecuteSaveCommand());
        }


        [ObservableProperty]
        string newCableName;

        [ObservableProperty]
        AFiberCable selectedFiberCable;


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
        CableType selectedCableType;

        [ObservableProperty]
        string _SMCount;

        [ObservableProperty]
        string _MMCount;

        [ObservableProperty]
        string selectedBufferCnt;

        [ObservableProperty]
        TwoColor selectedColor;

        [ObservableProperty]
        Sheath selectedSheath;

        [ObservableProperty]
        ReelId selectedReelId;

        [ObservableProperty]
        string selectedManufacturedDate;


        [ObservableProperty]
        string selectedInstalledAt;

        [ObservableProperty]
        FiberInstallType selectedInstallType;

        [ObservableProperty]
        string textLabel;

        // multi mode diameter color        
        public ObservableCollection<TwoColor> ClrCodeList
        {
            get
            {
                List<TwoColor> temp = new List<TwoColor>();
                temp.Add(new TwoColor { ClrKey = "12", ClrName = "Aqua", ClrHex = "#00FFFF" });
                temp.Add(new TwoColor { ClrKey = "2", ClrName = "Orange", ClrHex = "#FFA500" });
                return new ObservableCollection<TwoColor>(temp);

            }
        }


        public ObservableCollection<ReelId> ReelIdList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ReelId>();
                    var rwTable = conn.Table<ReelId>().ToList();
                    var table = rwTable.Where(a => a.JobNum == Session.jobnum).ToList();
                    Console.WriteLine();
                    return new ObservableCollection<ReelId>(table);
                }
            }
        }


        public ObservableCollection<AFiberCable> aFiberCableList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    // create dummy list 
                    List<AFiberCable> temp = new List<AFiberCable>();
                    temp.Add(new AFiberCable
                    {
                        CableIdDesc = "New"
                    });

                    conn.CreateTable<AFiberCable>();
                    var table = conn.Table<AFiberCable>().Where(a => a.OwnerKey == Session.ownerkey).ToList();
                    temp.AddRange(table);
                    foreach (var col in table)
                    {
                        col.CableIdDesc = HttpUtility.HtmlDecode(col.CableIdDesc); // should use for escape char "
                    }
                    Console.WriteLine();
                    return new ObservableCollection<AFiberCable>(temp);
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

        public ObservableCollection<CableType> CableTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CableType>();
                    var table = conn.Table<CableType>().ToList();
                    return new ObservableCollection<CableType>(table);
                }
            }
        }

        public ObservableCollection<Sheath> SheathList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Sheath>();
                    var table = conn.Table<Sheath>().ToList();
                    return new ObservableCollection<Sheath>(table);
                }
            }
        }

        public ObservableCollection<FiberInstallType> InstallTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<FiberInstallType>();
                    var table = conn.Table<FiberInstallType>().ToList();
                    return new ObservableCollection<FiberInstallType>(table);
                }
            }
        }

        public ICommand SaveandBackCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        private async Task ExecuteSaveandBackCommand()
        {
            save();
            Console.WriteLine();
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        List<KeyValuePair<string, string>> keyvaluepair()
        {
            Console.WriteLine();

            string CableName = string.IsNullOrWhiteSpace(newCableName) ? SelectedFiberCable.CableIdDesc : NewCableName; 
            //get max index in a_fiber_cable table, then +1 to get corect table
            var keyValues = new List<KeyValuePair<string, string>>{

                new KeyValuePair<string, string>("cable_id",CableName), // this is should be 
                new KeyValuePair<string, string>("manufacturer", SelectedManufacturer.ManufKey),
                new KeyValuePair<string, string>("model", SelectedModelDetail.ModelKey),
                new KeyValuePair<string, string>("manufactured_date", SelectedManufacturedDate),
                new KeyValuePair<string, string>("label", textLabel),
                new KeyValuePair<string, string>("cablelen", ""),



                new KeyValuePair<string, string>("singlemode_count", SMCount), //3
                new KeyValuePair<string, string>("multimode_count", MMCount),  //4
                new KeyValuePair<string, string>("buffer_count", SelectedBufferCnt), //1
                new KeyValuePair<string, string>("reel", SelectedReelId.ReelKey), // 6
                new KeyValuePair<string, string>("installed_date",SelectedInstalledAt), //  7 

                new KeyValuePair<string, string>("cabtype", SelectedCableType.CodeCableKey), //8
                new KeyValuePair<string, string>("installtyp", selectedInstallType.FbrInstallKey),  /// site_id
                new KeyValuePair<string, string>("sheath", SelectedSheath.SheathKey),  /// code_site_type.key
                new KeyValuePair<string, string>("multimode_diameter", SelectedColor.ClrKey),

                new KeyValuePair<string, string>("oid", Session.ownerkey), //1
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // #1
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), // 6
                new KeyValuePair<string, string>("jobkey", Session.jobkey),
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),  // 2
                new KeyValuePair<string, string>("jobnum",Session.jobnum), //  7 
                new KeyValuePair<string, string>("stage", Session.stage),
                new KeyValuePair<string, string>("country", ""),

            };
            Console.WriteLine();

            return keyValues;

        }

        private async Task ExecuteSaveCommand()
        {
            await save();
        }

        async Task save()
        {
            var KVPair = keyvaluepair();

            string result = await CloudDBService.PostSaveFiberOpticCable(KVPair);
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
