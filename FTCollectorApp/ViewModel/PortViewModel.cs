using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using FTCollectorApp.Model.Reference;
using SQLite;

namespace FTCollectorApp.ViewModel
{
    public class PortViewModel
    {
        public ObservableCollection<Port> PortKeyList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Port>();
                    var table = conn.Table<Port>().ToList();
                    return new ObservableCollection<Port>(table);
                }
            }
        }
    }
}
