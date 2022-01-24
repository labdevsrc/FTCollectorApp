using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Foundation;
using UIKit;

namespace FTCollectorApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {


            global::Xamarin.Forms.Forms.Init();
            // Xamarin maps for iOS
            Xamarin.FormsMaps.Init();
            // SQLite Dependency for iOS
            string dbName = "myfibertrak_db.sqlite";
            string folderPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "..", "Library");
            string fullPath = Path.Combine(folderPath, dbName);

            string signature = "signature.png";
            string folderPath_ = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "..", "Library");
            string signaturefullPath = Path.Combine(folderPath_, signature);

            Rg.Plugins.Popup.Popup.Init();

            LoadApplication(new App(fullPath, signaturefullPath));

            return base.FinishedLaunching(app, options);
        }
    }
}
