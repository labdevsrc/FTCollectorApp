using FTCollectorApp.View.TraceFiberPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View.CablePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PullCablePage : ContentPage
    {
        public PullCablePage()
        {
            InitializeComponent();
        }

        private void btnNewFiber_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FiberOpticCablePage());
        }

        private void OnClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}