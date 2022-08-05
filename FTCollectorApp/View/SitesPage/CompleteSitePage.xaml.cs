using FTCollectorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FTCollectorApp.Model;
using PCLStorage;
using Xamarin.CommunityToolkit.UI.Views;

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompleteSitePage : ContentPage
    {
        public CompleteSitePage()
        {
            InitializeComponent();
            BindingContext = this;
            crewleader.Text = Session.crew_leader;
        }


        private async void SaveBtn_Clicked(object sender, EventArgs e)
        {

            IsBusy = true;

            /*try
            {
                Console.WriteLine($"Start capture stream from {App.SignatureFileLocation}");
                Stream image = await signaturePad.GetImageStreamAsync(SignaturePad.Forms.SignatureImageFormat.Png);

                using (FileStream fs = new FileStream(App.SignatureFileLocation, FileMode.Create, FileAccess.Write))
                {
                    image.CopyTo(fs);

                }
            }
            catch (Exception exp)
            {
                Console.WriteLine($"Exception {exp.ToString()}");

            }*/


            ///////////////////////// S3 Bucket  Upload ////////////////////////////

            var fileTransferUtility = new TransferUtility(Constants.ACCES_KEY_ID, Constants.SECRET_ACCESS_KEY, RegionEndpoint.USEast2);

            //$fname=$owner."_".$tag."_".$user."_".$page."_".$longitude."_".$lattitude."_".date('Y-m-d-H-i-s').".png";
            try
            {
                Console.WriteLine($"Start capture stream from {App.SignatureFileLocation}");
                Stream image = await signaturePad.GetImageStreamAsync(SignaturePad.Forms.SignatureImageFormat.Png);

                var pictnaming = $"{Session.OwnerName}_{Session.tag_number}_signature_{Session.live_longitude}_{Session.live_lattitude}_{DateTime.Now.ToString("yyyy-M-d_HH-mm-ss")}_{Session.ownerkey}.png";
                IFolder rootFolder = FileSystem.Current.LocalStorage;
                IFolder folder = await rootFolder.CreateFolderAsync("images",
                    CreationCollisionOption.OpenIfExists);
                IFile file = await folder.CreateFileAsync(pictnaming,
                    CreationCollisionOption.ReplaceExisting);

                using (var fs = await file.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
                {
                    //await fs.WriteAsync(e.ImageData, 0, e.ImageData.Length);
                    image.CopyTo(fs);
                }

                await fileTransferUtility.UploadAsync(file.Path.ToString(), Constants.BUCKET_NAME);
                //await fileTransferUtility.UploadAsync(App.SignatureFileLocation, Constants.BUCKET_NAME);
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