using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace F1_Live_Hub.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Driver
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string nationality { get; set; }
        public string birthday { get; set; }
        public int number { get; set; }
        public string shortName { get; set; }
        public string url { get; set; }
    }

    public class DriversChampionship
    {
        public int classificationId { get; set; }
        public string driverId { get; set; }
        public string teamId { get; set; }
        public double points { get; set; }
        public int position { get; set; }
        public int wins { get; set; }
        public Driver driver { get; set; }
        public Team team { get; set; }
    }

    public class RootClassement
    {
        public string api { get; set; }
        public string url { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public int total { get; set; }
        public int season { get; set; }
        public string championshipId { get; set; }
        public List<DriversChampionship> drivers_championship { get; set; }
    }

    public class Team
    {
        public string teamId { get; set; }
        public string teamName { get; set; }
        public string country { get; set; }
        public int? firstAppareance { get; set; }
        public int? constructorsChampionships { get; set; }
        public int? driversChampionships { get; set; }
        public string url { get; set; }
    }

}

