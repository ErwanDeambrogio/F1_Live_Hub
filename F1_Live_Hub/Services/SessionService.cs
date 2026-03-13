using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using F1_Live_Hub.Models;

namespace F1_Live_Hub.Services
{
    internal class SessionService
    {
        public async Task<List<SessionRoot>> GetSessionDetails()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10);
                HttpResponseMessage response = await client.GetAsync(
                    "\"https://api.openf1.org/v1/sessions?year=2025\"");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    List<SessionRoot> sessions = JsonConvert.DeserializeObject<List<SessionRoot>>(content);
                    return sessions;
                }
                else
                {
                    var tt = "error";
                    Console.WriteLine($"{tt} : {(int)response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // ← Va afficher l'erreur exacte dans la console Visual Studio
                Console.WriteLine($"EXCEPTION : {ex.Message}");
                return null;
            }
        }
    }
}