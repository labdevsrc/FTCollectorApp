﻿using FTCollectorApp.View.SitesPage;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AsBuiltDocMenu : ContentPage
    {
        public AsBuiltDocMenu()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void btnSite_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SiteInputPage());
        }

        private void btnTraceF_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TraceFiberMenu());
        }
    }
}