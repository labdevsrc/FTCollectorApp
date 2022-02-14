using FTCollectorApp.Model;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartTimePage : ContentPage
    {
        public StartTimePage()
        {
            InitializeComponent();
            ArrayList crewnames = new ArrayList();
            crewnames = (ArrayList)Session.sessioncrew;

            int cntCrew = 0;
            
            foreach (var cname in crewnames)
            {
                cntCrew++;

                Button btnCrew1 = new Button { Text = "Start Time for " + cname };
                stackLayout.Children.Add(btnCrew1);
                btnCrew1.Clicked += (s, e) => OnClicked(cntCrew);
            }
            Session.crewCnt = cntCrew;
        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();



        }

        private void OnClicked(int Count)
        {
            PopupNavigation.Instance.PushAsync(new StartTimePopupView(Count));
        }
    }
}