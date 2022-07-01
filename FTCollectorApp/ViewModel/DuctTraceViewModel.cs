using CommunityToolkit.Mvvm.ComponentModel;
using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Service;
using FTCollectorApp.View;
using FTCollectorApp.View.TraceFiberPages;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Input;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{
    public partial class DuctTraceViewModel: ObservableObject
    {
        [ObservableProperty]
        AFiberCable selectedCable1;

        [ObservableProperty]
        AFiberCable selectedCable2;

        [ObservableProperty]
        AFiberCable selectedCable3;

        [ObservableProperty]
        AFiberCable selectedCable4;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(DuctConduitDatas))]
        ConduitsGroup selectedTagNum;

        [ObservableProperty]
        ConduitsGroup selectedDuct;

        [ObservableProperty]
        DuctInstallType selectedDuctInstall;

        [ObservableProperty]
        UnitOfMeasure selectedUOM;


        [ObservableProperty]
        string sheathMark1;

        [ObservableProperty]
        string sheathMark2;

        [ObservableProperty]
        string sheathMark3;

        [ObservableProperty]
        string sheathMark4;



        ObservableCollection<ConduitsGroup> ConduitsGroupListTable;
        ObservableCollection<ColorCode> ColorHextList;

        public ICommand SaveAndContinueCommand { get; set; }
        public ICommand RemoveCable1Command { get; set; }

        public ICommand RemoveCable2Command { get; set; }
        public ICommand RemoveCable3Command { get; set; }
        public ICommand RemoveCable4Command { get; set; }
        public DuctTraceViewModel()
        {
            SaveAndContinueCommand = new Command(ExecuteSaveAndContinueCommand);
            RemoveCable1Command = new Command(ExecuteRemoveCable1Command);
            RemoveCable2Command = new Command(ExecuteRemoveCable2Command);
            RemoveCable3Command = new Command(ExecuteRemoveCable3Command);
            RemoveCable4Command = new Command(ExecuteRemoveCable4Command);


            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<ConduitsGroup>();
                var table1 = conn.Table<ConduitsGroup>().ToList();
                ConduitsGroupListTable = new ObservableCollection<ConduitsGroup>(table1);

                conn.CreateTable<ColorCode>();
                var table2 = conn.Table<ColorCode>().ToList();
                ColorHextList = new ObservableCollection<ColorCode>(table2);
            }
        }

        private void ExecuteRemoveCable1Command()
        {
            SelectedCable1 = null;
            SheathMark1 = "";
        }

        private void ExecuteRemoveCable2Command()
        {
            SelectedCable2 = null;
            SheathMark2 = "";
        }

        private void ExecuteRemoveCable3Command()
        {
            SelectedCable3 = null;
            SheathMark3 = "";
        }

        private void ExecuteRemoveCable4Command()
        {
            SelectedCable4 = null;
            SheathMark4 = "";
        }


        public ObservableCollection<AFiberCable> aFiberCableList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<AFiberCable>();
                    // City of Port St Lucie for Demo purpose
                    var table = conn.Table<AFiberCable>().Where(a => a.OwnerKey == Session.ownerkey).ToList();
                    foreach (var col in table)
                    {
                        col.CableIdDesc = HttpUtility.HtmlDecode(col.CableIdDesc); // should use for escape char "
                    }
                    Console.WriteLine();
                    return new ObservableCollection<AFiberCable>(table);
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
        public ObservableCollection<UnitOfMeasure> UnitOfMeasures
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<UnitOfMeasure>();
                    var table = conn.Table<UnitOfMeasure>().ToList();
                    return new ObservableCollection<UnitOfMeasure>(table);
                }
            }
        }

        public ObservableCollection<ConduitsGroup> BeginningSiteList
        {
            get
            {
                var table = ConduitsGroupListTable.GroupBy(b => b.HosTagNumber).Select(g => g.First()).ToList();
                Console.WriteLine("BeginningSite");
                return new ObservableCollection<ConduitsGroup>(table);

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
                    {
                        table = ConduitsGroupListTable.Where(b => b.HosTagNumber == SelectedTagNum.HosTagNumber).ToList();
                        Console.WriteLine();
                    }
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

        public ObservableCollection<Site> Sites
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Site>();
                    var tableSite = conn.Table<Site>().OrderBy(g => g.TagNumber).ToList();
                    return new ObservableCollection<Site>(tableSite);
                }
            }
        }


        List<KeyValuePair<string, string>> keyvaluepair()
        {
            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("ownerkey", Session.ownerkey), // 
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), // 
                new KeyValuePair<string, string>("job", Session.jobnum), // 
                new KeyValuePair<string, string>("job_key", Session.jobkey), // 

                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // 1
                new KeyValuePair<string, string>("from_site", SelectedTagNum?.HosTagNumber is null ?"0" : SelectedTagNum.HosTagNumber  ),
                new KeyValuePair<string, string>("from_site_key", SelectedTagNum?.HostSiteKey is null ?"0" : SelectedTagNum.HostSiteKey  ),
                new KeyValuePair<string, string>("from_site_duct", SelectedDuct?.HosTagNumber is null ?"0" : SelectedDuct.HosTagNumber  ),  // 2
                new KeyValuePair<string, string>("from_site_duct_key", SelectedDuct?.ConduitKey is null ?"0" : SelectedDuct.ConduitKey  ),  // 2
                new KeyValuePair<string, string>("install_method", SelectedDuctInstall?.DuctInstallKey is null ? "0":SelectedDuctInstall.DuctInstallKey),  // 7
                new KeyValuePair<string, string>("uom", selectedUOM?.UOMKey is null ? "0":selectedUOM.UOMKey),  // 7
                new KeyValuePair<string, string>("stage", Session.stage),  // 7
                new KeyValuePair<string, string>("sheath_mark1", SheathMark1),
                new KeyValuePair<string, string>("sheath_mark2", SheathMark2),
                new KeyValuePair<string, string>("sheath_mark3", SheathMark3),
                new KeyValuePair<string, string>("sheath_mark4", SheathMark4),

                new KeyValuePair<string, string>("cable_type", Session.jobkey), // 
                new KeyValuePair<string, string>("from_site_duct_direction", SelectedDuct?.Direction is null ? "":SelectedDuct.Direction),
                new KeyValuePair<string, string>("from_site_duct_direction_count", SelectedDuct?.DirCnt is null ? "":SelectedDuct.DirCnt),

                new KeyValuePair<string, string>("cable_id1", SelectedCable1?.CableIdDesc is null ? "":SelectedCable1.CableIdDesc),
                new KeyValuePair<string, string>("cable_id2", SelectedCable2?.CableIdDesc is null ? "":SelectedCable2.CableIdDesc),
                new KeyValuePair<string, string>("cable_id3", SelectedCable3?.CableIdDesc is null ? "":SelectedCable3.CableIdDesc),
                new KeyValuePair<string, string>("cable_id4", SelectedCable4?.CableIdDesc is null ? "":SelectedCable4.CableIdDesc),

                new KeyValuePair<string, string>("cable_type1", SelectedCable1?.CableType is null ? "":SelectedCable1.CableType),
                new KeyValuePair<string, string>("cable_type2", SelectedCable2?.CableType is null ? "":SelectedCable2.CableType),
                new KeyValuePair<string, string>("cable_type3", SelectedCable3?.CableType is null ? "":SelectedCable3.CableType),
                new KeyValuePair<string, string>("cable_type4", SelectedCable4?.CableType is null ? "":SelectedCable4.CableType),

                new KeyValuePair<string, string>("cable_id1_key", SelectedCable1?.AFRKey is null ? "":SelectedCable1.AFRKey),
                new KeyValuePair<string, string>("cable_id2_key", SelectedCable2?.AFRKey is null ? "":SelectedCable2.AFRKey),
                new KeyValuePair<string, string>("cable_id3_key", SelectedCable3?.AFRKey is null ? "":SelectedCable3.AFRKey),
                new KeyValuePair<string, string>("cable_id4_key", SelectedCable4?.AFRKey is null ? "":SelectedCable4.AFRKey),


            };
            return keyValues;

        }

        private async void ExecuteSaveAndContinueCommand()
        {
            var KVPair = keyvaluepair();
            var result = await CloudDBService.PostDuctTrace(KVPair);
            Console.WriteLine(result);
            if (result.Length > 30)
            {
                var contentResponse = JsonConvert.DeserializeObject<ResponseKeyList>(result);

                Session.TraceCable1Idx = contentResponse?.key1 is null ? "0" : contentResponse?.key1;
                Session.TraceCable2Idx = contentResponse?.key2 is null ? "0" : contentResponse?.key2;
                Session.TraceCable3Idx = contentResponse?.key3 is null ? "0" : contentResponse?.key3;
                Session.TraceCable4Idx = contentResponse?.key4 is null ? "0" : contentResponse?.key4;
                Session.GpsPointMaxIdx = contentResponse?.locatepointkey is null ? "0" : contentResponse?.locatepointkey;

                Console.WriteLine();
            }
            await Application.Current.MainPage.Navigation.PushAsync(new LocatePointPage());
        }
    }
}
