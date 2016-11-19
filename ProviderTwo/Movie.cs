using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProviderTwo
{
    public class Movie
    {
        [BsonId]
        public MongoDB.Bson.ObjectId Id { get; set; } 

        public int Identifier { get; set; }

        public string Title { get; set; }

        public string Description_part1 { get; set; }

        public string Description_part2 { get; set; }

        public int PublishYear { get; set; }

        public int PublishMonth { get; set; }

        public int PublishDay { get; set; }

    }
}
