﻿using System;
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
			try
			{
				var pictnaming = $"{Session.OwnerName}_{Session.lattitude2}_{Session.longitude2}_{DateTime.Now.ToString("yyyy-M-d_HH-mm-ss")}_{Session.ownerkey}.png"; 
				Console.WriteLine($"Start capture stream from {App.ImageFileLocation}");
				fullpathFile = Path.Combine(App.ImageFileLocation, pictnaming);
				using (FileStream fs = new FileStream(App.ImageFileLocation, FileMode.Create, FileAccess.Write))
				{
					fs.Write(e.ImageData, 0, e.ImageData.Length);
				}
			}
			catch (Exception exp)
			{
				Console.WriteLine($"Exception {exp.ToString()}");

			}

			var fileTransferUtility = new TransferUtility(Constants.ACCES_KEY_ID, Constants.SECRET_ACCESS_KEY, RegionEndpoint.USEast2);
			try
			{
				await fileTransferUtility.UploadAsync(fullpathFile, Constants.BUCKET_NAME);
			}
			catch (Exception exp)
			{
				Console.WriteLine($"Exception {exp.ToString()}");
			}

			IsBusy = false;

		}
	}
}