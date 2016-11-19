using HUB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MovieLib.Controllers
{
    [RoutePrefix("api/movies")]
    public class MoviesController : ApiController
    {
        private HubProvider hubProviderApi = new HubProvider();

        // GET api/movies
        [Route("")]
        public IEnumerable<HubMovie> Get()
        {
            var result = hubProviderApi.GetMovies();
            return result;
        }

        // GET api/movies/1/5
        [Route("{provider}/{id}")]
        public HubMovie Get(Provider provider, int id)
        {
            return hubProviderApi.GetMovie(id, provider);
        }
    }
}
