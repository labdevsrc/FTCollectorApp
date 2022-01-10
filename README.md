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
            string dbName = "myfibertrak_db.sqlite";
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
            string dbName = "myfibertrak_db.sqlite";
            string folderPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "..", "Library");
            string fullPath = Path.Combine(folderPath, dbName);
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
            conn.CreateTable<User>();
            conn.InsertAll(content);
        }
    }
```
