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
        public BasicAllert(string strMessage, string strTitle )
        {
            InitializeComponent();
            BindingContext = this;
            messageTxt = strMessage;
            titlePopUp = strTitle;
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

        private void OnOK(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
    }
}