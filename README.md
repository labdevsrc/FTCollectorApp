# Fibertrak CollectorApp



Almost all page in colector apps has similar process :
* [Create Local SQLite](https://github.com/labdevsrc/FTCollectorApp/blob/master/README.md#1-create-local-sqlite)
* [Ajax /API Request via HttpClient](https://github.com/labdevsrc/FTCollectorApp/blob/master/README.md#2-ajax-request--api-access-to-backup_of_myfibertrakend_user)
* [Populate local SQLite and List var with table from MySQL](https://github.com/labdevsrc/FTCollectorApp/blob/master/README.md#3-populate-local-sqlite-and-xamarin-linq)
* Use table's columns to populate entries in each pages
* If there's change , submit change with button 
Note : for Ajax/ API request , for now GET each table from AWS MySQL myfibertrak. 
This will be changed to GET whole tables at first time Collector installation later

## Login 
>MainPage.xaml.cs

![Login Page](assets/Login.png)

## 1. Create Local SQLite
### Download sqlite-net-pcl from NuGET's Visual Studio
![NuGet sqlite-net=pcl](assets/sqlite-net.png)
### Add below code on Solution.Android file MainActivity.cs
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

### Add below code on Solution.iOS file AppDelegate.cs
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

### Add constructor overloading in App.xaml.cs

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

### Create AJAX / API script in collector.fibertrak.com cloud
example collector.fibertrak.com/phonev4/xamarinLogin.php 
```
<?php
	include "conn.php";
	
	$sql="select email, end_user.key as UserKey, password, first_name, last_name, created_on from end_user where record_state='L' and field_data_collection='Y'";
	$res= mysqli_query($con,$sql);	
	$data = array();
	while($row = mysqli_fetch_assoc($res)){
		$data[] = $row;
	}
	echo json_encode($data);
?>
```
Note : JSON column key in , must match with User class properties in User.cs

### Create User or End_User class that has same structure with MySQL table.
For Login page, we use end_user table with column id, key,first_name, last_name, password,...

``` 
using SQLite; // sqlite-net-pcl library directive 

public class User{

	// set id as PK and autoincrement
	[PrimaryKey, AutoIncrement] 
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

## 2. Ajax request / API access to `backup_of_myfibertrak.end_user` 
### install NUGet NewtonSoft
![NewtonSoft](assets/newtonsoft.png)

Each page has default event handler right after Page appearing , called : OnAppearing().
In this repo , downloaded MySQL table will be stored in local SQLite, and then populate to List<T> or ObservableCollection<Object>.
[link to OnAppearing() in MainPage.xaml.cs](https://github.com/labdevsrc/FTCollectorApp/blob/095c644593bf3ad4ec01366bf75a8ad3358191af/FTCollectorApp/Page/MainPage.xaml.cs#L37)

```
    public partial class MainPage : ContentPage
    {
        private HttpClient httpClient = new HttpClient(); // create new HttpClient
        private ObservableCollection<User> Users;
        ...
        
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

		// Populate local SQLite 
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<User>();
                    conn.InsertAll(content); // populate SQLite table myfibertrak_db.db3
                }
            }
```
        
## 3. Populate local SQLite and Xamarin LINQ
refer to previous section [link to source code](https://github.com/labdevsrc/FTCollectorApp/blob/095c644593bf3ad4ec01366bf75a8ad3358191af/FTCollectorApp/Page/MainPage.xaml.cs#L52)
	
```
		// Populate local SQLite 
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<User>();
                    conn.InsertAll(content); // populate SQLite table myfibertrak_db.db3
                }
```
now, if user online, end_user table's content already populated to local SQLite.
in case user not online or no internet network, apps will read from local SQLite   [link to source code](https://github.com/labdevsrc/FTCollectorApp/blob/095c644593bf3ad4ec01366bf75a8ad3358191af/FTCollectorApp/Page/MainPage.xaml.cs#L62)
	
```
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
		// 
	     }
	     else
	     {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<User>();
                    var userdetails = conn.Table<User>().ToList();
                    Users = new ObservableCollection<User>(userdetails);
                }
             }
```
### Xamarin LINQ	
Sometime we need specific or certain entry from SQLite. With Xamarin LINQ, we can do Query example get first_name or last_name from known email and password like below :
[source code link](https://github.com/labdevsrc/FTCollectorApp/blob/095c644593bf3ad4ec01366bf75a8ad3358191af/FTCollectorApp/Page/MainPage.xaml.cs#L86)
```
	txtFirstName.Text = Users.Where(a => (a.email == entryEmail.Text) && (a.password == entryPassword.Text)).Select(a => a.first_name).First();
	txtLastName.Text = Users.Where(a => (a.email == entryEmail.Text) && (a.password == entryPassword.Text)).Select(a => a.last_name).First();
```
	
`entryEmail.Text` is email from user input.
	
`entryPassword.Text` is password from user input.
	
with above LINQ expression, we want to query like this 
`SELECT first_name FROM Users WHERE email =  entryEmail.Text and password = entryPassword.Text`



## Verify Job Page 
> VerifyJobPage.xaml and VerifyJobPage.xaml.cs
	
![Verify Job Page](assets/verifyjob_empty.png)

