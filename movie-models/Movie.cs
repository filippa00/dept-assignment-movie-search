using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movie_models
{
    public class Movie
    {
        public string? Title { get; set; }
        public int RealeaseDate { get; set; }
        public string? Country { get; set; }
        public string? Image { get; set; }
        public List<Actor>? Actors { get; set; }

    }
}
