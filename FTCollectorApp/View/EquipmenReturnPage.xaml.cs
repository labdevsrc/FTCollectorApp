using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
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

            IsBusy = true;
            try
            {
                Console.WriteLine($"Start capture stream from {App.SignatureFileLocation}");
                Stream image = await signaturePad.GetImageStreamAsync( SignaturePad.Forms.SignatureImageFormat.Png);
                using (FileStream fs = new FileStream(App.SignatureFileLocation, FileMode.Create, FileAccess.Write))
                {
                    image.CopyTo(fs);
                }
            }
            catch(Exception exp)
            {
                Console.WriteLine($"Exception {exp.ToString()}");

            }

            var fileTransferUtility = new TransferUtility(Constants.ACCES_KEY_ID, Constants.SECRET_ACCESS_KEY, RegionEndpoint.USEast2);
            try
            {
                await fileTransferUtility.UploadAsync(App.SignatureFileLocation, Constants.BUCKET_NAME);
            }
            catch (Exception exp)
            {
                Console.WriteLine($"Exception {exp.ToString()}");
            }

            IsBusy = false;
        }

        private void ClearBtn_Clicked(object sender, EventArgs e)
        {
            signaturePad.Clear();
        }
    }
}