using FTCollectorApp.Model.Reference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using SQLite;
using Xamarin.Forms;
using System.Threading.Tasks;
using FTCollectorApp.View.SitesPage;
using CommunityToolkit.Mvvm.ComponentModel;
using FTCollectorApp.Model;
using FTCollectorApp.Service;


namespace FTCollectorApp.ViewModel
{
    public partial class DuctViewModel : ObservableObject
    {
        [ObservableProperty]
        string defaultHostTagNumber;

        [ObservableProperty]
        bool isBusy;


        public DuctViewModel()
        {

            // from Duct Color Selection PopUp

            ColorSelectedCommand = new Command(ductcolor => ExecuteColorSelectedCommand(ductcolor as ColorCode));

            // from DuctPage
            ShowPopupCommand = new Command(async _ => await ExecuteShowPopupCommand());
            SaveCommand = new Command(
                execute: async () =>
                {
                    var KVPair = keyvaluepair();
                    string result = await CloudDBService.PostDuctSave(KVPair); // async upload to AWS table
                    if (result.Equals("OK"))
                    {
                        Console.WriteLine();
                        var num = int.Parse(SelectedDirectionCnt) + 1;
                        SelectedDirectionCnt = num.ToString();
                    }
                });
            SaveBackCommand = new Command(
                execute: async () =>
                {
                    Console.WriteLine();
                    await Application.Current.MainPage.Navigation.PopAsync();

                });
            RefreshDuctKeyListCommand = new Command(
                execute: async () =>
                {

                    Console.WriteLine();
                    IsBusy = true;
                    var contentConduit = await CloudDBService.GetConduits(); // async download from AWS table
                    if (contentConduit.ToString().Length > 20)
                    {
                        using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                        {
                            conn.CreateTable<ConduitsGroup>();
                            conn.DeleteAll<ConduitsGroup>();
                            conn.InsertAll(contentConduit);
                        }

                        Console.WriteLine();
                        OnPropertyChanged(nameof(DuctKeyList)); // update CONDUIT_GROUPS dropdown list
                    }
                    IsBusy = false;
                    Console.WriteLine();
                });
            DefaultHostTagNumber = Session.tag_number;

        }
        public ICommand SaveCommand { get; set; }
        public ICommand SaveBackCommand { get; set; }
        public ICommand RefreshDuctKeyListCommand { get; set; }
        void RefreshCanExecutes()
        {
            (SaveCommand as Command).ChangeCanExecute();
            (SaveBackCommand as Command).ChangeCanExecute();
        }

        /// get selected color from popup - start
        [ObservableProperty]
        ColorCode selectedColor;

        public ICommand ShowPopupCommand { get; set; }
        public ICommand ColorSelectedCommand { get; set; }
        private Task ExecuteShowPopupCommand()
        {
            var popup = new DuctColorCodePopUp(SelectedColor)
            {
                ColorSelectedCommand = ColorSelectedCommand
            };
            return Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
        }
        // with Mode=TwoWay, no need this ?
        private void ExecuteColorSelectedCommand(ColorCode ductcolor)
        {
            SelectedColor = ductcolor;
            Console.WriteLine();
        }



        /// get selected color from popup - end
        /// 

        // Duct Page - start
        [ObservableProperty]
        ConduitsGroup selectedDuctKey;

        public ObservableCollection<ConduitsGroup> DuctKeyList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ConduitsGroup>();
                    var table = conn.Table<ConduitsGroup>().Where(a=>(a.HosTagNumber == Session.tag_number)&&(a.OwnerKey ==  Session.ownerkey)).ToList();
                    return new ObservableCollection<ConduitsGroup>(table);
                }
            }
        }
        public ObservableCollection<DuctType> DuctMaterialList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<DuctType>();
                    var table = conn.Table<DuctType>().ToList();
                    return new ObservableCollection<DuctType>(table);
                }
            }
        }
        public ObservableCollection<DuctUsed> DuctUsageList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<DuctUsed>();
                    var table = conn.Table<DuctUsed>().ToList();
                    return new ObservableCollection<DuctUsed>(table);
                }
            }
        }

        public ObservableCollection<DuctSize> DuctSizeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<DuctSize>();
                    var table = conn.Table<DuctSize>().ToList();
                    return new ObservableCollection<DuctSize>(table);
                }
            }
        }

        public ObservableCollection<DuctInstallType> DuctInstallList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<DuctInstallType>();
                    var table = conn.Table<DuctInstallType>().ToList();
                    return new ObservableCollection<DuctInstallType>(table);
                }
            }
        }
        // Duct Page - end

        public ObservableCollection<CompassDirection> TravelDirectionList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CompassDirection>();
                    var data = conn.Table<CompassDirection>().ToList();
                    return new ObservableCollection<CompassDirection>(data);
                }
            }
        }

        [ObservableProperty]
        string selectedDirectionCnt = "1";

        [ObservableProperty]
        string isPlugged;

        [ObservableProperty]
        string isOpen;

        [ObservableProperty]
        string hasPullTape;

        [ObservableProperty]
        string hasInnerDuct;

        [ObservableProperty]
        string hasTraceWire;

        [ObservableProperty]
        string percentOpen;

        [ObservableProperty]
        DuctSize selectedDuctSize;

        [ObservableProperty]
        DuctType selectedDuctType;

        [ObservableProperty]
        DuctInstallType selectedDuctInstallType;

        [ObservableProperty]
        CompassDirection selectedDirection;

        List<KeyValuePair<string, string>> keyvaluepair()
        {
            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), // 
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // 1
                new KeyValuePair<string, string>("host_tag_number", Session.tag_number),  // 2
                new KeyValuePair<string, string>("direction", SelectedDirection?.CompasKey  == null ? "0": SelectedDirection.CompasKey),  // 3
                new KeyValuePair<string, string>("direction_count", SelectedDirectionCnt ??= "0"),  // 4
                new KeyValuePair<string, string>("duct_size",  SelectedDuctSize?.DuctKey == null ? "0": SelectedDuctSize.DuctKey),  // 5
                new KeyValuePair<string, string>("duct_color", SelectedColor?.ColorKey == null ? "0": SelectedColor.ColorKey),  // 6
                new KeyValuePair<string, string>("duct_type",  selectedDuctType?.DucTypeKey == null ?"0" : SelectedDuctType.DucTypeKey),  // 7
                new KeyValuePair<string, string>("site_type_key", Session.site_type_key),  // 8
                new KeyValuePair<string, string>("duct_usage", "0"),  // 9
                new KeyValuePair<string, string>("duct_grouptype", "0"),  // 9
                new KeyValuePair<string, string>("duct_groupid", "0"),  // 9
                new KeyValuePair<string, string>("duct_inuse", "1"),  // 9
                new KeyValuePair<string, string>("duct_trace", "0"),  // 9

                new KeyValuePair<string, string>("install", SelectedDuctInstallType?.DuctInstallKey == null ? "0":  SelectedDuctInstallType.DuctInstallKey),  // 10


                new KeyValuePair<string, string>("openpercent", PercentOpen ??= "100"),  // 11
                new KeyValuePair<string, string>("has_trace_wire", HasTraceWire ??= "0"),  // 12
                new KeyValuePair<string, string>("has_pull_tape", HasPullTape ??= "0"),  // 12
            };


            return keyValues;

        }
    }
}
