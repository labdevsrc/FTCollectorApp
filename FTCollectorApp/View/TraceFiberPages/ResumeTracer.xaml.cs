using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;

namespace FTCollectorApp.View.TraceFiberPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResumeTracer : ContentPage
    {
        public ObservableCollection<ConduitsGroup> BeginningSiteList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ConduitsGroup>();

                    var table = conn.Table<ConduitsGroup>().GroupBy(b => b.HosTagNumber).Select(g => g.First()).ToList();
                    Console.WriteLine("BeginningSite");
                    return new ObservableCollection<ConduitsGroup>(table);
                }
            }
        }
        public ResumeTracer()
        {
            InitializeComponent();
            BindingContext = this;
        }

        string selectedTagNumber;
        private void OnIndexChanged(object sender, EventArgs e)
        {
            if(pTagNumber.SelectedIndex != -1)
            {
                var selected = pTagNumber.SelectedItem as ConduitsGroup;
                selectedTagNumber = selected.HosTagNumber;
            }
        }

        private void btnStartTracing_Clicked(object sender, EventArgs e)
        {

        }

        private void btnResume_Clicked(object sender, EventArgs e)
        {

        }
    }
}