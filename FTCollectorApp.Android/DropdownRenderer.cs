using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using FTCollectorApp.Droid;
using FTCollectorApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Dropdown), typeof(DropdownRenderer))]  
namespace FTCollectorApp.Droid
{
    public class DropdownRenderer : ViewRenderer<Dropdown, Spinner>
    {
        AppCompatSpinner spinner;
        public DropdownRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Dropdown> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                spinner = new AppCompatSpinner(Context);
                SetNativeControl(spinner);
            }

            if (e.OldElement != null)
            {
                Control.ItemSelected -= OnItemSelected;
            }
            if (e.NewElement != null)
            {
                var view = e.NewElement;

                ArrayAdapter adapter = new ArrayAdapter(Context, Android.Resource.Layout.SimpleListItem1, view.ItemsSource);
                Control.Adapter = adapter;

                if (view.SelectedIndex != -1)
                {
                    Control.SetSelection(view.SelectedIndex);
                }

                Control.ItemSelected += OnItemSelected;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var view = Element;
            if (e.PropertyName == Dropdown.ItemsSourceProperty.PropertyName)
            {
                ArrayAdapter adapter = new ArrayAdapter(Context, Android.Resource.Layout.SimpleListItem1, view.ItemsSource);
                Control.Adapter = adapter;
            }
            if (e.PropertyName == Dropdown.SelectedIndexProperty.PropertyName)
            {
                Control.SetSelection(view.SelectedIndex);
            }
            base.OnElementPropertyChanged(sender, e);
        }

        private void OnItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var view = Element;
            if (view != null)
            {
                view.SelectedIndex = e.Position;
                view.OnItemSelected(e.Position);
            }
        }
    }
}  