using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasicAllert 
    {
        public BasicAllert(string strMessage, string strTitle)
        {
            messageTxt = strMessage;
            titlePopUp = strTitle;
            InitializeComponent();
            BindingContext = this;
        }

        public BasicAllert(string strMessage, string strTitle, bool OK )
        {
            messageTxt = strMessage;
            titlePopUp = strTitle;
            InitializeComponent();
            BindingContext = this;
            SelectedOK = OK;

        }

        bool selectedOK = false;
        public bool SelectedOK
        {
            get => selectedOK;
            set
            {
                if (selectedOK == value) return;
                selectedOK = value;
                OnPropertyChanged(nameof(SelectedOK));
                Console.WriteLine();
            }
        }


        string messageTxt;
        public string MessageTxt
        {
            get => messageTxt;
            set
            {
                if (messageTxt == value)
                    return;
                messageTxt = value;
                OnPropertyChanged(nameof(MessageTxt));
            }
        }


        string titlePopUp;
        public string TitlePopUp
        {
            get => titlePopUp;
            set
            {
                if (titlePopUp == value)
                    return;
                titlePopUp = value;
                OnPropertyChanged(nameof(TitlePopUp));
            }
        }

        private async void OnOK(object sender, EventArgs e)
        {
            SelectedOK = true;
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}