using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace F1_Live_Hub.Services
{
    public class AuthService
    {
        private readonly string _filePath;
        private readonly string _sessionFile;

        public AuthService()
        {
            var folder = Path.Combine(
                System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.ApplicationData),
                "F1LiveHub");

            // Crée le dossier une seule fois ici
            Directory.CreateDirectory(folder);

            _filePath = Path.Combine(folder, "users.json");
            _sessionFile = Path.Combine(folder, "session.json");
        }

        // ── Enregistre un utilisateur ─────────────────────────────
        public bool Register(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password)) return false;

            var users = LoadUsers();
            if (users[username] != null) return false;

            users[username] = password;
            File.WriteAllText(_filePath, users.ToString(Formatting.Indented));
            return true;
        }

        // ── Vérifie les identifiants ──────────────────────────────
        public bool Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password)) return false;

            var users = LoadUsers();
            var stored = users[username]?.ToString();
            return stored == password;
        }

        // ── Sauvegarde l'utilisateur connecté ─────────────────────
        public void SaveCurrentUser(string username)
        {
            // Le dossier est déjà créé dans le constructeur
            File.WriteAllText(_sessionFile,
                JsonConvert.SerializeObject(new { username }));
        }

        // ── Récupère l'utilisateur connecté ───────────────────────
        public string GetCurrentUser()
        {
            try
            {
                if (!File.Exists(_sessionFile)) return null;
                var json = JObject.Parse(File.ReadAllText(_sessionFile));
                return json["username"]?.ToString();
            }
            catch { return null; }
        }

        // ── Vérifie si quelqu'un est connecté ─────────────────────
        public bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(GetCurrentUser());
        }

        // ── Déconnexion ───────────────────────────────────────────
        public void Logout()
        {
            if (File.Exists(_sessionFile))
                File.Delete(_sessionFile);
        }

        private JObject LoadUsers()
        {
            try
            {
                if (File.Exists(_filePath))
                    return JObject.Parse(File.ReadAllText(_filePath));
            }
            catch { }
            return new JObject();
        }
    }
}