using CommunityToolkit.Mvvm.ComponentModel;
using FTCollectorApp.Model.Reference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SQLite;
using System.Web;
using FTCollectorApp.Model;
using Xamarin.Forms;
using System.Windows.Input;
using FTCollectorApp.Service;
using Newtonsoft.Json;
using FTCollectorApp.View;

namespace FTCollectorApp.ViewModel
{
    public partial class EndTraceViewModel: ObservableObject
    {
        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(DuctConduitDatas))]
        ConduitsGroup selectedTagNum;

        string[] colorFiberHex = { "#0000FF", "#FFA500", "#008000", "#A52A2A", "#708090", "#FFFFFF", "#FF0000","#00000", "#FFFF00", "#963D7F", "#FF00FF", "#00FFFF" };
        string[] colorFiber = { "Blue", "Orange", "Green", "Brown", "Slate", "White", "Red", "Black", "Yellow", "Violet", "Rose", "Aqua" };

        [ObservableProperty]
        bool isEntriesDiplayed = true;

        // flag for Hide or show listview 
        [ObservableProperty]
        bool isSearching = false;


        // selected tag num in listview
        ConduitsGroup selectedSiteIn;
        public ConduitsGroup SelectedSiteIn
        {
            get
            {
                Console.WriteLine(  );

                return selectedSiteIn;
            }
            set 
            {
                Console.WriteLine();
                SetProperty(ref (selectedSiteIn), value);
                SearchTag = value.HosTagNumber;
                OnPropertyChanged(nameof(DuctConduitDatas));
                OnPropertyChanged(nameof(SearchTag));
            }

        }


        // search bar object
        string searchTag;
        public string SearchTag
        {
            get
            {
                Console.WriteLine();
                return searchTag;
            }
            set
            {
                IsSearching = string.IsNullOrEmpty(value) ? false:true;
                IsEntriesDiplayed = true;
                SetProperty(ref (searchTag), value);

                OnPropertyChanged(nameof(SiteInListView));
                Console.WriteLine(  );
            }
        }


        ConduitsGroup selectedDuct;
        public ConduitsGroup SelectedDuct
        {
            get=> selectedDuct;

            set
            {

                if (value?.DuctColor != null)
                {
                    if (int.Parse(value.DuctColor) > 0)
                    {
                        value.ColorName = colorFiber[int.Parse(value.DuctColor) - 1];
                        value.ColorHex = colorFiberHex[int.Parse(value.DuctColor) - 1];
                    }
                }
                Console.WriteLine();
                
                Session.ToDuct = value;
                SetProperty(ref selectedDuct, value);

            }
        }

        [ObservableProperty]
        ConduitsGroup toDuct;

        ObservableCollection<ConduitsGroup> ConduitsGroupListTable;
        ObservableCollection<ColorCode> ColorHextList;

        public ICommand CompleteFiberCommand { get; set; }
        public ICommand SuspendCommand { get; set; }
        public ICommand BrokenTraceWireCommand { get; set; }
        public ICommand DeleteTraceCommand { get; set; }
        public ICommand CreateNewCommand { get; set; }

        


        public EndTraceViewModel()
        {



            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {


                conn.CreateTable<ConduitsGroup>();
                var table1 = conn.Table<ConduitsGroup>().ToList();
                ConduitsGroupListTable = new ObservableCollection<ConduitsGroup>(table1);

                conn.CreateTable<ColorCode>();
                var table2 = conn.Table<ColorCode>().ToList();
                ColorHextList = new ObservableCollection<ColorCode>(table2);
            }

            CompleteFiberCommand = new Command(ExecuteCompleteFiberCommand);
            SuspendCommand = new Command(ExecuteSuspendCommand);
            BrokenTraceWireCommand = new Command(ExecuteBrokenTraceWireCommand);
            DeleteTraceCommand = new Command(ExecuteDeleteTraceCommand);
            CreateNewCommand = new Command(ExecuteCreateNewCommand);
        }

        void ExecuteCreateNewCommand()
        {
            IsEntriesDiplayed = false;
            IsSearching = false;

            OnPropertyChanged(nameof(SelectedSiteIn));
            Console.WriteLine();
        }


        List<KeyValuePair<string, string>> keyvaluepair()
        {
            //Session.GpsPointMaxIdx = (int.Parse(maxGPSpoint?.MaxId) + 1).ToString();

            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new KeyValuePair<string, string>("jobnum", Session.jobnum),
                new KeyValuePair<string, string>("ownerkey", Session.ownerkey),
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD),

                //new KeyValuePair<string, string>("locate_point_number", LocPointNumber.ToString()),
                new KeyValuePair<string, string>("locpoint_numstart", Session.LocpointnumberStart is null ? "0" : Session.LocpointnumberStart ),
                new KeyValuePair<string, string>("locpoint_numend", Session.LocpointnumberEnd is null ? "0" : Session.LocpointnumberEnd ),
                new KeyValuePair<string, string>("tag_to", Session.ToDuct?.HosTagNumber is null ? "0" :Session.ToDuct.HosTagNumber ),
                new KeyValuePair<string, string>("tag_to_key", Session.ToDuct?.HostSiteKey is null ? "0" :Session.ToDuct.HostSiteKey ),
                new KeyValuePair<string, string>("duct_to", Session.ToDuct?.ConduitKey is null ? "0" :Session.ToDuct.ConduitKey ),

                new KeyValuePair<string, string>("from_site", Session.FromDuct?.HosTagNumber is null ? "0" :Session.FromDuct.HosTagNumber ),
                new KeyValuePair<string, string>("from_site_key", Session.FromDuct?.HostSiteKey is null ? "0" :Session.FromDuct.HostSiteKey ),

            };

            return keyValues;

        }


        async void ExecuteSuspendCommand()
        {
            Application.Current.Properties[Constants.SavedFromDuctTagNumber] = Session.FromDuct?.HosTagNumber;
            Application.Current.Properties[Constants.SavedFromDuctTagNumberKey] = Session.FromDuct?.ConduitKey;
            //Application.Current.Properties[Constants.SavedToDuctTagNumber] = Session.ToDuct?.HosTagNumber;
            //Application.Current.Properties[Constants.SavedToDuctTagNumberKey] = Session.ToDuct?.ConduitKey;
            await Application.Current.MainPage.Navigation.PushAsync(new AsBuiltDocMenu());
            // ToDo
        }


        async void ExecuteBrokenTraceWireCommand()
        { 
            // ToDo
        }


        async void ExecuteDeleteTraceCommand()
        {
            // ToDo
            var KVPair = keyvaluepair();
            try
            {
                // JSON convert and send to AWS 
                var result = await CloudDBService.PostDuctTrace(KVPair);

                Console.WriteLine(result);

                if (result.Length > 30)
                {

                    var contentResponse = JsonConvert.DeserializeObject<ResponseKeyList>(result);
                    Console.WriteLine();

                    Session.GpsPointMaxIdx = contentResponse?.locatepointkey is null ? "0" : contentResponse.locatepointkey;



                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        async void ExecuteCompleteFiberCommand()
        {
            // put to key map before convert to json
            var KVPair = keyvaluepair();


            try
            {
                // JSON convert and send to AWS 
                var result = await CloudDBService.PostEndDuctTrace(KVPair);

                Console.WriteLine(result);

                if (result.Length > 30)
                {

                    var contentResponse = JsonConvert.DeserializeObject<ResponseKeyList>(result);
                    Console.WriteLine();

                    Session.GpsPointMaxIdx = contentResponse?.locatepointkey is null ? "0" : contentResponse.locatepointkey;

                    Application.Current.Properties[Constants.SavedFromDuctTagNumber] = "";
                    Application.Current.Properties[Constants.SavedFromDuctTagNumberKey] = "";
                    Application.Current.Properties[Constants.SavedToDuctTagNumber] = "";
                    Application.Current.Properties[Constants.SavedToDuctTagNumberKey] = "";

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        public ObservableCollection<ConduitsGroup> DuctConduitDatas
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    var table = ConduitsGroupListTable.ToList();
                    if (SelectedSiteIn?.HosTagNumber != null)
                        table = ConduitsGroupListTable.Where(b => b.HosTagNumber == SelectedSiteIn.HosTagNumber).ToList();

                    foreach (var col in table)
                    {

                        col.DuctSize = HttpUtility.HtmlDecode(col.DuctSize);
                        col.WhichDucts = col.Direction + " " + col.DirCnt;
                    }
                    Console.WriteLine("DuctConduitDatas ");
                    return new ObservableCollection<ConduitsGroup>(table);
                }
            }
        }

        public ObservableCollection<ConduitsGroup> SiteInList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    // create dummy list 
                    List<ConduitsGroup> temp = new List<ConduitsGroup>();
                    temp.Add(new ConduitsGroup
                    {
                        HosTagNumber = "New"
                    });

                    var table = ConduitsGroupListTable.GroupBy(b => b.HosTagNumber).Select(g => g.First()).ToList();

                    temp.AddRange(table);

                    Console.WriteLine();
                    return new ObservableCollection<ConduitsGroup>(temp);
                }
            }
        }

        public ObservableCollection<ConduitsGroup> SiteInListView
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {

                    var table = ConduitsGroupListTable.GroupBy(b => b.HosTagNumber).Select(g => g.First()).ToList();
                    if (SearchTag != null)
                    {
                        Console.WriteLine();
                        table = ConduitsGroupListTable.Where(i => i.HosTagNumber.ToLower().Contains(SearchTag.ToLower())).
                            GroupBy(b => b.HosTagNumber).Select(g => g.First()).ToList();
                    }
                    Console.WriteLine();
                    return new ObservableCollection<ConduitsGroup>(table);
                }
            }
        }
    }


    
}
