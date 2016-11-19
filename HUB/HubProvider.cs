using ProviderOne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HUB
{
    public class HubProvider
    {
        private ProviderOneApi providerOneApi = new ProviderOneApi();
        private ProviderTwoApi providerTwoApi = new ProviderTwoApi();

        public IEnumerable<HubMovie> GetMovies()
        {
            var moviesFromProviderOne = providerOneApi.GetMovies().Select(x=> new HubMovie() {
                Id = x.Id,
                Description = x.ShortDescription.Trim() + " " + x.Genre,
                PublishDate = x.PublishDate,
                Name = x.Name,
                Provider = Provider.ProviderOne
            });

            var moviesFromProviderTwo = providerTwoApi.GetMovies().Select(x=> new HubMovie()
            {
                Id = x.Identifier,
                Description = x.Description_part1 + " " + x.Description_part2,
                PublishDate = new DateTime(x.PublishYear, x.PublishMonth, x.PublishDay),
                Name = x.Title,
                Provider = Provider.ProviderTwo
            });

            return moviesFromProviderOne.Union(moviesFromProviderTwo);

        }

        public HubMovie GetMovie(int id, Provider provider)
        {
            switch (provider)
            {
                case Provider.ProviderOne:
                    var movieFromOne = providerOneApi.GetMovie(id);
                    if(movieFromOne != null)
                    {
                        return new HubMovie()
                        {
                            Id = movieFromOne.Id,
                            Description = movieFromOne.ShortDescription + " " + movieFromOne.Genre,
                            PublishDate = movieFromOne.PublishDate,
                            Name = movieFromOne.Name,
                            Provider = provider
                        };
                    }
                    break;
                case Provider.ProviderTwo:
                    var movieFromTwo = providerTwoApi.GetMovie(id);
                    if (movieFromTwo != null)
                    {
                        return new HubMovie()
                        {
                            Id = movieFromTwo.Identifier,
                            Description = movieFromTwo.Description_part1 + " " + movieFromTwo.Description_part2,
                            PublishDate = new DateTime(movieFromTwo.PublishYear, movieFromTwo.PublishMonth, movieFromTwo.PublishDay),
                            Name = movieFromTwo.Title,
                            Provider = provider
                        };
                    }
                    break;
            }

            return null;
        }

    }
}
