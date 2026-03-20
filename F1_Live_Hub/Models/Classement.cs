using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace F1_Live_Hub.Models
{
    public class DriverChampionship
    {
        public int Position { get; set; }
        public string DriverId { get; set; }
        public string TeamId { get; set; }
        public double Points { get; set; }
        public int Wins { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ShortName { get; set; }
        public string Nationality { get; set; }
        public int Number { get; set; }
        public string DriverUrl { get; set; }
        public string TeamName { get; set; }
        public string TeamCountry { get; set; }
        public string TeamUrl { get; set; }
    }

    public class ConstructorChampionship
    {
        public int Position { get; set; }
        public string TeamId { get; set; }
        public double Points { get; set; }
        public int Wins { get; set; }
        public string TeamName { get; set; }
        public string TeamCountry { get; set; }
        public int ConstructorsChampionships { get; set; }
        public int DriversChampionships { get; set; }
        public string TeamUrl { get; set; }
    }
}