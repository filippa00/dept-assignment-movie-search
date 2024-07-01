namespace dept_assignment_movie_search.Models
{
    public class Movie
    {
        public string? Title { get; set;}
        public int RealeaseDate { get; set;}
        public string? Country { get; set;}
        public string? Image { get; set;}
        public List<Actor>? Actors { get; set;}

    }
}
