using FTCollectorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using System.Collections.ObjectModel;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.View.Utils;

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DuctPage : ContentPage
    {
        List<string> OneToTen = new List<string>();
        List<string> YesNo = new List<string>();
        List<string> SixtyToHundred = new List<string>();


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

        public DuctPage()
        {
            InitializeComponent();
            BindingContext = new DropDownViewModel();

            for (int i = 0; i < 10; i++)
            {
                OneToTen.Add(i.ToString());
            }
            for (int i = 60; i <= 100; i++)
            {
                SixtyToHundred.Add(i.ToString());
            }

            YesNo.Add("N");
            YesNo.Add("Y");

            pDirCnt.ItemsSource = OneToTen;

            isPlugged.ItemsSource = YesNo;
            isOpen.ItemsSource = YesNo;
            hasPullTape.ItemsSource = YesNo;
            hasInnerDuct.ItemsSource = YesNo;
            hasTraceWire.ItemsSource = YesNo;
            percentOpen.ItemsSource = SixtyToHundred;
        }

        private void btnCamera(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CameraViewPage());
        }
    }
}