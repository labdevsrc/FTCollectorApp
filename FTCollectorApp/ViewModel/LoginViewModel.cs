using CommunityToolkit.Mvvm.ComponentModel;
using FTCollectorApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SQLite;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using FTCollectorApp.View;
using System.Linq;

namespace FTCollectorApp.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {

        string emailText;
        public string EmailText{
            get=> emailText;
            set
            {
                SetProperty(ref emailText, value );
                CheckEmailCommand.Execute(emailText);
            }
        }


        string passwordText;
        public string PasswordText
        {
            get => passwordText;
            set
            {
                SetProperty(ref passwordText, value);
                CheckPasswordCommand.Execute(passwordText);
            }
        }

        [ObservableProperty]
        string firstName;

        [ObservableProperty]
        string lastName;

        ObservableCollection<User> Users;

        public ICommand CheckEmailCommand { get; set; }
        public ICommand CheckPasswordCommand { get; set; }
        public ICommand LoginCommand { get; set; }

        public LoginViewModel()
        {
            CheckEmailCommand = new Command<string>((param) => ExecuteCheckEmailCommand(param));
            CheckPasswordCommand = new Command<string>((param) => ExecuteCheckPasswordCommand(param));
            LoginCommand = new Command(async () => ExecuteLoginCommand());
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                //Type classname = object1.GetType();

                conn.CreateTable<User>();
                Console.WriteLine("CreateTable<User> ");
                var userdetails = conn.Table<User>().ToList();
                //conn.InsertAll(users);

                Users = new ObservableCollection<User>(userdetails);
            }
        }

        private void ExecuteCheckPasswordCommand(string param)
        {
            try
            {
                FirstName = Users.Where(a => (a.email == EmailText) && (a.password == param)).Select(a => a.first_name).First();
                LastName = Users.Where(a => (a.email == EmailText) && (a.password == param)).Select(a => a.last_name).First();
                Console.WriteLine(FirstName + " " + LastName);
            }
            catch (Exception exception)
            {
                FirstName = "";
                LastName = "";

                Console.WriteLine(exception.ToString());
            }
            OnPropertyChanged(nameof(FirstName)); // update FirstName entry
            OnPropertyChanged(nameof(LastName)); // update LastName entry
            Console.WriteLine();

        }

        private void ExecuteCheckEmailCommand(string param)
        {
            try
            {
                FirstName = Users.Where(a => (a.email == param) && (a.password == PasswordText)).Select(a => a.first_name).First();
                LastName = Users.Where(a => (a.email == param) && (a.password == PasswordText)).Select(a => a.last_name).First();
                Console.WriteLine(FirstName + " " + LastName);
            }
            catch (Exception exception)
            {
                FirstName = "";
                LastName = "";

                Console.WriteLine(exception.ToString());
            }
            OnPropertyChanged(nameof(FirstName));
            OnPropertyChanged(nameof(LastName));
            Console.WriteLine();

        }
        private async Task ExecuteLoginCommand()
        {
            Session.uid = Users.Where(a => (a.email == EmailText) && (a.password == PasswordText)).Select(a => a.UserKey).First(); // populate uid to Static-class (session) property uid  
            Session.crew_leader = $"{FirstName} {LastName}";
            await Application.Current.MainPage.Navigation.PushAsync(new VerifyJobPage());
        }
    }
}
