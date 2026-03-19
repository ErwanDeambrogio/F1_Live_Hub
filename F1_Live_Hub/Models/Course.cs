using F1_Live_Hub.Models;
using F1_Live_Hub.Services;
using F1_Live_Hub.Views;
using System;
using System.ComponentModel;
using System.Linq;                          // ← AJOUTÉ pour .Where() et .LastOrDefault()
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Data;

namespace F1_Live_Hub.Models
{
    public class Course : INotifyPropertyChanged
    {
        private readonly SessionService _sessionService = new SessionService();

        private string _circuitShortName = "—";
        public string CircuitShortName
        {
            get => _circuitShortName;
            set { _circuitShortName = value; OnPropertyChanged(); }
        }

        private string _sessionType = "—";
        public string SessionType
        {
            get => _sessionType;
            set { _sessionType = value; OnPropertyChanged(); }
        }

        private string _sessionName = "—";
        public string SessionName
        {
            get => _sessionName;
            set { _sessionName = value; OnPropertyChanged(); }
        }

        private string _countryName = "—";
        public string CountryName
        {
            get => _countryName;
            set { _countryName = value; OnPropertyChanged(); }
        }

        private string _countryCode = "—";
        public string CountryCode
        {
            get => _countryCode;
            set { _countryCode = value; OnPropertyChanged(); }
        }

        private string _location = "—";
        public string Location
        {
            get => _location;
            set { _location = value; OnPropertyChanged(); }
        }

        private string _dateStart = "—";
        public string DateStart
        {
            get => _dateStart;
            set { _dateStart = value; OnPropertyChanged(); }
        }

        private string _year = "—";
        public string Year
        {
            get => _year;
            set { _year = value; OnPropertyChanged(); }
        }

        private string _gmtOffset = "—";
        public string GmtOffset
        {
            get => _gmtOffset;
            set { _gmtOffset = value; OnPropertyChanged(); }
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        private bool _hasError = false;
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

        public async Task LoadSessionDataAsync()
        {
            IsLoading = true;
            HasError = false;
            ErrorMessage = string.Empty;

            var sessions = await _sessionService.GetSessionDetails();

            // TEMPORAIRE : vérifie ce que l'API retourne
            if (sessions == null)
            {
                HasError = true;
                ErrorMessage = "API n'a rien retourné (null).";
                IsLoading = false;
                return;
            }

            // TEMPORAIRE : affiche le nombre et les types reçus
            var types = string.Join(", ", sessions.Select(s => s.session_type).Distinct());
            ErrorMessage = $"Reçu {sessions.Count} sessions. Types: {types}";
            HasError = true;

            SessionRoot session = sessions
                .Where(s => s.session_type == "Race")
                .LastOrDefault();

            if (session != null)
            {
                CircuitShortName = session.circuit_short_name ?? "—";
                SessionType = session.session_type ?? "—";
                SessionName = session.session_name ?? "—";
                CountryName = session.country_name ?? "—";
                CountryCode = session.country_code ?? "—";
                Location = session.location ?? "—";
                Year = session.year.ToString();
                GmtOffset = session.gmt_offset ?? "—";
                DateStart = session.date_start.ToString("dd MMM yyyy  HH:mm");
            }
            else
            {
                HasError = true;
                ErrorMessage = "Aucune session Race trouvée.";
            }

            IsLoading = false;
        }

        // ── INotifyPropertyChanged ────────────────────────────────
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}