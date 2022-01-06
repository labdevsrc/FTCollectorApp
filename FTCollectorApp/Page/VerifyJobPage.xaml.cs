using FTCollectorApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerifyJobPage : ContentPage
    {
        private IList<Job> _job;
        private Job _currjob;
        public List<string> OwnerName;
        public VerifyJobPage()
        {
            InitializeComponent();
            _job = GetJobs();

            var _ownergrouped = _job.GroupBy(b => b.OwnerName).Select(g => g.First()).ToList();
            foreach (var ownergrouped in _ownergrouped)
                jobOwnersPicker.Items.Add(ownergrouped.OwnerName);



        }

        private IList<Job> GetJobs()
        {
            return new List<Job>{
                new Job { OwnerName = "FDOT District 4", OWNER_CD = "FC014", JobNumber = "17910", OwnerKey = 14, JobLocation = "SR 806 Atlantic Ave & I95 Interchange", ContactName = "Eric Juhl", CustomerName = "Rusell Engineering, Inc.", CustomerPhone = "954 321 9336" },
                new Job { OwnerName = "FDOT District 4", OWNER_CD = "FC014", JobNumber = "17910", OwnerKey = 14, JobLocation = "SR 806 Atlantic Ave & I95 Interchange", ContactName = "Eric Juhl", CustomerName = "Russell Engineering, Inc.", CustomerPhone = "954 321 9336" },
                new Job { OwnerName = "FDOT District 6", OWNER_CD = "FC016", JobNumber = "19180", OwnerKey = 16, JobLocation = "I95/SR9A - N of NW 29th St to S of NW 131st", ContactName = "Liddie Regeledo", CustomerName = "Rusell Engineering, Inc.", CustomerPhone = "3052227040" },
                new Job { OwnerName = "FDOT District 6", OWNER_CD = "FC016", JobNumber = "19180", OwnerKey = 16, JobLocation = "I95/SR9A - N of NW 29th St to S of NW 131st", ContactName = "Liddie Regeledo", CustomerName = "Russell Engineering, Inc.", CustomerPhone = "3052227040" },
                new Job { OwnerName = "FDOT FTE", OWNER_CD = "FC018", JobNumber = "17-16535", OwnerKey = 18, JobLocation = "SR 570 & Braddock Rd, Lakeland, FL", ContactName = "D J Conner", CustomerName = "Highway Safety Devices", CustomerPhone = "813-759-1559" },
                new Job { OwnerName = "FDOT FTE", OWNER_CD = "FC018", JobNumber = "21-20590", OwnerKey = 18, JobLocation = "SR 91 Interchange", ContactName = "Alan Ballard", CustomerName = "FIE", CustomerPhone = "561-743-9737" },
                new Job { OwnerName = "Palm Beach Co.", OWNER_CD = "FC029", JobNumber = "19080", OwnerKey = 29, JobLocation = "Jupiter Indiantown Rd from Island Way to US1", ContactName = "camilo", CustomerName = "Carr Construction, LLC", CustomerPhone = "5555555555" },
                new Job { OwnerName = "Palm Beach Co.", OWNER_CD = "FC029", JobNumber = "19230", OwnerKey = 29, JobLocation = "FDOT Palm Beach various locations T4550", ContactName = "Camilo Martinez", CustomerName = "Carr Construction, LLC", CustomerPhone = "561-719-0980" },
                new Job { OwnerName = "FDOT FTE", OWNER_CD = "FC018", JobNumber = "17-16535", OwnerKey = 18, JobLocation = "SR 570 & Braddock Rd, Lakeland, FL", ContactName = "D J Conner", CustomerName = "Highway Safety Devices", CustomerPhone = "813-759-1559" },
                new Job { OwnerName = "FDOT FTE", OWNER_CD = "FC018", JobNumber = "21-20590", OwnerKey = 18, JobLocation = "SR 91 Interchange", ContactName = "Alan Ballard", CustomerName = "FIE", CustomerPhone = "561-743-9737" },
                new Job { OwnerName = "FDOT FTE", OWNER_CD = "FC018", JobNumber = "17-16535", OwnerKey = 18, JobLocation = "SR 570 & Braddock Rd, Lakeland, FL", ContactName = "D J Conner", CustomerName = "Highway Safety Devices", CustomerPhone = "813-759-1559" },
                new Job { OwnerName = "FDOT FTE", OWNER_CD = "FC018", JobNumber = "21-20590", OwnerKey = 18, JobLocation = "SR 91 Interchange", ContactName = "Alan Ballard", CustomerName = "FIE", CustomerPhone = "561-743-9737" },
                new Job { OwnerName = "Port St. Lucie", OWNER_CD = "PCS005", JobNumber = "1816905", OwnerKey = 119, JobLocation = "Manth Lane to US1 Federal Highway", ContactName = "Crosstown Parkway", CustomerName = "Traffic Management Solutions", CustomerPhone = "904-256-2500" },
                new Job { OwnerName = "Port St. Lucie", OWNER_CD = "PCS005", JobNumber = "18745", OwnerKey = 119, JobLocation = "PSL Crosstown", ContactName = "Camilo", CustomerName = "Ferreira", CustomerPhone = "423-222-5555" },
                new Job { OwnerName = "Port St. Lucie", OWNER_CD = "PCS005", JobNumber = "18745-4", OwnerKey = 119, JobLocation = "Crosstown", ContactName = "Camilo", CustomerName = "Ferreira", CustomerPhone = "123-555-2222" },
                new Job { OwnerName = "Port St. Lucie", OWNER_CD = "PCS005", JobNumber = "18745-3", OwnerKey = 119, JobLocation = "Crosstown", ContactName = "Camilo", CustomerName = "Ferreira", CustomerPhone = "423-222-5555" },
                new Job { OwnerName = "Port St. Lucie", OWNER_CD = "PCS005", JobNumber = "18745", OwnerKey = 119, JobLocation = "PSL Crosstown", ContactName = "Camilo", CustomerName = "Ferreira Construction", CustomerPhone = "423-222-5555" },
                new Job { OwnerName = "Port St. Lucie", OWNER_CD = "PCS005", JobNumber = "18745-4", OwnerKey = 119, JobLocation = "Crosstown", ContactName = "Camilo", CustomerName = "Ferreira Construction", CustomerPhone = "123-555-2222" },
                new Job { OwnerName = "Port St. Lucie", OWNER_CD = "PCS005", JobNumber = "18745-3", OwnerKey = 119, JobLocation = "Crosstown", ContactName = "Camilo", CustomerName = "Ferreira Construction", CustomerPhone = "423-222-5555" },
                new Job { OwnerName = "Port St. Lucie", OWNER_CD = "PCS005", JobNumber = "18745-50", OwnerKey = 119, JobLocation = "PSL at Tradition", ContactName = "camilo", CustomerName = "Universal Cabling", CustomerPhone = "5555555555" },
                new Job { OwnerName = "Port St. Lucie", OWNER_CD = "PCS005", JobNumber = "19215", OwnerKey = 119, JobLocation = "psl", ContactName = "camilo", CustomerName = "Universal Cabling", CustomerPhone = "5555555555" },
                new Job { OwnerName = "Port St. Lucie", OWNER_CD = "PCS005", JobNumber = "19270-10", OwnerKey = 119, JobLocation = "Coral Reef Park", ContactName = "Lee Dearlove", CustomerName = "City of PSL", CustomerPhone = "7723619094" },
                new Job { OwnerName = "North Carolina Turnpike Authority", OWNER_CD = "PCS006", JobNumber = "1918750", OwnerKey = 120, JobLocation = "Raleigh, North Carolina", ContactName = "Bill Cleven", CustomerName = "Kapsch TrafficCom USA", CustomerPhone = "512-720-1510" },
                new Job { OwnerName = "City of Port St. Lucie", OWNER_CD = "PCS005", JobNumber = "1816905", OwnerKey = 119, JobLocation = "Manth Lane to US1 Federal Highway", ContactName = "Crosstown Parkway", CustomerName = "Traffic Management Solutions", CustomerPhone = "904-256-2500" },
                new Job { OwnerName = "City of Port St. Lucie", OWNER_CD = "PCS005", JobNumber = "18745", OwnerKey = 119, JobLocation = "PSL Crosstown", ContactName = "Camilo", CustomerName = "Ferreira", CustomerPhone = "423-222-5555" },
                new Job { OwnerName = "City of Port St. Lucie", OWNER_CD = "PCS005", JobNumber = "18745-4", OwnerKey = 119, JobLocation = "Crosstown", ContactName = "Camilo", CustomerName = "Ferreira", CustomerPhone = "123-555-2222" },
                new Job { OwnerName = "City of Port St. Lucie", OWNER_CD = "PCS005", JobNumber = "18745-3", OwnerKey = 119, JobLocation = "Crosstown", ContactName = "Camilo", CustomerName = "Ferreira", CustomerPhone = "423-222-5555" },
                new Job { OwnerName = "City of Port St. Lucie", OWNER_CD = "PCS005", JobNumber = "18745", OwnerKey = 119, JobLocation = "PSL Crosstown", ContactName = "Camilo", CustomerName = "Ferreira Construction", CustomerPhone = "423-222-5555" },
                new Job { OwnerName = "City of Port St. Lucie", OWNER_CD = "PCS005", JobNumber = "18745-4", OwnerKey = 119, JobLocation = "Crosstown", ContactName = "Camilo", CustomerName = "Ferreira Construction", CustomerPhone = "123-555-2222" },
                new Job { OwnerName = "City of Port St. Lucie", OWNER_CD = "PCS005", JobNumber = "18745-3", OwnerKey = 119, JobLocation = "Crosstown", ContactName = "Camilo", CustomerName = "Ferreira Construction", CustomerPhone = "423-222-5555" },
                new Job { OwnerName = "City of Port St. Lucie", OWNER_CD = "PCS005", JobNumber = "18745-50", OwnerKey = 119, JobLocation = "PSL at Tradition", ContactName = "camilo", CustomerName = "Universal Cabling", CustomerPhone = "5555555555" },
                new Job { OwnerName = "City of Port St. Lucie", OWNER_CD = "PCS005", JobNumber = "19215", OwnerKey = 119, JobLocation = "psl", ContactName = "camilo", CustomerName = "Universal Cabling", CustomerPhone = "5555555555" },
                new Job { OwnerName = "City of Port St. Lucie", OWNER_CD = "PCS005", JobNumber = "19270-10", OwnerKey = 119, JobLocation = "Coral Reef Park", ContactName = "Lee Dearlove", CustomerName = "City of PSL", CustomerPhone = "7723619094" },
                new Job { OwnerName = "Virgin Trains USA", OWNER_CD = "PCS008", JobNumber = "19225", OwnerKey = 216, JobLocation = "FL East Coast Rail line", ContactName = "Lou Opall", CustomerName = "Wabtec", CustomerPhone = "352-302-3139" },
                new Job { OwnerName = "FDOT District 4", OWNER_CD = "FC014", JobNumber = "17910", OwnerKey = 14, JobLocation = "SR 806 Atlantic Ave & I95 Interchange", ContactName = "Eric Juhl", CustomerName = "Rusell Engineering, Inc.", CustomerPhone = "954 321 9336" },
                new Job { OwnerName = "FDOT District 4", OWNER_CD = "FC014", JobNumber = "17910", OwnerKey = 14, JobLocation = "SR 806 Atlantic Ave & I95 Interchange", ContactName = "Eric Juhl", CustomerName = "Russell Engineering, Inc.", CustomerPhone = "954 321 9336" },
                new Job { OwnerName = "FDOT District 6", OWNER_CD = "FC016", JobNumber = "19180", OwnerKey = 16, JobLocation = "I95/SR9A - N of NW 29th St to S of NW 131st", ContactName = "Liddie Regeledo", CustomerName = "Rusell Engineering, Inc.", CustomerPhone = "3052227040" },
                new Job { OwnerName = "FDOT District 6", OWNER_CD = "FC016", JobNumber = "19180", OwnerKey = 16, JobLocation = "I95/SR9A - N of NW 29th St to S of NW 131st", ContactName = "Liddie Regeledo", CustomerName = "Russell Engineering, Inc.", CustomerPhone = "3052227040" },
                new Job { OwnerName = "Palm Beach Co.", OWNER_CD = "FC029", JobNumber = "19080", OwnerKey = 29, JobLocation = "Jupiter Indiantown Rd from Island Way to US1", ContactName = "camilo", CustomerName = "Carr Construction, LLC", CustomerPhone = "5555555555" },
                new Job { OwnerName = "Palm Beach Co.", OWNER_CD = "FC029", JobNumber = "19230", OwnerKey = 29, JobLocation = "FDOT Palm Beach various locations T4550", ContactName = "Camilo Martinez", CustomerName = "Carr Construction, LLC", CustomerPhone = "561-719-0980" },
                new Job { OwnerName = "FDOT District 4", OWNER_CD = "FC014", JobNumber = "17910", OwnerKey = 14, JobLocation = "SR 806 Atlantic Ave & I95 Interchange", ContactName = "Eric Juhl", CustomerName = "Rusell Engineering, Inc.", CustomerPhone = "954 321 9336" },
                new Job { OwnerName = "FDOT District 4", OWNER_CD = "FC014", JobNumber = "17910", OwnerKey = 14, JobLocation = "SR 806 Atlantic Ave & I95 Interchange", ContactName = "Eric Juhl", CustomerName = "Russell Engineering, Inc.", CustomerPhone = "954 321 9336" },
                new Job { OwnerName = "FDOT FTE", OWNER_CD = "FC018", JobNumber = "17-16535", OwnerKey = 18, JobLocation = "SR 570 & Braddock Rd, Lakeland, FL", ContactName = "D J Conner", CustomerName = "Highway Safety Devices", CustomerPhone = "813-759-1559" },
                new Job { OwnerName = "FDOT FTE", OWNER_CD = "FC018", JobNumber = "21-20590", OwnerKey = 18, JobLocation = "SR 91 Interchange", ContactName = "Alan Ballard", CustomerName = "FIE", CustomerPhone = "561-743-9737" },
            };
        }


        private void jobOwnersPicker_SelectedIndexChanged(object sender, EventArgs e)
        {


            var owner = jobOwnersPicker.Items[jobOwnersPicker.SelectedIndex];

            var _jobNumbergrouped = _job.Where(a => a.OwnerName == owner).GroupBy(b => b.JobNumber).Select(g => g.First()).ToList();
            foreach (var jobNumbergrouped in _jobNumbergrouped)
                jobNumbersPicker.Items.Add(jobNumbergrouped.JobNumber);

            //var jobNumber = jobNumbersPicker.Items[jobNumbersPicker.SelectedIndex];
            //_currjob = _job.Single(curjob => curjob.OwnerName == owner && curjob.JobNumber == jobNumber );
            //DisplayAlert("Selected Owner", $"Owner : {owner}", "OK");
        }

        private void jobNumbersPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

            var selectedOwner = jobOwnersPicker.SelectedIndex;
            if (selectedOwner == 0)
            {
                DisplayAlert("Warning", "Select Owner First", "OK");

            }

            var jobNumber = jobNumbersPicker.Items[jobNumbersPicker.SelectedIndex];
            var owner = jobOwnersPicker.Items[jobOwnersPicker.SelectedIndex];

            jobLocation.Text = _job.Where(a => (a.OwnerName == owner) && (a.JobNumber == jobNumber)).Select(a => a.JobLocation).First();
            contactName.Text = _job.Where(a => (a.OwnerName == owner) && (a.JobNumber == jobNumber)).Select(a => a.ContactName).First();
            custName.Text = _job.Where(a => (a.OwnerName == owner) && (a.JobNumber == jobNumber)).Select(a => a.CustomerName).First();
            custPhoneNum.Text = _job.Where(a => (a.OwnerName == owner) && (a.JobNumber == jobNumber)).Select(a => a.CustomerPhone).First();
        }
    }
}