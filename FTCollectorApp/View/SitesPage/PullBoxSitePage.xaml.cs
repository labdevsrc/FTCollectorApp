using FTCollectorApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PullBoxSitePage : ContentPage
    {
        string MajorMinorType;
        string TagNumber;
        public PullBoxSitePage(string majorType, string minorType, string tagNumber)
        {
            InitializeComponent();
            MajorMinorType = $"{majorType} - {minorType}";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            entrySiteType.Text = MajorMinorType;
            entryTagNum.Text = TagNumber;
            ownerName.Text = Session.OwnerName;
        }

        private void btnCamera(object sender, EventArgs e)
        {

        }
    }
}