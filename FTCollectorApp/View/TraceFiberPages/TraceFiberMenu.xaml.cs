using FTCollectorApp.View.TraceFiberPages;
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
    public partial class TraceFiberMenu : ContentPage
    {
        public TraceFiberMenu()
        {
            InitializeComponent();
        }

        private void btnSelect_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FiberOpticCablePage());
        }

        private void btnStartTrace_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TracingMenu());
        }
    }
}