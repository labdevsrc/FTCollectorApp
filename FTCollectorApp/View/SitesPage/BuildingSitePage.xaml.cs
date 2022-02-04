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

            pickerDotDisctrict.ItemsSource = DotDistrict;
            pickerElectSiteKey.ItemsSource = DotDistrict;
            pickerHasPowerDisconnect.ItemsSource = YesNo;
            picker3rdpComms.ItemsSource = YesNo;
            pickerLaneClosure.ItemsSource = YesNo;
            buildingClass.ItemsSource = BuildingClassification();
            pIntersection.ItemsSource = Roadway();
            pRoadway.ItemsSource = Roadway();

            pDirTravel.ItemsSource = DirectionTravel();
            pOrientation.ItemsSource = DirectionTravel();


            pMaterial.ItemsSource = Material();
            pMounting.ItemsSource = Mounting();
            pFilterType.ItemsSource = FilterType();
            pFilterSize.ItemsSource = FilterSize();


            pHaveSunShield.ItemsSource = YesNo;
            pHasGround.ItemsSource = YesNo;
            pHasKey.ItemsSource = YesNo;
            pKeyType.ItemsSource = DotDistrict;
            pDirectionTravel.ItemsSource = DotDistrict;
            pIsSiteClearZone.ItemsSource = YesNo;
            pBucketTruck.ItemsSource = YesNo;



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
            roadWay.Add("SW Tradition Pkwy");
            return roadWay;
        }

        List<string> DirectionTravel()
        {
            List<string> dirTravel = new List<string>();
            dirTravel.Add("North");
            dirTravel.Add("North East");
            dirTravel.Add("East");
            dirTravel.Add("South East");
            dirTravel.Add("South");
            dirTravel.Add("South West");
            dirTravel.Add("West");
            dirTravel.Add("North West");
            dirTravel.Add("Median");
            dirTravel.Add("On Ramp");
            dirTravel.Add("Unknown");

            return dirTravel;
        }

        List<string> Material()
        {
            List<string> material = new List<string>();
            material.Add("Composite");
            material.Add("Concrete");
            material.Add("Fiberglass");
            material.Add("Galvanized Steel");
            material.Add("Plastic");
            material.Add("Cast Iron");
            material.Add("Polymer");
            material.Add("Metal");
            material.Add("CONC_W/METAL_LID");
            material.Add("Steel");
            material.Add("Aluminium");

            return material;
        }

        List<string> Mounting()
        {
            List<string> mounting = new List<string>();
            mounting.Add("Butterfly");
            mounting.Add("In Pavement");
            mounting.Add("Under Pavement");
            mounting.Add("Bridge");
            mounting.Add("Cantilever");
            mounting.Add("Concrete Pad");
            mounting.Add("Overhead Span");
            mounting.Add("Pier");
            mounting.Add("Pole");
            mounting.Add("Strut");
            mounting.Add("Wall");
            mounting.Add("Inside of Cabinet");
            mounting.Add("Outside of Cabinet");
            mounting.Add("Unistrut");
            mounting.Add("Building");
            mounting.Add("Pedistal");
            mounting.Add("Messenger Wire");
            mounting.Add("Mast Arm");
            mounting.Add("Street Light");
            mounting.Add("In Ground");

            return mounting;
        }

        List<string> FilterType()
        {
            List<string> filterType = new List<string>();
            filterType.Add("Fabric");
            filterType.Add("Metal");
            filterType.Add("Paper");
            filterType.Add("Fiberglass");
            filterType.Add("Sponge");
            filterType.Add("NONE");
            filterType.Add("Unknown");

            return filterType;
        }

        List<string> FilterSize()
        {
            List<string> filterSize = new List<string>();
            filterSize.Add("12\" x 12\" x 1\"");
            filterSize.Add("12\" x 14\" x 1\"");
            filterSize.Add("12\" x 16\" x 1\"");
            filterSize.Add("12\" x 18\" x 1\"");
            filterSize.Add("12\" x 25\" x 1\"");

            return filterSize;
        }
    }
}