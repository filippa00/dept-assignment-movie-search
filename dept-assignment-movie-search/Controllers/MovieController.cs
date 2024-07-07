using dept_assignment_movie_search.Models;
using dept_assignment_movie_search.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Encodings.Web;

namespace dept_assignment_movie_search.Controllers
{
    public class MovieController : Controller
    {
        private readonly ILogger<MovieController> _logger;
        public IConfiguration _configuration;
        private MovieService _movieService;

        public MovieController(ILogger<MovieController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _movieService = new MovieService(configuration);
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
                return RedirectToAction("Index");
            }
            var sanitizedSearchTerm = HtmlEncoder.Default.Encode(searchTerm);
            try
            {
                var movies = await _movieService.GetMoviesAsync(sanitizedSearchTerm);
                return View("Index", movies);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
          
        }

        [HttpPost]
        public ActionResult MovieDetails(Movie movie)
        {
            if (movie == null )
            {
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}