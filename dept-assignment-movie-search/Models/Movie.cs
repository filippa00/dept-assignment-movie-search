﻿namespace dept_assignment_movie_search.Models
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? Country { get; set; }
        public List<Actor>? Actors { get; set; }

    }
}
