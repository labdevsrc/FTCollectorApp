using FTCollectorApp.Model;
using FTCollectorApp.View.Utils;
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
    public partial class BuildingSitePage : ContentPage
    {
        string MajorMinorType;
        string TagNumber;
        List<string> DotDistrict = new List<string>();
        List<string> YesNo = new List<string>();
        public BuildingSitePage(string majorType, string minorType, string tagNumber)
        {
            InitializeComponent();
            MajorMinorType = $"{majorType} - {minorType}";

            for (int i = 0; i < 100; i++)
            {
                DotDistrict.Add(i.ToString());
            }

            YesNo.Add("N");
            YesNo.Add("Y");

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            entrySiteType.Text = MajorMinorType;
            entryTagNum.Text = TagNumber;
            ownerName.Text = Session.OwnerName;
            pickerDotDisctrict.ItemsSource = DotDistrict;
            pickerElectSiteKey.ItemsSource = DotDistrict;
            pickerHasPowerDisconnect.ItemsSource = YesNo;
            picker3rdpComms.ItemsSource = YesNo;
            pickerLaneClosure.ItemsSource = YesNo;
            buildingClass.ItemsSource = BuildingClassification();
            pRoadway.ItemsSource = Roadway();

        }

        private void btnCamera(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CameraViewPage());
        }



        List<string> BuildingClassification()
        {
            List<string> buildingClassification = new List<string>();
            buildingClassification.Add("City Facility");
            buildingClassification.Add("County Facility");
            buildingClassification.Add("Other Agency Facility");
            buildingClassification.Add("Private Partner Facility");
            buildingClassification.Add("Shelter – Aboveground");
            buildingClassification.Add("Shelter – Underground");
            buildingClassification.Add("Utility Company Facility");

            return buildingClassification;
        }

        List<string> Roadway()
        {
            List<string> roadWay = new List<string>();
            roadWay.Add("Crosstown Parkway");
            roadWay.Add("US 1");
            roadWay.Add("SE Floesta Drive");
            roadWay.Add("SE Sandia Drive");
            roadWay.Add("SE Manth Lane");
            roadWay.Add("I95");
            roadWay.Add("SW Rosser Blvd");
            roadWay.Add("SW Gatlin  Blvd");
            roadWay.Add("SW Savona  Blvd");

            return roadWay;
        }
    }
}