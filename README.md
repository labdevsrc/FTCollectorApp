# Fibertrak CollectorApp
![Login Page](assets/Login.png)
![Verify Job Page](assets/verifyjob_empty.png)

# Login (MainPage.xaml.cs)
Almost all page in colector apps has similar process :
* Create Local SQLite
* GET each table from AWS MySQL myfibertrak --> this will be changed to GET whole tables at first time Collector install
* Populate local SQLite and List var with table from MySQL
* Use table's columns to populate entries in each pages
* If there's change , submit change with button 



## 1. Create Local SQLite
* Download sqlite-net-pcl from NuGET repo
![NuGet sqlite-net=pcl](assets/sqlite-net.png)
* Add below code on Solution.Android file MainActivity.cs
```
        protected override void OnCreate(Bundle savedInstanceState)
        {
        ....
            // SQLite initial
            string dbName = "myfibertrak_db.sqlite"; // SQLite db filename
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string fullPath = Path.Combine(folderPath, dbName);

            LoadApplication(new App());
         }
```
* Add below code on Solution.iOS file AppDelegate.cs
```
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
        ....
            // SQLite Dependency for iOS
            string dbName = "myfibertrak_db.sqlite"; // SQLite db filename
            string folderPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "..", "Library");
            string fullPath = Path.Combine(folderPath, dbName);
         }
```
* Add constructor overloading in App.xaml.cs
```
    public partial class App : Application
    {
        public static string DatabaseLocation = string.Empty;
        public App(string databaseLoc) // database location as param
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
            DatabaseLocation = databaseLoc;

        }
```        
* Create class that same structure with MySQL table
for Login page, we use end_user table with column id, key,first_name, last_name, password,...
``` 
using SQLite; // sqlite-net-pcl library directive 

public class User{
        public int id {get; set;}   // use snippet by typing "prop" then tab2x
        public int UserKey {get; set;}
        public string email {get;set;}
        public string password {get;set;} 
        public string first_name {get;set;}    
        public string last_name {get;set;}    
        ...
        public string created_on {get;set;}        
}
```

## 2. Ajax request / API access to `backup_of_myfibertrak.end_user` as below :
```
protected override async void OnAppearing()
{
    base.OnAppearing();

    Console.WriteLine("Connectivity : " + Connectivity.NetworkAccess);
    
    // if Internet connection available 
    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
    {

        Users.Clear();
        // grab End User tables from Url https://collector.fibertrak.com/phonev4/xamarinLogin.php
        // Constants.GetEndUserTableUrl = "https://collector.fibertrak.com/phonev4/xamarinLogin.php"
        var response = await httpClient.GetStringAsync(Constants.GetEndUserTableUrl); 
        var content = JsonConvert.DeserializeObject<List<User>>(response);
        Users = new ObservableCollection<User>(content);
        Console.WriteLine(response);

        using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
        {
            conn.CreateTable<User>(); // here , we create table from filename specified in App.Databaselocation or "myfibertrak_db.sqlite"
            conn.InsertAll(content);
        }
    }
```
