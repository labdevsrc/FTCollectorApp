# Fibertrak CollectorApp
![Login Page](assets/Login.png)
![Verify Job Page](assets/verifyjob_empty.png)

## Login (MainPage.xaml.cs)
### Ajax request / API access to `backup_of_myfibertrak.end_user` as below :
```
protected override async void OnAppearing()
{
    base.OnAppearing();

    Console.WriteLine("Connectivity : " + Connectivity.NetworkAccess);

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
