﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Collections.ObjectModel;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Model;
using System.Web;
using System.Windows.Input;
using System.Threading.Tasks;
using Xamarin.Forms;
using FTCollectorApp.View.SitesPage;
using FTCollectorApp.Service;

namespace FTCollectorApp.ViewModel
{
    public partial class SlotBladePageViewModel : ObservableObject
    {
        public ICommand SaveCommand { get; set; }
        public ICommand FinishSaveCommand { get; set; }
        public ICommand ShowPortPageCommand { get; set; }
        public ICommand ShowPortConnPageCommand { get; set; }
        public ICommand RefreshBladeKeyListCommand { get; set; }
        public SlotBladePageViewModel()
        {
            SaveCommand = new Command(async () => ExecuteSaveCommand());
            FinishSaveCommand = new Command(() => ExecuteFinishSaveCommand());
            RefreshBladeKeyListCommand = new Command(() => ExecuteRefreshBladeKeyListCommand());
            ShowPortPageCommand = new Command(async () => ExecuteShowPortPageCommand());
            ShowPortConnPageCommand = new Command(async () => ExecuteShowPortConnPageCommand());
            Session.tag_number = "65901";
        }

        private void ExecuteRefreshBladeKeyListCommand()
        {
            AddtoSQLite();
        }

        private async Task ExecuteSaveCommand()
        {
            var KVPair = keyvaluepair();
            var result = await CloudDBService.PostBladeSave(KVPair);

            if (result.Equals("OK"))
            {
                Console.WriteLine();
            }
        }

        public void AddtoSQLite()
        {

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                var KVPair = keyvaluepair();
                conn.CreateTable<SlotBladeTray>();
                conn.Insert(KVPair);
                OnPropertyChanged(nameof(SlotBladeTrayTables));
                /*SQLiteCommand insertSQL = new SQLiteCommand("INSERT INTO SlotBladeTray (key, owner_key, rack_key, chassis_key, OWNER_CD, " +
                "site, slot_or_blade_number, front_back, orientation) VALUES (?,?,?,?,?,?,?,?,?)", conn);
                insertSQL.Parameters.Add(slotbtray.key);
                insertSQL.Parameters.Add(slotbtray.owner_key);
                insertSQL.Parameters.Add(slotbtray.rack_key);
                insertSQL.Parameters.Add(slotbtray.chassis_key);
                insertSQL.Parameters.Add(slotbtray.OWNER_CD);
                insertSQL.Parameters.Add(slotbtray.site);
                insertSQL.Parameters.Add(slotbtray.slot_or_blade_number);
                insertSQL.Parameters.Add(slotbtray.front_back);
                insertSQL.Parameters.Add(slotbtray.orientation);

                try
                {
                    insertSQL.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }*/
            }
        }

        private async Task ExecuteFinishSaveCommand()
        {
            var KVPair = keyvaluepair();
            var result = await CloudDBService.PostBladeSave(KVPair);
            if (result.Equals("OK"))
            {
                Console.WriteLine();
                Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        private async Task ExecuteShowPortPageCommand()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new PortPage());
        }

        private async Task ExecuteShowPortConnPageCommand()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new PortConnection());
        }

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(ModelDetailList))]
        Manufacturer selectedManufacturer;

        [ObservableProperty]
        ModelDetail selectedModelDetail;


        [ObservableProperty]
        SlotBladeTray sessionSlotBladTray;

        [ObservableProperty]
        SlotBladeTray selectedBladSlotTray;


        [ObservableProperty]
        string textField;

        [ObservableProperty]
        string selectedOrientation;

        [ObservableProperty]
        string selectedPorts;

        [ObservableProperty]
        string selectedBlade;

        List<KeyValuePair<string, string>> keyvaluepair()
        {
            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), // 
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // 1
                new KeyValuePair<string, string>("tag", Session.tag_number),  // 2
                new KeyValuePair<string, string>("direction", SelectedBladSlotTray?.orientation  == null ? "0": SelectedBladSlotTray.orientation),  // 3
                new KeyValuePair<string, string>("orientation", SelectedOrientation ??= "0"),  // 4
                new KeyValuePair<string, string>("slot_or_blade_number", SelectedOrientation ??= "0"),  // 4
                new KeyValuePair<string, string>("manufacturer_key",  SelectedManufacturer?.ManufKey == null ? "0": selectedManufacturer.ManufKey),  // 5
                new KeyValuePair<string, string>("model_key", SelectedModelDetail?.ModelKey == null ? "0": SelectedModelDetail.ModelKey),  // 6
                new KeyValuePair<string, string>("manufacturer",  SelectedManufacturer?.ManufName == null ? "0": selectedManufacturer.ManufName),  // 7
                new KeyValuePair<string, string>("model", SelectedModelDetail?.ModelDescription == null ? "0": SelectedModelDetail.ModelDescription),  // 8
                new KeyValuePair<string, string>("textField", textField ??= "0"),  // 9
                new KeyValuePair<string, string>("bnumber", SelectedBlade ??= "0"),
                new KeyValuePair<string, string>("stage", Session.stage)
            };


            return keyValues;

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
        public ObservableCollection<SlotBladeTray> SlotBladeTrayTables
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<SlotBladeTray>();
                    var table = conn.Table<SlotBladeTray>().Where(a => a.site == Session.tag_number).ToList();
                    Console.WriteLine();
                    return new ObservableCollection<SlotBladeTray>(table);
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

        public ObservableCollection<Chassis> ChassisList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Chassis>();
                    var table = conn.Table<Chassis>().Where(a => a.TagNumber == Session.tag_number).ToList();
                    Console.WriteLine();
                    return new ObservableCollection<Chassis>(table);
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
                    var table = conn.Table<ModelDetail>().ToList();
                    if (SelectedManufacturer?.ManufKey != null)
                        table = conn.Table<ModelDetail>().Where(a => a.ManufKey == SelectedManufacturer.ManufKey).ToList();

                    foreach (var col in table)
                    {
                        col.ModelNumber = HttpUtility.HtmlDecode(col.ModelNumber); // should use for escape char 
                        if (col.ModelCode1 == "") // sometimes this model entri is null
                            col.ModelCode1 = col.ModelCode2;
                        if (col.ModelCode2 == "")
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
        }



    }
}
