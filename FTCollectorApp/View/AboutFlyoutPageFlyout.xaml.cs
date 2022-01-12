using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.Page
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutFlyoutPageFlyout : ContentPage
    {
        public ListView ListView;

        public AboutFlyoutPageFlyout()
        {
            InitializeComponent();

            BindingContext = new AboutFlyoutPageFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class AboutFlyoutPageFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<AboutFlyoutPageFlyoutMenuItem> MenuItems { get; set; }

            public AboutFlyoutPageFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<AboutFlyoutPageFlyoutMenuItem>(new[]
                {
                    new AboutFlyoutPageFlyoutMenuItem { Id = 0, Title = "Find Me" },
                    new AboutFlyoutPageFlyoutMenuItem { Id = 1, Title = "Last Page" },
                    new AboutFlyoutPageFlyoutMenuItem { Id = 2, Title = "Log Out" }
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}