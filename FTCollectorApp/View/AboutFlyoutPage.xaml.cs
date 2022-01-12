using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.Page
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutFlyoutPage : FlyoutPage
    {
        public AboutFlyoutPage()
        {
            InitializeComponent();
            FlyoutPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as AboutFlyoutPageFlyoutMenuItem;
            if (item == null)
                return;
            if(item.Title.Equals("Find Me"))
                Detail = new NavigationPage(new MapPage());
            else if (item.Title.Equals("Previous"))
                Navigation.PopAsync();
            else if (item.Title.Equals("Log Out"))
                Navigation.PopModalAsync();
            /*var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;

            Detail = new NavigationPage(page);*/
            IsPresented = false;

            FlyoutPage.ListView.SelectedItem = null;
        }
    }
}