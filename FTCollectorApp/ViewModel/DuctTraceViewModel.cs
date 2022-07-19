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

        ConduitsGroup selectedDuct;
        public ConduitsGroup SelectedDuct
        {
            get => selectedDuct;
            set
            {
                SetProperty(ref selectedDuct, value);
                // backup selected duct 
                Session.FromDuct = value;
            }
        }

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

            /*SelectedCable1 = new AFiberCable { CableIdDesc ="", FiberSegmentIdx = "0" };
            SelectedCable2 = new AFiberCable { CableIdDesc = "", FiberSegmentIdx = "0" };
            SelectedCable3 = new AFiberCable { CableIdDesc = "", FiberSegmentIdx = "0" };
            SelectedCable4 = new AFiberCable { CableIdDesc = "", FiberSegmentIdx = "0" };
            SelectedDuct = new ConduitsGroup();
            SelectedTagNum = new ConduitsGroup();*/

        }

        private void ExecuteRemoveCable1Command()
        {
            //SelectedCable1.CableIdDesc = "";
            //SelectedCable1.CableType = "";
            //SelectedCable1.AFRKey = "0";
            SelectedCable1 = null;
            Console.WriteLine();
            SheathMark1 = "";
        }

        private void ExecuteRemoveCable2Command()
        {
            //SelectedCable2.CableIdDesc = ""; // = new AFiberCable { CableIdDesc = "", FiberSegmentIdx = "0" };
            //SelectedCable2.CableType = "";
            //SelectedCable2.AFRKey = "0";
            SelectedCable2 = null;
            Console.WriteLine();
            SheathMark2 = "";
        }

        private void ExecuteRemoveCable3Command()
        {
            //SelectedCable3.CableIdDesc = ""; // = new AFiberCable { CableIdDesc = "", FiberSegmentIdx = "0" };
            //SelectedCable3.CableType = "";
            //SelectedCable3.AFRKey = "0";
            Console.WriteLine();
            SelectedCable3 = null;
            SheathMark3 = "";
        }

        private void ExecuteRemoveCable4Command()
        {
            //SelectedCable4.CableIdDesc = ""; // = new AFiberCable { CableIdDesc = "", FiberSegmentIdx = "0" };
            //SelectedCable4.CableType = "";
            //SelectedCable4.AFRKey = "0";
            SelectedCable4 = null;
            Console.WriteLine();
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
                new KeyValuePair<string, string>("from_site_duct", SelectedDuct?.ConduitKey is null ?"0" : SelectedDuct.ConduitKey  ),  // 2
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

            if (string.IsNullOrEmpty(SelectedCable1?.CableIdDesc) 
                && string.IsNullOrEmpty(SelectedCable2?.CableIdDesc)
                && string.IsNullOrEmpty(SelectedCable3?.CableIdDesc)
                && string.IsNullOrEmpty(SelectedCable4?.CableIdDesc)
                )
            {
                await Application.Current.MainPage.DisplayAlert("Warning", "Cable 1 or 2 or 3 or 4 shouldn't be empty","OK");
                return;
            }

            //cek tag number or beginning site
            if (string.IsNullOrEmpty(SelectedTagNum.HosTagNumber))
            {
                await Application.Current.MainPage.DisplayAlert("Warning", "Tag number shouldn't be empty", "OK");
                return;
            }

            //cek which duct is empty or not
            if (string.IsNullOrEmpty(SelectedDuct.WhichDucts))
            {
                await Application.Current.MainPage.DisplayAlert("Warning", "Which Duct shouldn't be empty", "OK");
                return;
            }

            // put to key map before convert to json
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
                        if (SelectedCable1 != null)
                        {
                            SelectedCable1.FiberSegmentIdx = contentResponse?.key1 is null ? "0" : contentResponse.key1;
                            Session.Cable1 = SelectedCable1;
                            Console.WriteLine(SelectedCable1.FiberSegmentIdx);
                        }
                        if (SelectedCable2 != null)
                        {
                            SelectedCable2.FiberSegmentIdx = contentResponse?.key2 is null ? "0" : contentResponse.key2;
                        Session.Cable2 = SelectedCable2;
                        Console.WriteLine(SelectedCable2.FiberSegmentIdx);
                        }
                        if (SelectedCable3 != null)
                        {
                            SelectedCable3.FiberSegmentIdx = contentResponse?.key3 is null ? "0" : contentResponse.key3;
                        Session.Cable3= SelectedCable3;
                        Console.WriteLine(SelectedCable3.FiberSegmentIdx);
                        }
                        if (SelectedCable4 != null)
                        {
                            SelectedCable4.FiberSegmentIdx = contentResponse?.key4 is null ? "0" : contentResponse.key4;
                        Session.Cable4 = SelectedCable4;
                        Console.WriteLine(SelectedCable4.FiberSegmentIdx);
                        }

                        Session.GpsPointMaxIdx = contentResponse?.locatepointkey is null ? "0" : contentResponse.locatepointkey;



                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }



            await Application.Current.MainPage.Navigation.PushAsync(new LocatePointPage());
        }                                                                   
    }
}
