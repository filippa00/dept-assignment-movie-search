namespace dept_assignment_movie_search.Models
{
    public class MovieData
    {
        public Guid Id { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? Country { get; set; }
        public List<Actor>? Actors { get; set; }
    }
}
