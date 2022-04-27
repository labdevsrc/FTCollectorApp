using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SQLite;
using FTCollectorApp.Model;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using FTCollectorApp.View.FiberPages;

namespace FTCollectorApp.ViewModel
{
    public partial class TerminateFiberViewModel :  ObservableObject
    {

        public ICommand SaveCommand { get; set; }
        public TerminateFiberViewModel()
        {
            SaveCommand = new Command(() => ExecuteSaveCommand());
        }

        private async void ExecuteSaveCommand()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new FiberTermination());
        }

        string selectedSiteType;
        public string SelectedSiteType
        {
            get => selectedSiteType;
            set
            {
                SetProperty(ref selectedSiteType, value);
                _sites.Where(b => b.SiteTypeDesc == value);
                
                OnPropertyChanged(nameof(Sites));

            }
        }

        [ObservableProperty]
        Site selectedTagNumber;

        ObservableCollection<Site> _sites;

        public ObservableCollection<Site> Sites
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Site>();
                    var table = conn.Table<Site>().ToList();
                    _sites = new ObservableCollection<Site>(table);
                    return _sites;
                }
            }
        }
    }
}
