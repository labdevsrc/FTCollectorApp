using FTCollectorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using System.Collections.ObjectModel;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.View.Utils;
using FTCollectorApp.Model;
using FTCollectorApp.Service;
using Rg.Plugins.Popup.Services;
using System.Windows.Input;

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DuctPage : ContentPage
    {
        //List<string> OneToTen = new List<string>();
        //List<string> YesNo = new List<string>();
        //List<string> SixtyToHundred = new List<string>();

        public ICommand SendResultCommand { get; set; }
        private string _result;
        public string UpdateData
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged(nameof(UpdateData));
            }
        }


        public DuctPage()
        {
            InitializeComponent();
            Console.WriteLine();
            //BindingContext = new DropDownViewModel();
            BindingContext = new DuctViewModel();
            //for (int i = 0; i < 10; i++)
            //{
            //    OneToTen.Add(i.ToString());
            //}
            //for (int i = 100; i >= 80; i--)
            //{
            //    SixtyToHundred.Add(i.ToString());
            //}

            //YesNo.Add("N");
            //YesNo.Add("Y");

            //pDirCnt.ItemsSource = OneToTen;

            //isPlugged.ItemsSource = YesNo;
            //isOpen.ItemsSource = YesNo;
            //hasPullTape.ItemsSource = YesNo;
           // hasInnerDuct.ItemsSource = YesNo;
            //hasTraceWire.ItemsSource = YesNo;
            //percentOpen.ItemsSource = SixtyToHundred;


        }


        private void btnCamera(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CameraViewPage());
        }




        public ColorCode SelectedDuctColor;

        private void OnIndexChanged(object sender, EventArgs e)
        { }

            /*string selectedDuctSize, selectedDirection, selectedDuctMaterial, selectedDuctColor,
                selectedDuctInstall, selectedDuctUsage;



            string IsPlugged, IsOpen, HasPullTape, HasInnerDuct, HasTraceWire;



            int PercentageOpen, selectedDirCnt;

                    List<KeyValuePair<string, string>> keyvaluepair()
            {
                var keyValues = new List<KeyValuePair<string, string>>{
                    new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                    new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), // 
                    new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // 1
                    new KeyValuePair<string, string>("host_tag_number", Session.tag_number),  // 2
                    new KeyValuePair<string, string>("direction", selectedDirection),  // 3
                    new KeyValuePair<string, string>("direction_count", selectedDirCnt.ToString()),  // 4
                    new KeyValuePair<string, string>("duct_size", selectedDuctSize),  // 5
                    new KeyValuePair<string, string>("duct_color", selectedDuctColor),  // 6
                    new KeyValuePair<string, string>("duct_type", selectedDuctMaterial),  // 7
                    new KeyValuePair<string, string>("site_type_key", ""),  // 8
                    new KeyValuePair<string, string>("duct_usage", selectedDuctUsage),  // 9
                    new KeyValuePair<string, string>("install", selectedDuctInstall),  // 10


                    new KeyValuePair<string, string>("openpercent", PercentageOpen.ToString()),  // 11
                    new KeyValuePair<string, string>("duct_trace_wire", HasTraceWire),  // 12
                };


                return keyValues;

            }

            private void OnIndexChanged(object sender, EventArgs e)
            {
                if (pDirection.SelectedIndex != -1)
                {
                    var selected = pDirection.SelectedItem as CompassDirection;
                    selectedDirection = selected.CompasKey;
                }

                if (pDirCnt.SelectedIndex != -1)
                {
                    selectedDirCnt = pDirCnt.SelectedIndex + 1;
                }

                if (pDuctSize.SelectedIndex != -1)
                {
                    var selected = pDuctSize.SelectedItem as DuctSize;
                    selectedDuctSize = selected.DuctKey;
                }


                if (pDuctUsed.SelectedIndex != -1)
                {
                    var selected = pDuctUsed.SelectedItem as DuctUsed;
                    selectedDuctUsage = selected.DuctKey;
                }

                if (pDuctMaterial.SelectedIndex != -1)
                {
                    var selected = pDuctMaterial.SelectedItem as DuctType;
                    selectedDuctMaterial = selected.DucTypeKey;
                }

                if (pInstall.SelectedIndex != -1)
                {
                    var selected = pInstall.SelectedItem as DuctInstallType;
                    selectedDuctInstall = selected.DuctInstallKey;
                }



                if (isPlugged.SelectedIndex != -1)
                    IsPlugged = isPlugged.SelectedIndex.ToString(); // == 0 ? "1" : "0";

                if (isOpen.SelectedIndex != -1)
                    IsOpen = isOpen.SelectedIndex.ToString();

                if (hasPullTape.SelectedIndex != -1)
                    HasPullTape = hasPullTape.SelectedIndex.ToString();

                if (hasInnerDuct.SelectedIndex != -1)
                    HasInnerDuct = hasInnerDuct.SelectedIndex.ToString();

                if (hasTraceWire.SelectedIndex != -1)
                    HasTraceWire = hasTraceWire.SelectedIndex.ToString();

                if (percentOpen.SelectedIndex != -1)
                    PercentageOpen = percentOpen.SelectedIndex + 1;

            }

            private void btnRecInner_Clicked(object sender, EventArgs e)
            {

            }

            private async void btnFinishDRect_Clicked(object sender, EventArgs e)
            {
                var KVPair = keyvaluepair();
                var result = await CloudDBService.PostDuctSave(KVPair);
                if (result.Equals("OK"))
                {
                    SendResultCommand?.Execute("OK");
                }
            }

            private async void btnSaveStart_Clicked(object sender, EventArgs e)
            {
                var KVPair = keyvaluepair();
                var result = await CloudDBService.PostDuctSave(KVPair);
                if (result.Equals("OK"))
                {
                    SendResultCommand?.Execute("OK");
                }
                await Navigation.PopAsync();
            }*/
        }
}