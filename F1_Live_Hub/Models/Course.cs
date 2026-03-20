using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace F1_Live_Hub.Models
{
    public class Course : INotifyPropertyChanged
    {
        // ── Identifiants ──────────────────────────────────────────
        public string RaceId { get; set; }
        public string ChampionshipId { get; set; }
        public string Url { get; set; }

        // ── Infos course ──────────────────────────────────────────
        private string _raceName = "—";
        public string RaceName
        {
            get => _raceName;
            set { _raceName = value; OnPropertyChanged(); }
        }

        private int _round;
        public int Round
        {
            get => _round;
            set { _round = value; OnPropertyChanged(); }
        }

        private int? _laps;
        public int? Laps
        {
            get => _laps;
            set { _laps = value; OnPropertyChanged(); }
        }

        private string _year = "—";
        public string Year
        {
            get => _year;
            set { _year = value; OnPropertyChanged(); }
        }

        // ── Circuit ───────────────────────────────────────────────
        private string _circuitName = "—";
        public string CircuitName
        {
            get => _circuitName;
            set { _circuitName = value; OnPropertyChanged(); }
        }

        private string _country = "—";
        public string Country
        {
            get => _country;
            set { _country = value; OnPropertyChanged(); }
        }

        private string _city = "—";
        public string City
        {
            get => _city;
            set { _city = value; OnPropertyChanged(); }
        }

        private string _circuitLength = "—";
        public string CircuitLength
        {
            get => _circuitLength;
            set { _circuitLength = value; OnPropertyChanged(); }
        }

        private string _lapRecord = "—";
        public string LapRecord
        {
            get => _lapRecord;
            set { _lapRecord = value; OnPropertyChanged(); }
        }

        private int _corners;
        public int Corners
        {
            get => _corners;
            set { _corners = value; OnPropertyChanged(); }
        }

        private int _circuitFirstYear;
        public int CircuitFirstYear
        {
            get => _circuitFirstYear;
            set { _circuitFirstYear = value; OnPropertyChanged(); }
        }

        // ── Sessions ──────────────────────────────────────────────
        private string _raceDate = "—";
        public string RaceDate
        {
            get => _raceDate;
            set { _raceDate = value; OnPropertyChanged(); }
        }

        private string _raceTime = "—";
        public string RaceTime
        {
            get => _raceTime;
            set { _raceTime = value; OnPropertyChanged(); }
        }

        private string _qualyDate = "—";
        public string QualyDate
        {
            get => _qualyDate;
            set { _qualyDate = value; OnPropertyChanged(); }
        }

        private string _qualyTime = "—";
        public string QualyTime
        {
            get => _qualyTime;
            set { _qualyTime = value; OnPropertyChanged(); }
        }

        private string _fp1Date = "—";
        public string Fp1Date
        {
            get => _fp1Date;
            set { _fp1Date = value; OnPropertyChanged(); }
        }

        private string _fp1Time = "—";
        public string Fp1Time
        {
            get => _fp1Time;
            set { _fp1Time = value; OnPropertyChanged(); }
        }

        private string _fp2Date = "—";
        public string Fp2Date
        {
            get => _fp2Date;
            set { _fp2Date = value; OnPropertyChanged(); }
        }

        private string _fp2Time = "—";
        public string Fp2Time
        {
            get => _fp2Time;
            set { _fp2Time = value; OnPropertyChanged(); }
        }

        private string _fp3Date = "—";
        public string Fp3Date
        {
            get => _fp3Date;
            set { _fp3Date = value; OnPropertyChanged(); }
        }

        private string _fp3Time = "—";
        public string Fp3Time
        {
            get => _fp3Time;
            set { _fp3Time = value; OnPropertyChanged(); }
        }

        private string _sprintRaceDate = "—";
        public string SprintRaceDate
        {
            get => _sprintRaceDate;
            set { _sprintRaceDate = value; OnPropertyChanged(); }
        }

        private string _sprintRaceTime = "—";
        public string SprintRaceTime
        {
            get => _sprintRaceTime;
            set { _sprintRaceTime = value; OnPropertyChanged(); }
        }

        private string _sprintQualyDate = "—";
        public string SprintQualyDate
        {
            get => _sprintQualyDate;
            set { _sprintQualyDate = value; OnPropertyChanged(); }
        }

        // ── Vainqueur pilote ──────────────────────────────────────
        private string _winnerName = "—";
        public string WinnerName
        {
            get => _winnerName;
            set { _winnerName = value; OnPropertyChanged(); }
        }

        private string _winnerSurname = "—";
        public string WinnerSurname
        {
            get => _winnerSurname;
            set { _winnerSurname = value; OnPropertyChanged(); }
        }

        private string _winnerShortName = "—";
        public string WinnerShortName
        {
            get => _winnerShortName;
            set { _winnerShortName = value; OnPropertyChanged(); }
        }

        private string _winnerCountry = "—";
        public string WinnerCountry
        {
            get => _winnerCountry;
            set { _winnerCountry = value; OnPropertyChanged(); }
        }

        private int _winnerNumber;
        public int WinnerNumber
        {
            get => _winnerNumber;
            set { _winnerNumber = value; OnPropertyChanged(); }
        }

        // ── Vainqueur équipe ──────────────────────────────────────
        private string _teamWinnerName = "—";
        public string TeamWinnerName
        {
            get => _teamWinnerName;
            set { _teamWinnerName = value; OnPropertyChanged(); }
        }

        private string _teamWinnerCountry = "—";
        public string TeamWinnerCountry
        {
            get => _teamWinnerCountry;
            set { _teamWinnerCountry = value; OnPropertyChanged(); }
        }

        // ── Meilleur tour ─────────────────────────────────────────
        private string _fastLap = "—";
        public string FastLap
        {
            get => _fastLap;
            set { _fastLap = value; OnPropertyChanged(); }
        }

        private string _fastLapDriverId = "—";
        public string FastLapDriverId
        {
            get => _fastLapDriverId;
            set { _fastLapDriverId = value; OnPropertyChanged(); }
        }

        private string _fastLapTeamId = "—";
        public string FastLapTeamId
        {
            get => _fastLapTeamId;
            set { _fastLapTeamId = value; OnPropertyChanged(); }
        }

        // ── États UI ──────────────────────────────────────────────
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        private bool _hasError;
        public bool HasError
        {
            get => _hasError;
            set { _hasError = value; OnPropertyChanged(); }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        // ── INotifyPropertyChanged ────────────────────────────────
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}