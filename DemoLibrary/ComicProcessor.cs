using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoLibrary
{
    public class ComicProcessor
    {

        public async Task<ComicModel> LoadComic(int comicNumber=0)
        {
            // 0 will equate to the current comic
            string url = "";

            if (comicNumber > 0)
            {
                url = $"https://xkcd.com/{comicNumber}/info.0.json";
            } else
            {

                url = $"https://xkcd.com/info.0.json";
            }

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode) 
                {
                    ComicModel comic = await response.Content.ReadAsAsync<ComicModel>();

                    return comic;

                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

        }
    }
}
