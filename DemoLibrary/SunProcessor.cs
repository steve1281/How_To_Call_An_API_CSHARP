using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoLibrary
{
    public class SunProcessor
    {
        public async Task<SunResultModel> LoadSunInformation()
        {
            string url = "https://api.sunrise-sunset.org/json?lat=48.4463684&lng=-89.2712642&date=today";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    SunModel result = await response.Content.ReadAsAsync<SunModel>();

                    return result.Results;

                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

        }
    }
}


