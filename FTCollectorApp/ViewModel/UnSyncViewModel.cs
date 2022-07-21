using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using FTCollectorApp.Model.AWS;
using FTCollectorApp.Service;
using SQLite;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{
    public partial class UnSyncViewModel : ObservableObject
    {

        UnSyncTaskList selectedTask;
        public UnSyncTaskList SelectedTask{
            get => selectedTask;
            set
            {

            }
          }

        public ObservableCollection<UnSyncTaskList> TaskList;
        public ObservableCollection<a_fiber_segment> AFCTaskList;
        ICommand SendCommand { get; set; }

        public UnSyncViewModel()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<UnSyncTaskList>();
                var table = conn.Table<UnSyncTaskList>().ToList();
                TaskList = new ObservableCollection<UnSyncTaskList>(table);

            }
            SendCommand = new Command(ExecuteSendCommand);
        }

        async void ExecuteSendCommand()
        {
            if (SelectedTask.ajaxTarget.Equals(Constants.ajaxSaveDuctTrace))
            {
                List<a_fiber_segment> table = new List<a_fiber_segment>();

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<a_fiber_segment>();
                    table = conn.Table<a_fiber_segment>().ToList();

                }

                List<KeyValuePair<string, string>> test = new List<KeyValuePair<string, string>>();
                foreach (var afs in table)
                {
                    foreach (var item in afs.GetType().GetProperties())
                    {
                        test.Add(new KeyValuePair<string, string>(item.Name, item.GetValue(afs).ToString()));
                    }
                    await CloudDBService.PostDuctTrace(test);
                }
            }
        }
    }
}
