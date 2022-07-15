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

namespace FTCollectorApp.ViewModel
{
    public partial class EndTraceViewModel: ObservableObject
    {
        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(DuctConduitDatas))]
        ConduitsGroup selectedTagNum;

        ConduitsGroup selectedDuct;
        public ConduitsGroup SelectedDuct
        {
            get => selectedDuct;
            set
            {
                SetProperty(ref selectedDuct, value);
                // backup selected duct 
                Session.ToDuct = value;
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
                new KeyValuePair<string, string>("duct_to_key", Session.ToDuct?.ConduitKey is null ? "0" :Session.ToDuct.ConduitKey ),

                new KeyValuePair<string, string>("tag_from", Session.FromDuct?.HosTagNumber is null ? "0" :Session.FromDuct.HosTagNumber ),
                new KeyValuePair<string, string>("tag_from_key", Session.FromDuct?.HostSiteKey is null ? "0" :Session.FromDuct.HostSiteKey ),
                //new KeyValuePair<string, string>("cable_id1", Session.Cable1.AFRKey),
                //new KeyValuePair<string, string>("cable_type", Session.Cable1.CableType),

                //new KeyValuePair<string, string>("lattitude", Session.lattitude2),
                //new KeyValuePair<string, string>("longitude", Session.longitude2),
                //new KeyValuePair<string, string>("altitude", Session.altitude),
                //new KeyValuePair<string, string>("accuracy", Session.accuracy),

                //new KeyValuePair<string, string>("gps_offset_latitude", Session.lattitude_offset),
                //new KeyValuePair<string, string>("gps_offset_longitude", Session.longitude_offset),

                //new KeyValuePair<string, string>("gps_offset_bearing", Session.gps_offset_bearing),
                //new KeyValuePair<string, string>("gps_offset_distance", Session.gps_offset_distance),

                //new KeyValuePair<string, string>("comment", CommentText),

                //new KeyValuePair<string, string>("site_type", SelectedSiteType?.IdLocatePoint is null ? "0" :SelectedSiteType.IdLocatePoint),


            };

            return keyValues;

        }

        async void ExecuteSuspendCommand()
        {
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
                    if (SelectedTagNum?.HosTagNumber != null)
                        table = ConduitsGroupListTable.Where(b => b.HosTagNumber == SelectedTagNum.HosTagNumber).ToList();
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
    }


    
}
