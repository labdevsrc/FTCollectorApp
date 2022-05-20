using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using SQLite;

namespace FTCollectorApp.ViewModel
{
    public partial class PortConnectionViewModel : ObservableObject
    {

        public ObservableCollection<RackNumber> RackRailShelfs
        {
            get
            {

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<RackNumber>();
                    Console.WriteLine();
                    var table = conn.Table<RackNumber>().Where(a => a.SiteId == Session.tag_number).ToList();
                    Console.WriteLine();
                    return new ObservableCollection<RackNumber>(table);
                }

            }
        }

        public ObservableCollection<Chassis> FromChassisList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    Console.WriteLine();
                    conn.CreateTable<Chassis>();
                    var table = conn.Table<Chassis>().Where(a => a.TagNumber == Session.tag_number).ToList();
                    foreach (var col in table)
                    {
                        //if(col?.Model == null)
                        if (string.IsNullOrEmpty(col?.Model))
                        {
                            col.Model = "Unknwon"; // because DisplayBindingItem ={Binding Model}, make sure no Model = null 
                            Console.WriteLine();
                        }
                    }

                    try
                    {

                        if (SelectedFromRack != null)
                        {
                            table = conn.Table<Chassis>().Where(a => (a.TagNumber == Session.tag_number) && (a.rack_number == SelectedFromRack.Racknumber)).ToList();
                            Console.WriteLine();
                        }
                        Console.WriteLine();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    return new ObservableCollection<Chassis>(table);
                }
            }
        }


        public ObservableCollection<Chassis> ToChassisList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    Console.WriteLine();
                    conn.CreateTable<Chassis>();
                    var table = conn.Table<Chassis>().Where(a => a.TagNumber == Session.tag_number).ToList();
                    foreach (var col in table)
                    {
                        //if(col?.Model == null)
                        if (string.IsNullOrEmpty(col?.Model))
                        {
                            col.Model = "Unknwon"; // because DisplayBindingItem ={Binding Model}, make sure no Model = null 
                            Console.WriteLine();
                        }
                    }

                    try
                    {

                        if (SelectedFromRack != null)
                        {
                            table = conn.Table<Chassis>().Where(a => (a.TagNumber == Session.tag_number) && (a.rack_number == SelectedFromRack.Racknumber)).ToList();
                            Console.WriteLine();
                        }
                        Console.WriteLine();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    return new ObservableCollection<Chassis>(table);
                }
            }
        }
    }
}
