using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View.TraceFiberPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TracingMenu : ContentPage
    {
        public TracingMenu()
        {
            InitializeComponent();
        }

        private async void btnStart_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DuctTracePage());
        }

        private async void btnResume_Clicked(object sender, EventArgs e)
        {

        }
    }
}