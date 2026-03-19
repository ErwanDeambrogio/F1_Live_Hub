using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace F1_Live_Hub.Models
{
    public class PiloteRaceResult
    {
        public string RaceName { get; set; }
        public string RaceDate { get; set; }
        public int Round { get; set; }
        public string CircuitName { get; set; }
        public string Country { get; set; }
        public int FinishingPosition { get; set; }
        public string PositionLabel { get; set; }
        public int GridPosition { get; set; }
        public string RaceTime { get; set; }
        public double PointsObtained { get; set; }
        public bool Retired { get; set; }
        public int SprintPosition { get; set; }
        public double SprintPoints { get; set; }
    }

    public class Pilote : INotifyPropertyChanged
    {
        public string DriverId { get; set; }
        public string Url { get; set; }
        public string TeamUrl { get; set; }

        private string _photoUrl = "";
        public string PhotoUrl
        {
            get => _photoUrl;
            set { _photoUrl = value; OnPropertyChanged(); }
       
        }

        private string _name = "—";
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        private string _surname = "—";
        public string Surname
        {
            get => _surname;
            set { _surname = value; OnPropertyChanged(); }
        }

        private string _nationality = "—";
        public string Nationality
        {
            get => _nationality;
            set { _nationality = value; OnPropertyChanged(); }
        }

        private string _birthday = "—";
        public string Birthday
        {
            get => _birthday;
            set { _birthday = value; OnPropertyChanged(); }
        }

        private int _number;
        public int Number
        {
            get => _number;
            set { _number = value; OnPropertyChanged(); }
        }

        private string _shortName = "—";
        public string ShortName
        {
            get => _shortName;
            set { _shortName = value; OnPropertyChanged(); }
        }

        private string _teamId = "";
        public string TeamId
        {
            get => _teamId;
            set
            {
                _teamId = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TeamColor));
                OnPropertyChanged(nameof(TeamLogoUrl));
            }
        }

        private string _teamName = "—";
        public string TeamName
        {
            get => _teamName;
            set { _teamName = value; OnPropertyChanged(); }
        }

        private string _teamNationality = "—";
        public string TeamNationality
        {
            get => _teamNationality;
            set { _teamNationality = value; OnPropertyChanged(); }
        }

        private int _constructorsChampionships;
        public int ConstructorsChampionships
        {
            get => _constructorsChampionships;
            set { _constructorsChampionships = value; OnPropertyChanged(); }
        }

        private int _driversChampionships;
        public int DriversChampionships
        {
            get => _driversChampionships;
            set { _driversChampionships = value; OnPropertyChanged(); }
        }

        // ── Stats courses ─────────────────────────────────────────
        private int _wins;
        public int Wins
        {
            get => _wins;
            set { _wins = value; OnPropertyChanged(); }
        }

        private int _podiums;
        public int Podiums
        {
            get => _podiums;
            set { _podiums = value; OnPropertyChanged(); }
        }

        private double _totalPoints;
        public double TotalPoints
        {
            get => _totalPoints;
            set { _totalPoints = value; OnPropertyChanged(); }
        }

        private int _racesEntered;
        public int RacesEntered
        {
            get => _racesEntered;
            set { _racesEntered = value; OnPropertyChanged(); }
        }

        private int _retirements;
        public int Retirements
        {
            get => _retirements;
            set { _retirements = value; OnPropertyChanged(); }
        }

        private int _bestFinish;
        public int BestFinish
        {
            get => _bestFinish;
            set { _bestFinish = value; OnPropertyChanged(); }
        }

        private List<PiloteRaceResult> _raceResults = new List<PiloteRaceResult>();
        public List<PiloteRaceResult> RaceResults
        {
            get => _raceResults;
            set { _raceResults = value; OnPropertyChanged(); }
        }

        // ── Classement championnat pilotes ────────────────────────
        private int _championshipPosition;
        public int ChampionshipPosition
        {
            get => _championshipPosition;
            set
            {
                _championshipPosition = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ChampionshipPositionColor));
            }
        }

        private double _championshipPoints;
        public double ChampionshipPoints
        {
            get => _championshipPoints;
            set { _championshipPoints = value; OnPropertyChanged(); }
        }

        private int _championshipWins;
        public int ChampionshipWins
        {
            get => _championshipWins;
            set { _championshipWins = value; OnPropertyChanged(); }
        }

        // ── Classement constructeur ───────────────────────────────
        private int _constructorPosition;
        public int ConstructorPosition
        {
            get => _constructorPosition;
            set { _constructorPosition = value; OnPropertyChanged(); }
        }

        private double _constructorPoints;
        public double ConstructorPoints
        {
            get => _constructorPoints;
            set { _constructorPoints = value; OnPropertyChanged(); }
        }

        private int _constructorWins;
        public int ConstructorWins
        {
            get => _constructorWins;
            set { _constructorWins = value; OnPropertyChanged(); }
        }

        // ── Couleurs dynamiques ───────────────────────────────────
        public string ChampionshipPositionColor
        {
            get
            {
                switch (_championshipPosition)
                {
                    case 1: return "#FFD700";
                    case 2: return "#C0C0C0";
                    case 3: return "#CD7F32";
                    default: return "#E10600";
                }
            }
        }

        public string TeamColor
        {
            get
            {
                switch (_teamId?.ToLower())
                {
                    case "red_bull": return "#3671C6";
                    case "ferrari": return "#E8002D";
                    case "mercedes": return "#27F4D2";
                    case "mclaren": return "#FF8000";
                    case "aston_martin": return "#358C75";
                    case "alpine": return "#FF87BC";
                    case "williams": return "#64C4FF";
                    case "haas": return "#B6BABD";
                    case "kick_sauber": return "#52E252";
                    case "rb": return "#6692FF";
                    default: return "#555555";
                }
            }
        }

        public string TeamLogoUrl
        {
            get
            {
                switch (_teamId?.ToLower())
                {
                    case "red_bull":
                        return "https://upload.wikimedia.org/wikipedia/en/thumb/9/97/Red_Bull_Racing_logo.svg/200px-Red_Bull_Racing_logo.svg.png";
                    case "ferrari":
                        return "https://upload.wikimedia.org/wikipedia/en/thumb/d/d4/Scuderia_Ferrari_Logo.svg/200px-Scuderia_Ferrari_Logo.svg.png";
                    case "mercedes":
                        return "https://upload.wikimedia.org/wikipedia/en/thumb/f/fb/Mercedes-Benz_in_Formula_One_logo.svg/200px-Mercedes-Benz_in_Formula_One_logo.svg.png";
                    case "mclaren":
                        return "https://upload.wikimedia.org/wikipedia/en/thumb/6/66/McLaren_Racing_logo.svg/200px-McLaren_Racing_logo.svg.png";
                    case "aston_martin":
                        return "https://upload.wikimedia.org/wikipedia/en/thumb/9/9f/Aston_Martin_F1_Logo.svg/200px-Aston_Martin_F1_Logo.svg.png";
                    case "alpine":
                        return "https://upload.wikimedia.org/wikipedia/commons/thumb/7/7e/Alpine_F1_Team_Logo.svg/200px-Alpine_F1_Team_Logo.svg.png";
                    case "williams":
                        return "https://upload.wikimedia.org/wikipedia/commons/thumb/8/80/Williams_Racing_logo_2020.svg/200px-Williams_Racing_logo_2020.svg.png";
                    case "haas":
                        return "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9b/Haas_F1_Team_Logo.svg/200px-Haas_F1_Team_Logo.svg.png";
                    case "kick_sauber":
                        return "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9b/Sauber_Motorsport_AG_logo.svg/200px-Sauber_Motorsport_AG_logo.svg.png";
                    case "rb":
                        return "https://upload.wikimedia.org/wikipedia/commons/thumb/8/82/Visa_Cash_App_RB_Formula_One_Team_logo.svg/200px-Visa_Cash_App_RB_Formula_One_Team_logo.svg.png";
                    default:
                        return "";
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}