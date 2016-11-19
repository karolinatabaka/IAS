using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProviderTwo;
using FizzWare.NBuilder;

namespace ProviderOne
{
    public class ProviderTwoApi
    {
        private MongoClient client = new MongoClient("mongodb://localhost:27017");
        private IMongoDatabase database;
        public ProviderTwoApi()
        {
            database = client.GetDatabase("MoviesDB104");

            GenerateData();
        }

        public IEnumerable<Movie> GetMovies() {
            var collection = database.GetCollection<Movie>("Movies");
            return collection.AsQueryable().ToList();
        }

        public Movie GetMovie(int id)
        {

            var collection = database.GetCollection<Movie>("Movies");
            return collection.Find(x=>x.Identifier == id).FirstOrDefault();
        }

        private void GenerateData() {
            var movies = GetMovies();
            if (!movies.Any())
            {
                var generator = new RandomGenerator();
                var idPool = new List<int>();
                for(var i = 1; i < 100; i++)
                {
                    idPool.Add(i);
                }

                var generatedMovieList = Builder<Movie>.CreateListOfSize(50).All()
                    .With(x => x.PublishYear = generator.Next(1950, DateTime.Now.Year))
                .And(x => x.PublishMonth = generator.Next(1, DateTime.Now.Month))
                .And(x => x.PublishDay = generator.Next(1, DateTime.Now.Day))
                .And(x=>x.Identifier = IncrementId(ref idPool))
                .Build();

                var collection = database.GetCollection<Movie>("Movies");

                foreach(var movie in generatedMovieList)
                {
                    collection.InsertOne(movie);
                }

            }

        }

        private int IncrementId(ref List<int> idPool)
        {
            var result = 0;
            if (idPool != null && idPool.Any())
            {
                result = idPool.First();
                idPool.RemoveAt(0);
            }

            return result; 
        }

    }
}
