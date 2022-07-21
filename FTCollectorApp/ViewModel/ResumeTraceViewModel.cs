using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FTCollectorApp.Model.Reference;
using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using FTCollectorApp.Service;

namespace FTCollectorApp.ViewModel
{
    public partial class ResumeTraceViewModel: ObservableObject
    {
        [ObservableProperty]
        ConduitsGroup selectedTagNum;


        string from_duct_tag;
        string from_duct_key;
        public ObservableCollection<SuspendedTrace> SspTraceList;
        public ResumeTraceViewModel()
        {

            /*if (Application.Current.Properties.ContainsKey(Constants.SavedFromDuctTagNumber))
            {
                from_duct_tag = (string) Application.Current.Properties[Constants.SavedFromDuctTagNumber];
            }

            if (Application.Current.Properties.ContainsKey(Constants.SavedFromDuctTagNumberKey))
            {
                from_duct_key = (string) Application.Current.Properties[Constants.SavedFromDuctTagNumberKey];
            }*/


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


        [ICommand]
        async void Resume()
        {

        }

        [ICommand]
        async void ViewSuspended()
        {
            var suspList = await CloudDBService.GetSuspendedTrace(); //gps_point

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<SuspendedTrace>();
                conn.DeleteAll<SuspendedTrace>();
                conn.InsertAll(suspList);
                var table = conn.Table<SuspendedTrace>().ToList();
                SspTraceList = new ObservableCollection<SuspendedTrace>(table);
            }

        }
    }
}
