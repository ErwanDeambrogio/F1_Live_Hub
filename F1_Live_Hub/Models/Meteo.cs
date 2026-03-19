using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace F1_Live_Hub.Models
{
    public class Meteo
    {
        public async Task<string> GetBlogdetails()
        {
            try
            {
                // Assure TLS 1.2 sur .NET Framework si nécessaire
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(15);
                    HttpResponseMessage response = await client.GetAsync("https://api.openf1.org/v1/weather");
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
            }
            catch (HttpRequestException ex)
            {
                return "HttpRequestException: " + ex.Message;
            }
            catch (TaskCanceledException ex)
            {
                return "Timeout or cancelled: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Exception: " + ex.Message;
            }
        }
    }
}


