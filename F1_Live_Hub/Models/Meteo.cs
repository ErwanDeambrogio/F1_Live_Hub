using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace F1_Live_Hub.Models
{
    internal class Meteo
    {
     
        


public async Task<string> GetBlogdetails()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync( "https://api.openf1.org/v1/weather");
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            else
            {
                return "Error: " + response.StatusCode;
            }


        }
    

}
}


