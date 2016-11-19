using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Webpage.Model;

namespace Webpage.Controllers
{
    public class HomeController : Controller
    {
        private string apiUrl = "http://localhost:59247/api";
        private string movieListUrl = "/movies";
        private string movieDetails = "/movies/{0}/{1}";

        public async Task<ActionResult> Index()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl + movieListUrl);
            using (var client = InitHttpClient())
            {
                var data = await client.SendAsync(request);

                var currentResponse = JsonConvert.DeserializeObject<List<HubMovie>>(data.Content.ReadAsStringAsync().Result);
                return View(currentResponse);
            }
        }

        public async Task<ActionResult> Details(Provider provider, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl + string.Format(movieDetails, provider, id));
            using (var client = InitHttpClient())
            {
                var data = await client.SendAsync(request);

                var currentResponse = JsonConvert.DeserializeObject<HubMovie>(data.Content.ReadAsStringAsync().Result);
                return View(currentResponse);
            }
        }


        private HttpClient InitHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(apiUrl);

            return httpClient;
        }

    }
}