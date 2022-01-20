using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EquipmenReturnPage : ContentPage
    {
        public EquipmenReturnPage()
        {
            InitializeComponent();
        }

        private async void SaveBtn_Clicked(object sender, EventArgs e)
        {
            Stream image = await signaturePad.GetImageStreamAsync(SignaturePad.Forms.SignatureImageFormat.Jpeg);
        }

        private void ClearBtn_Clicked(object sender, EventArgs e)
        {
            signaturePad.Clear();
        }
    }
}