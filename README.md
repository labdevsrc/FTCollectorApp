# Fibertrak CollectorApp
![Login Page](assets/Login.png)
![Verify Job Page](assets/verifyjob_empty.png)

# Login (MainPage.xaml.cs)
Almost all page in colector apps has similar process :
* Create Local SQLite
* Ajax /API Request via HttpClient
* Populate local SQLite and List var with table from MySQL
* Use table's columns to populate entries in each pages
* If there's change , submit change with button 
Note : for Ajax/ API request , for now GET each table from AWS MySQL myfibertrak. 
This will be changed to GET whole tables at first time Collector installation later



## 1. Create Local SQLite
* Download sqlite-net-pcl from NuGET's Visual Studio
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

* Create AJAX / API script in collector.fibertrak.com cloud
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

* Create User or End_User class that has same structure with MySQL table.
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
Each page has default event handler right after Page appearing , called : OnAppearing().
In this repo , downloaded MySQL table will be stored in local SQLite, and then populate to List<T> or ObservableCollection<Object>.
Example : Login Page.
API script : Select all from 

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


                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<User>();
                    conn.InsertAll(content); // populate SQLite table myfibertrak_db.db3
                }
            }
```
        
3. Xamarin LINQ
in MainPage.xaml.cs, end_user table already populated in local SQLite and IList Users.
With Xamarin LINQ, we can do Query example get first_name or last_name from known email and password like below :
	
`txtFirstName.Text = Users.Where(a => (a.email == entryEmail.Text) && (a.password == entryPassword.Text)).Select(a => a.first_name).First();`
entryEmail.Text is email from user input.
entryPassword.Text is password from user input.
with above LINQ expression, we want to query like this SELECT first_name FROM Users WHERE email =  entryEmail.Text and password = entryPassword.Text

