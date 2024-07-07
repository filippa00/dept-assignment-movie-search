using dept_assignment_movie_search.Models;
using dept_assignment_movie_search.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace dept_assignment_movie_search.Controllers
{
    public class MovieController : Controller
    {
        private readonly ILogger<MovieController> _logger;
        public IConfiguration _configuration;

        public MovieController(ILogger<MovieController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return View("Index");
            }
            MovieService movieService = new MovieService(_configuration);
            var movies = await movieService.GetMoviesAsync(searchTerm);
            return View("Index",movies);
        }

        [HttpPost]
        public ActionResult MovieDetails(Movie movie)
        {
            if (movie == null )
            {
                return View("Index");
            }

            return View(movie);
        }
    }
}