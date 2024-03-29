﻿using FTCollectorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using System.Collections.ObjectModel;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.View.Utils;
using FTCollectorApp.Model;
using FTCollectorApp.Service;
using Rg.Plugins.Popup.Services;
using System.Windows.Input;

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DuctPage : ContentPage
    {

        public DuctPage()
        {
            InitializeComponent();

            BindingContext = new DuctViewModel();
        }


        private void btnCamera(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CameraViewPage());
        }

     }
}