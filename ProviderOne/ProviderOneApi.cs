using FizzWare.NBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProviderOne
{
    public class ProviderOneApi
    {
        private Entities movieEntities = new Entities();

        public ProviderOneApi()
        {
            GenerateData();
        }

        public IEnumerable<Movie> GetMovies()
        {
            return movieEntities.Movies.ToList();
        }

        public Movie GetMovie(int id)
        {
            return movieEntities.Movies.FirstOrDefault(x => x.Id == id);
        }

        private void GenerateData()
        {
            var movies = GetMovies();
            if (!movies.Any())
            {
                var generator = new RandomGenerator();

                var generatedMovieList = Builder<Movie>.CreateListOfSize(50).All()
                    .With(x => x.PublishDate = generator.Next(new DateTime(1950, 1, 1), DateTime.Now.AddDays(-1)))
                .Build();

                foreach (var movie in generatedMovieList)
                {
                    movieEntities.Movies.Add(movie);
                }

                movieEntities.SaveChanges();
            }
        }
    }
}
