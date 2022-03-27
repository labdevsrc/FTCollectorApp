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
using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using System.Web;
using FTCollectorApp.View.Utils;
using FTCollectorApp.Service;

namespace FTCollectorApp.View.TraceFiberPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DuctTracePage : ContentPage
    {
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
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ConduitsGroup>();
                    var table = conn.Table<ConduitsGroup>().GroupBy(b => b.HosTagNumber).Select(g => g.First()).ToList();
                    Console.WriteLine("BeginningSite");
                    return new ObservableCollection<ConduitsGroup>(table);
                }
            }
        }

        public ObservableCollection<ConduitsGroup> DuctConduitDatas
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ConduitsGroup>();
                    var table = conn.Table<ConduitsGroup>().Where(b => b.HosTagNumber == selectedTagNum).ToList();
                    foreach (var col in table)
                    {
                        col.DuctSize = HttpUtility.HtmlDecode(col.DuctSize);
                        col.WhichDucts = col.Direction + " " + col.DirCnt;
                    }
                    Console.WriteLine("WhichDuctLists ");
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

        public DuctTracePage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        int selectedDuctInstall, selectedUOM;
        string selectedTagNum, selectedDirection;

        private void btnCamera(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CameraViewPage());
        }


        private void OnIndexChanged(object sender, EventArgs e)
        {

            if(pWDuct.SelectedIndex != -1)
            {
                if(pTagNumber.SelectedIndex == -1)
                {
                    DisplayAlert("Warning", "Select Beginning TagNumber First", "OK");
                    return;
                }
                var selected = pWDuct.SelectedItem as ConduitsGroup;
                selectedDuctInstall = pWDuct.SelectedIndex;
                txtUsage.Text = selected.DuctUsage == "1" ? "Yes" : "No";
                txtColor.Text = "██████████";
                txtColor.TextColor = Color.SlateGray;
                txtSize.Text = selected.DuctSize;
                Console.WriteLine("");
            }

            if (pCableId.SelectedIndex != -1)
            {
                var selected = pCableId.SelectedItem as AFiberCable;
                txtSMCount.Text = selected.SMCount;
                txtMMCount.Text = selected.MMCount;
            }

            if(pUOM.SelectedIndex != -1)
                selectedUOM = pUOM.SelectedIndex;
                

        }

        private void OnBeginSiteIndexChanged(object sender, EventArgs e)
        {
            if (pTagNumber.SelectedIndex != -1)
            {
                var selected = pTagNumber.SelectedItem as ConduitsGroup;
                selectedTagNum = selected.HosTagNumber;
                selectedDirection = selected.Direction;
                txtSiteType.Text = selected.HostType;
                pWDuct.ItemsSource = DuctConduitDatas; // this picker only populate after tag_number selected
                Console.WriteLine("");
            }
        }

        List<KeyValuePair<string, string>> keyvaluepair()
        {
            var keyValues = new List<KeyValuePair<string, string>>{
                //new KeyValuePair<string, string>("uid", Session.uid.ToString()), 
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // 1
                new KeyValuePair<string, string>("host_tag_number", selectedTagNum),  // 2


                new KeyValuePair<string, string>("accuracy", Session.accuracy), //3
                new KeyValuePair<string, string>("altitude", Session.altitude),  //4
                new KeyValuePair<string, string>("oid", Session.ownerkey), //1
                //new KeyValuePair<string, string>("owner", Session.ownerkey), //5
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), // 6
                new KeyValuePair<string, string>("jobnum",Session.jobnum), //  7 

                /*new KeyValuePair<string, string>("tag",TagNumber), //8
                new KeyValuePair<string, string>("site2", entrySiteName.Text),  /// site_id
                new KeyValuePair<string, string>("type2", SiteType),  /// code_site_type.key
                new KeyValuePair<string, string>("sitname2", entrySiteName.Text),


                new KeyValuePair<string, string>("manufacturer", ""),  // manufacturer , for Cabinet, pull box
                new KeyValuePair<string, string>("manufactured_date", Manufactured),
                new KeyValuePair<string, string>("model", ""), /// model name, Building : x,  Cabinet/Pull Box : o
                new KeyValuePair<string, string>("pic2", ""),
                new KeyValuePair<string, string>("otag", ""),
                new KeyValuePair<string, string>("roadway", RoadwaySelected),
                new KeyValuePair<string, string>("pid", ""),
                new KeyValuePair<string, string>("loct", ""),
                new KeyValuePair<string, string>("staddr", entryStreetAddr.Text), // site_address
                new KeyValuePair<string, string>("pscode", entryPostalCode.Text),

                new KeyValuePair<string, string>("btype", buildingClassiKeySelected),
                new KeyValuePair<string, string>("orientation", OrientationSelected),

                new KeyValuePair<string, string>("laneclosure", IsLaneClosure == 1 ? "1":"0"),
                new KeyValuePair<string, string>("dotdis",  DotDistrictCnt.ToString()),
                new KeyValuePair<string, string>("powr", IsHasPowerDisconnect == 1 ? "1":"0"),
                new KeyValuePair<string, string>("elecsite", ElectSiteKeyCnt.ToString()),
                new KeyValuePair<string, string>("comm", Is3rdComms == 1 ? "1":"0"),
                new KeyValuePair<string, string>("commprovider", commsProvide.Text),
                new KeyValuePair<string, string>("sitaddr", entryStreetAddr.Text), // site_street_addres
                new KeyValuePair<string, string>("udsowner", ""),

                new KeyValuePair<string, string>("rs2", "L"),

                new KeyValuePair<string, string>("height2", entryHeight.Text),
                new KeyValuePair<string, string>("depth2", entryDepth.Text),
                new KeyValuePair<string, string>("width2", entryWidth.Text),
                new KeyValuePair<string, string>("CLEAR_ZONE_IND2", IsSiteClearZone  == 1 ? "1":"0"),

                new KeyValuePair<string, string>("intersect2", IntersectionSelected),
                new KeyValuePair<string, string>("material2", MaterialCodeKeySelected),
                new KeyValuePair<string, string>("mounting2", MountingSelected),
                new KeyValuePair<string, string>("offilter2", FilterTypeSelected),
                new KeyValuePair<string, string>("fltrsize2", FilterSizeKeySelected),
                new KeyValuePair<string, string>("sunshield2", IsHaveSunShield == 1 ? "1":"0"),
                new KeyValuePair<string, string>("installed2", InstalledAt),
                new KeyValuePair<string, string>("comment2", Notes), // Notes, pr description

                new KeyValuePair<string, string>("etc2", ""),
                new KeyValuePair<string, string>("fosc2", ""),
                new KeyValuePair<string, string>("vault2", ""),
                new KeyValuePair<string, string>("trlane2", ""),
                new KeyValuePair<string, string>("bucket2", IsBucketTruck == 1 ? "1":"0"),
                new KeyValuePair<string, string>("serialno", entrySerial.Text),
                new KeyValuePair<string, string>("key", ""),
                new KeyValuePair<string, string>("ktype", KeyTypeSelected.ToString()),
                new KeyValuePair<string, string>("ground", IsHasGround == 1 ? "1":"0"),
                new KeyValuePair<string, string>("traveldir", TravelDirSelected),*/
                new KeyValuePair<string, string>("owner_key", Session.ownerkey),
                new KeyValuePair<string, string>("owner_county", Session.countycode),
                new KeyValuePair<string, string>("jobkey", Session.jobkey),

                new KeyValuePair<string, string>("gps_offset_latitude", ""),
                new KeyValuePair<string, string>("gps_offset_longitude", ""),
                new KeyValuePair<string, string>("LATITUDE", Session.lattitude2),
                new KeyValuePair<string, string>("LONGITUDE", Session.longitude2),


                new KeyValuePair<string, string>("plansheet","0"),
                new KeyValuePair<string, string>("psitem","0"),
                new KeyValuePair<string, string>("stage", Session.stage),
            };


            return keyValues;

        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            var KVPair = keyvaluepair();
            await CloudDBService.PostSaveBuilding(KVPair);
        }
    }
}