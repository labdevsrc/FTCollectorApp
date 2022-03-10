using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Xamarin.CommunityToolkit.UI.Views;
using System.IO;
using Amazon.S3.Transfer;
using Amazon;
using FTCollectorApp.Model;
using FTCollectorApp.Utils;
using PCLStorage;

namespace FTCollectorApp.View.Utils
{
    public partial class CameraViewPage : BasePage
	{
		// Note: not all options of the CameraView are on this page, make sure to discover them for yourself!
		string fullpathFile;
		public CameraViewPage()
		{
			InitializeComponent();
			BindingContext = this;
			zoomLabel.Text = string.Format("Zoom: {0}", zoomSlider.Value);
		}

		void ZoomSlider_ValueChanged(object? sender, ValueChangedEventArgs e)
		{
			cameraView.Zoom = (float)zoomSlider.Value;
			zoomLabel.Text = string.Format("Zoom: {0}", Math.Round(zoomSlider.Value));
		}

		/*void VideoSwitch_Toggled(object? sender, ToggledEventArgs e)
		{
			var captureVideo = e.Value;

			if (captureVideo)
				cameraView.CaptureMode = CameraCaptureMode.Video;
			else
				cameraView.CaptureMode = CameraCaptureMode.Photo;

			previewPicture.IsVisible = !captureVideo;

			doCameraThings.Text = e.Value ? "Start Recording"
				: "Snap Picture";
		}*/

		// You can also set it to Default and External
		void FrontCameraSwitch_Toggled(object? sender, ToggledEventArgs e)
			=> cameraView.CameraOptions = e.Value ? CameraOptions.Front : CameraOptions.Back;

		// You can also set it to Torch (always on) and Auto
		void FlashSwitch_Toggled(object? sender, ToggledEventArgs e)
			=> cameraView.FlashMode = e.Value ? CameraFlashMode.On : CameraFlashMode.Off;

		void DoCameraThings_Clicked(object? sender, EventArgs e)
		{
			cameraView.Shutter();
			var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
			player.Load("camera.mp3");
			player.Play();
			//doCameraThings.Text = cameraView.CaptureMode == CameraCaptureMode.Video
			//	? "Stop Recording"
			//	: "Snap Picture";
		}

		void CameraView_OnAvailable(object? sender, bool e)
		{
			if (e)
			{
				zoomSlider.Value = cameraView.Zoom;
				var max = cameraView.MaxZoom;
				if (max > zoomSlider.Minimum && max > zoomSlider.Value)
					zoomSlider.Maximum = max;
				else
					zoomSlider.Maximum = zoomSlider.Minimum + 1; // if max == min throws exception
			}

			doCameraThings.IsEnabled = e;
			zoomSlider.IsEnabled = e;
		}

		async void CameraView_MediaCaptured(object? sender, MediaCapturedEventArgs e)
		{
			IsBusy = true;
			var fileTransferUtility = new TransferUtility(Constants.ACCES_KEY_ID, Constants.SECRET_ACCESS_KEY, RegionEndpoint.USEast2);
			try
			{
				var pictnaming = $"{Session.OwnerName}_{Session.lattitude2}_{Session.longitude2}_{DateTime.Now.ToString("yyyy-M-d_HH-mm-ss")}_{Session.ownerkey}.png";
				IFolder rootFolder = FileSystem.Current.LocalStorage;
				IFolder folder = await rootFolder.CreateFolderAsync("images",
					CreationCollisionOption.OpenIfExists);
				IFile file = await folder.CreateFileAsync(pictnaming,
					CreationCollisionOption.ReplaceExisting);
				//await file.WriteAllTextAsync(e.ImageData)
				
				//fullpathFile = Path.Combine(App.ImageFileLocation, pictnaming);
				Console.WriteLine($"Start capture stream from {file.Path.ToString()}");
				//using (FileStream fs = new FileStream(folder.Path, FileMode.Create, FileAccess.Write))
				using(var fs = await file.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
				{
					await fs.WriteAsync(e.ImageData, 0, e.ImageData.Length);
				}

				await fileTransferUtility.UploadAsync(file.Path.ToString(), Constants.BUCKET_NAME);
			}
			catch (Exception exp)
			{
				Console.WriteLine($"Exception {exp.ToString()}");

			}


			IsBusy = false;

		}

        private void btnCamera(object sender, EventArgs e)
        {

        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {

        }

    }
}