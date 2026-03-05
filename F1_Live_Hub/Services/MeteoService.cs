using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace F1_Live_Hub.Services
{
    internal class MeteoService
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
        public class Root
        {
            public DateTime date { get; set; }
            public int session_key { get; set; }
            public double humidity { get; set; }
            public double pressure { get; set; }
            public int rainfall { get; set; }
            public double track_temperature { get; set; }
            public double wind_speed { get; set; }
            public int meeting_key { get; set; }
            public int wind_direction { get; set; }
            public double air_temperature { get; set; }
        }


    }
}
