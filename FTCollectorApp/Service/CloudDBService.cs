using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Net.Http;
using Newtonsoft.Json;
using Plugin.Connectivity;
using FTCollectorApp.Model;
using Xamarin.Essentials;

namespace FTCollectorApp.Service
{
    public static class CloudDBService
    {
        static HttpClient client;

        static CloudDBService()
        {
            try
            {
                client = new HttpClient()
                {
                    BaseAddress = new Uri(Constants.BaseUrl)
                };
            }
            catch
            {

            }
        }

        // grab End User tables from Url https://collector.fibertrak.com/phonev4/xamarinLogin.php
        public static Task<IEnumerable<User>> GetEndUserFromAWSMySQLTable() =>
            GetAsync<IEnumerable<User>>(Constants.GetEndUserTableUrl);
        public static Task<IEnumerable<Job>> GetJobFromAWSMySQLTable() =>
            GetAsync<IEnumerable<Job>>(Constants.GetJobTableUrl);

        public static Task<IEnumerable<Site>> GetSiteFromAWSMySQLTable() =>
            GetAsync<IEnumerable<Site>>(Constants.GetSiteTableUrl);

        public static Task<IEnumerable<Crewdefault>> GetCrewDefaultFromAWSMySQLTable() =>
            GetAsync<IEnumerable<Crewdefault>>(Constants.GetCrewdefaultTableUrl);

        public static Task<IEnumerable<CodeSiteType>> GetCodeSiteTypeFromAWSMySQLTable() =>
            GetAsync<IEnumerable<CodeSiteType>>(Constants.GetCodeSiteTypeTableUrl);


        async static Task<T> GetAsync<T>(String Url)
        {
            var json = string.Empty;
            
            try
            {
                json = await client.GetStringAsync(Url);
                Console.WriteLine($"[CloudDBService] response : {json}");
                var content = JsonConvert.DeserializeObject<T>(json);
                //var sqliteContent = JsonConvert.DeserializeObject<List<User>>(response);

                Console.WriteLine($"[CloudDBService] content : {content.ToString()}");             
                return content;
            }
            catch(Exception exp)
            {
                Console.WriteLine("Exception {0}", exp.ToString());
            }

            return JsonConvert.DeserializeObject<T>(json); 
        }
        public static async Task PostJobEvent() => await PostJobEvent(null,null);
        public static async Task PostJobEvent(string param1, string param2)
        {

            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("jobnum",Session.jobnum),
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),

                new KeyValuePair<string, string>("min", Session.event_type == Session.ClockIn ? param1 : "0"),
                new KeyValuePair<string, string>("hr", Session.event_type == Session.ClockIn ? param2 : "0"),


                new KeyValuePair<string, string>("gps_sts", Session.gps_sts),
                
                // xSaveJobEvents.php Line 59 : $longitude=$_POST['longitude2'];
                // xSaveJobEvents.php Line 60 : $latitude =$_POST['lattitude2'];
                new KeyValuePair<string, string>("manual_latti", Session.gps_sts == "1" ? "0":Session.manual_latti),
                new KeyValuePair<string, string>("manual_longi", Session.gps_sts == "1" ? "0":Session.manual_longi),

                // xSaveJobEvents.php Line 73 : $longitude=$_POST['longitude2'];
                // xSaveJobEvents.php Line 74 : $latitude =$_POST['lattitude2'];
                new KeyValuePair<string, string>("lattitude2", Session.lattitude2),
                new KeyValuePair<string, string>("longitude2", Session.longitude2),

                new KeyValuePair<string, string>("evtype", Session.event_type), 

                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),

