using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using FTCollectorApp.Model;
using SQLite;
namespace FTCollectorApp.ViewModel
{
    public class SelectCrewViewModel : BaseViewModel
    {

        public ObservableCollection<CrewRole> CrewRoles
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CrewRole>();
                    var table = conn.Table<CrewRole>().ToList();
                    return new ObservableCollection<CrewRole>(table);
                }
            }
        }


    }
}
