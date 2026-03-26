using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace F1_Live_Hub.Services
{
    public class FavoritesService
    {
        private readonly string _filePath;
        private List<string> _favoriteIds;

        public FavoritesService()
        {
            _filePath = Path.Combine(
                System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.ApplicationData),
                "F1LiveHub", "favorites.json");

            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
            Load();
        }

        private void Load()
        {
            try
            {
                if (File.Exists(_filePath))
                    _favoriteIds = JsonConvert.DeserializeObject<List<string>>(
                        File.ReadAllText(_filePath)) ?? new List<string>();
                else
                    _favoriteIds = new List<string>();
            }
            catch { _favoriteIds = new List<string>(); }
        }

        private void Save()
        {
            try
            {
                File.WriteAllText(_filePath,
                JsonConvert.SerializeObject(_favoriteIds));
            }
            catch { }
        }

        public bool IsFavorite(string driverId)
            => _favoriteIds.Contains(driverId);

        public void Toggle(string driverId)
        {
            if (_favoriteIds.Contains(driverId))
                _favoriteIds.Remove(driverId);
            else
                _favoriteIds.Add(driverId);
            Save();
        }

        public List<string> GetAll() => new List<string>(_favoriteIds);
    }
}