                new KeyValuePair<string, string>("ajaxname", Constants.InsertJobEvents)
            };
            // this Httpconten will work for Content-type : x-wwww-url-formencoded REST
            HttpContent content = new FormUrlEncodedContent(keyValues);

            HttpResponseMessage response = null;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                response = await client.PostAsync(Constants.InsertJobEvents, content);
                if (response.IsSuccessStatusCode)
                {
                    var isi = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[CloudService] Response from {Constants.InsertJobEvents} OK = 200 , content :" + isi);
                }
            }
            else
            {
                // Put to Pending Sync
            }
        }


        public static async Task PostCreateSiteAsync(string tagnum, string typecode)
        {


            var keyValues = new List<KeyValuePair<string, string>>{
                //new KeyValuePair<string, string>("jobnum",Session.jobnum),
                new KeyValuePair<string, string>("jno",Session.jobnum),
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("tag",tagnum),
                new KeyValuePair<string, string>("typecode",typecode),
                new KeyValuePair<string, string>("plansheet","0"),
                new KeyValuePair<string, string>("psitem","0"),


                //new KeyValuePair<string, string>("gps_sts", Session.gps_sts),                
                //new KeyValuePair<string, string>("manual_latti", Session.manual_latti),
                //new KeyValuePair<string, string>("manual_longi", Session.manual_longi),

                new KeyValuePair<string, string>("lattitude2", Session.lattitude2),
                new KeyValuePair<string, string>("longitude2", Session.longitude2),
                new KeyValuePair<string, string>("altitude", Session.altitude),
                new KeyValuePair<string, string>("accuracy", Session.accuracy),

                //new KeyValuePair<string, string>("evtype", Session.event_type),
                
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // created_on
                new KeyValuePair<string, string>("owner", Session.ownerkey), //
                new KeyValuePair<string, string>("user", Session.uid.ToString()),
                new KeyValuePair<string, string>("stage", Session.stage),
                new KeyValuePair<string, string>("gpstime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new KeyValuePair<string, string>("ownerCD", Session.ownerCD),
                new KeyValuePair<string, string>("ownerkey", Session.ownerkey),
                new KeyValuePair<string, string>("jobkey", Session.jobkey),
                new KeyValuePair<string, string>("createdfrm", "field collection"),
                new KeyValuePair<string, string>("usercounty", Session.countycode),
                new KeyValuePair<string, string>("ajaxname", Constants.CreateSiteTableUrl),


            };
            // this Httpconten will work for Content-type : x-wwww-url-formencoded REST
            HttpContent content = new FormUrlEncodedContent(keyValues);

            HttpResponseMessage response = null;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                response = await client.PostAsync(Constants.CreateSiteTableUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    var isi = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[CloudService.PostSiteAsync] Response from  OK = 200 , content :" + isi);
                }
            }
            else
            {
                // Put to Pending Sync
            }
        }

        public static async Task SaveCrewdata(string OWNER_CD, string name1, string name2, string name3, string name4, string name5, string name6, string diem1, string diem2, string diem3, string diem4, string diem5, string diem6, string driver11, string driver12, string driver13, string driver14, string driver15, string driver16)
        {

            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("evtype",Session.CrewAssembled),
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new KeyValuePair<string, string>("jobnum", Session.jobnum),
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("OWNER_CD", OWNER_CD),
                new KeyValuePair<string, string>("name1", name1),
                new KeyValuePair<string, string>("name2", name2),
                new KeyValuePair<string, string>("name3", name3),
                new KeyValuePair<string, string>("name4", name4),
                new KeyValuePair<string, string>("name5", name5),
                new KeyValuePair<string, string>("name6", name6),
                new KeyValuePair<string, string>("diem1", diem1),
                new KeyValuePair<string, string>("diem2", diem2),
                new KeyValuePair<string, string>("diem3", diem3),
                new KeyValuePair<string, string>("diem4", diem4),
                new KeyValuePair<string, string>("diem5", diem5),
                new KeyValuePair<string, string>("diem6", diem6),
                new KeyValuePair<string, string>("driver11", driver11),
                new KeyValuePair<string, string>("driver12", driver12),
                new KeyValuePair<string, string>("driver13", driver13),
                new KeyValuePair<string, string>("driver14", driver14),
                new KeyValuePair<string, string>("driver15", driver15),
                new KeyValuePair<string, string>("driver16", driver16),
                new KeyValuePair<string, string>("lattitude", Session.lattitude2),
                new KeyValuePair<string, string>("longitude", Session.longitude2)
            };
            // this Httpconten will work for Content-type : x-wwww-url-formencoded REST
            HttpContent content = new FormUrlEncodedContent(keyValues);

            HttpResponseMessage response = null;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                response = await client.PostAsync(Constants.SaveCrewUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    var isi = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[CloudService.SaveCrewdata] Response OK = 200 , content :" + isi);
                }
            }
            else
            {
                // Put to Pending Sync
            }
        }
    }
}
