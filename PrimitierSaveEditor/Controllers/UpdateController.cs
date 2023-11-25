using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierSaveEditor.Controllers
{
    public static class UpdateController
    {
        static HttpClient http = new();
        const string releasesUrl = "https://api.github.com/repos/Seva167/Primitedit/releases";

        public static async Task<UpdateInfo> GetUpdateInfo()
        {
            try
            {
                HttpRequestMessage req = new(HttpMethod.Get, releasesUrl);
                req.Headers.Add("user-agent", "Primitedit");

                HttpResponseMessage res = await http.SendAsync(req);
                if (!res.IsSuccessStatusCode)
                    return null;
                string body = await res.Content.ReadAsStringAsync();
                UpdateInfo[] info = JsonConvert.DeserializeObject<UpdateInfo[]>(body);

                return info.First();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public class UpdateInfo
        {
            [JsonProperty("html_url")]
            public string Url { get; set; }

            [JsonProperty("tag_name")]
            public string Tag { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("body")]
            public string Body { get; set; }

            public bool IsNewerVersion()
            {
                Version curVer = Assembly.GetExecutingAssembly().GetName().Version;
                Version newVer = new(Tag.TrimStart('V', 'v'));

                return newVer > curVer;
            }
        }
    }
}
