using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1_Live_Hub.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Circuit
    {
        public string circuitId { get; set; }
        public string circuitName { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public int? circuitLength { get; set; }
        public string lapRecord { get; set; }
        public int? firstParticipationYear { get; set; }
        public int? numberOfCorners { get; set; }
        public string fastestLapDriverId { get; set; }
        public string fastestLapTeamId { get; set; }
        public int? fastestLapYear { get; set; }
        public string url { get; set; }
    }

    public partial class Root
    {
        public string api { get; set; }
        public string url { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public string query { get; set; }
        public int total { get; set; }
        public List<Circuit> circuits { get; set; }
    }
}

