using dept_assignment_movie_search.Models;
using dept_assignment_movie_search.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace dept_assignment_movie_search.Controllers
{
    public class MovieController : Controller
    {
        private readonly ILogger<MovieController> _logger;

        public MovieController(ILogger<MovieController> logger)
        {
            _logger = logger;
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
            MovieService movieService = new MovieService();
            var movies = await movieService.GetMoviesAsync(searchTerm);
            return View("Index",movies);
        }
        /*[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }*/
    }
}