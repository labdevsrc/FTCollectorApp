using FTCollectorApp.Model.Reference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using SQLite;
using Xamarin.Forms;
using System.Threading.Tasks;
using FTCollectorApp.View.SitesPage;

namespace FTCollectorApp.ViewModel
{
    public class DuctViewModel : BaseViewModel
    {
        public DuctViewModel()
        {

            // from Duct Color Selection PopUp

            ColorSelectedCommand = new Command(ductcolor => ExecuteColorSelectedCommand(ductcolor as ColorCode));

            // from DuctPage
            ShowPopupCommand = new Command(async _ => await ExecuteShowPopupCommand());

        }



        /// get selected color from popup - start
        private ColorCode _selectedColor;
        public ColorCode SelectedColor
        {
            get => _selectedColor;
            set => SetProperty(ref _selectedColor, value);
        }

        public ICommand ShowPopupCommand { get; }
        public ICommand ColorSelectedCommand { get; }
        private Task ExecuteShowPopupCommand()
        {
            var popup = new DuctColorCodePopUp(SelectedColor)
            {
                ColorSelectedCommand = ColorSelectedCommand
            };
            return Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
        }
        // with Mode=TwoWay, no need this ?
        private void ExecuteColorSelectedCommand(ColorCode ductcolor)
        {
            SelectedColor = ductcolor;
            Console.WriteLine();
        }



        /// get selected color from popup - end
        /// 

        // Duct Page - start
        public ObservableCollection<DuctType> DuctMaterialList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<DuctType>();
                    var table = conn.Table<DuctType>().ToList();
                    return new ObservableCollection<DuctType>(table);
                }
            }
        }
        public ObservableCollection<DuctUsed> DuctUsageList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<DuctUsed>();
                    var table = conn.Table<DuctUsed>().ToList();
                    return new ObservableCollection<DuctUsed>(table);
                }
            }
        }

        public ObservableCollection<DuctSize> DuctSizeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<DuctSize>();
                    var table = conn.Table<DuctSize>().ToList();
                    return new ObservableCollection<DuctSize>(table);
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
        // Duct Page - end

        public ObservableCollection<CompassDirection> TravelDirectionList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CompassDirection>();
                    var data = conn.Table<CompassDirection>().ToList();
                    return new ObservableCollection<CompassDirection>(data);
                }
            }
        }

    }
}
