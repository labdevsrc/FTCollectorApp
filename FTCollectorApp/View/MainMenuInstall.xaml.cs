using FTCollectorApp.View.CablePages;
using FTCollectorApp.View.FiberPages;
using FTCollectorApp.View.SitesPage;
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
    public partial class MainMenuInstall : ContentPage
    {
        public MainMenuInstall()
        {
            InitializeComponent();
        }

        private void btnSite_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SiteInputPage());
        }


        private void gotoFOCablePage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FiberOpticCablePage());
        }

        private void btnPullCable_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PullCablePage());
        }

        private void btnSpliceCable_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SpliceFiberPage());
        }

        private void btnTerminate_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TerminateFiber());
        }

        private void btnInstallDev_Clicked(object sender, EventArgs e)
        {

        }

        private void OTDR_Clicked(object sender, EventArgs e)
        {

        }
    }
}