using FTCollectorApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PendingSendPage : ContentPage
    {

        public PendingSendPage()
        {
            InitializeComponent();
            
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            listviewPost.ItemsSource = CloudDBService.listPendingTask;
            base.OnAppearing();
        }

        private async void syncWorkItem_Clicked(object sender, EventArgs e)
        {

        }

        private async void listviewPost_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedTask = listviewPost.SelectedItem as string;
            await CloudDBService.PostPendingTask(selectedTask);
        }

        private void delWorkItem_Clicked(object sender, EventArgs e)
        {

        }
    }
}