using Rg.Plugins.Popup.Services;
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
    public partial class StartTimePage : ContentPage
    {
        public StartTimePage()
        {
            InitializeComponent();
            Button btnCrew1 = new Button { Text = "Start Time for Crew1" };
            Button btnCrew2 = new Button { Text = "Start Time for Crew2" };
            Button btnCrew3 = new Button { Text = "Start Time for Crew3" };

            stackLayout.Children.Add(btnCrew1);
            stackLayout.Children.Add(btnCrew2);
            stackLayout.Children.Add(btnCrew3);

            btnCrew1.Clicked += (s, e) => PopupNavigation.Instance.PushAsync(new StartTimePopupView());
            btnCrew2.Clicked += (s, e) => PopupNavigation.Instance.PushAsync(new StartTimePopupView());
            btnCrew3.Clicked += (s, e) => PopupNavigation.Instance.PushAsync(new StartTimePopupView());

        }

    }
}