using FTCollectorApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;


namespace FTCollectorApp.View.FiberPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TerminateFiber : ContentPage
    {
        List<string> SiteType = new List<string>();


        public ObservableCollection<Site> Sites
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Site>();
                    var tableSite = conn.Table<Site>().Where(g => g.SiteTypeDesc == selectedSiteType).ToList();
                    return new ObservableCollection<Site>(tableSite);
                }
            }
        }


        public TerminateFiber()
        {
            InitializeComponent();
            BindingContext = this;

            SiteType.Add("--Select--");
            SiteType.Add("Building");
            SiteType.Add("Cabinet");
            SiteType.Add("Structure");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            pSiteType.ItemsSource = SiteType;

        }




        private void OnClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        string selectedSiteType;
        private void OnIndexChanged(object sender, EventArgs e)
        {
            if(pSiteType.SelectedIndex > 0)
            {
                selectedSiteType = pSiteType.SelectedItem.ToString();
                pTagNumber.ItemsSource = Sites;
            }

            if (pTagNumber.SelectedIndex != -1)
            {

            }
        }
    }
}