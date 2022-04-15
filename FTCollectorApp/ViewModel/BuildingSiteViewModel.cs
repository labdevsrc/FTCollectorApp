using FTCollectorApp.Model.Reference;
using FTCollectorApp.View.SitesPage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using SQLite;
using FTCollectorApp.Model;

namespace FTCollectorApp.ViewModel
{
    public class BuildingSiteViewModel : BaseViewModel
    {
        public BuildingSiteViewModel()
        {
        }



        public ObservableCollection<BuildingType> BuildingTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<BuildingType>();
                    var bdClassiTable = conn.Table<BuildingType>().ToList();
                    return new ObservableCollection<BuildingType>(bdClassiTable);
                }
            }
        }


        public ObservableCollection<Roadway> RoadwayList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Roadway>();
                    var table = conn.Table<Roadway>().Where(a => a.RoadOwnerKey == Session.ownerkey).ToList();
                    return new ObservableCollection<Roadway>(table);
                }
            }
        }

        public ObservableCollection<Mounting> MountingTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Mounting>();
                    var mountingTable = conn.Table<Mounting>().ToList();
                    return new ObservableCollection<Mounting>(mountingTable);
                }
            }
        }


    }
}
