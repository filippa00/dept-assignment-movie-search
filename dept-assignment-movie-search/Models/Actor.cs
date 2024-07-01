namespace dept_assignment_movie_search.Models
{
    public class Actor
    {
        private readonly string? firstname;
        private readonly string? lastname;  
        public string Fullname { get { return $"{firstname} {lastname}"; } }
        public string? Image { get; set; }
    }
}
