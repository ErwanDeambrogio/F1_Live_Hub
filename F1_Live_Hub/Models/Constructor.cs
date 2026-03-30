using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1_Live_Hub.Models
{
    public class ConstructorsChampionship
    {
        public int classificationId { get; set; }
        public string teamId { get; set; }
        public int points { get; set; }
        public int position { get; set; }
        public int wins { get; set; }
        public ConstructorTeam team { get; set; }
    }

    public class RootConstructeur
    {
        public string api { get; set; }
        public string url { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public int total { get; set; }
        public int season { get; set; }
        public string championshipId { get; set; }
        public List<ConstructorsChampionship> constructors_championship { get; set; }
    }

    public class ConstructorTeam
    {
        public string teamId { get; set; }
        public string teamName { get; set; }
        public string country { get; set; }
        public int firstAppareance { get; set; }
        public int? constructorsChampionships { get; set; }
        public int? driversChampionships { get; set; }
        public string url { get; set; }
    }
}

