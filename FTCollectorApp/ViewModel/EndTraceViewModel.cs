using CommunityToolkit.Mvvm.ComponentModel;
using FTCollectorApp.Model.Reference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SQLite;
using System.Web;

namespace FTCollectorApp.ViewModel
{
    public partial class EndTraceViewModel: ObservableObject
    {
        [ObservableProperty]
        ConduitsGroup selectedTagNum;

        public ObservableCollection<ConduitsGroup> ConduitSiteTag
        {
            get
            {
                var table = ConduitsGroupListTable.GroupBy(b => b.HosTagNumber).Select(g => g.First()).ToList();
                Console.WriteLine("BeginningSite");
                return new ObservableCollection<ConduitsGroup>(table);
            }
        }

        ObservableCollection<ConduitsGroup> ConduitsGroupListTable;
        ObservableCollection<ColorCode> ColorHextList;

        public EndTraceViewModel()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<ConduitsGroup>();
                var table1 = conn.Table<ConduitsGroup>().ToList();
                ConduitsGroupListTable = new ObservableCollection<ConduitsGroup>(table1);

                conn.CreateTable<ColorCode>();
                var table2 = conn.Table<ColorCode>().ToList();
                ColorHextList = new ObservableCollection<ColorCode>(table2);
            }
        }

        public ObservableCollection<ConduitsGroup> DuctConduitDatas
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    var table = ConduitsGroupListTable.ToList();
                    if (SelectedTagNum?.HosTagNumber != null)
                        table = ConduitsGroupListTable.Where(b => b.HosTagNumber == SelectedTagNum.HosTagNumber).ToList();
                    foreach (var col in table)
                    {
                        col.DuctSize = HttpUtility.HtmlDecode(col.DuctSize);
                        col.WhichDucts = col.Direction + " " + col.DirCnt;
                    }
                    Console.WriteLine("DuctConduitDatas ");
                    return new ObservableCollection<ConduitsGroup>(table);
                }
            }
        }
    }
}
