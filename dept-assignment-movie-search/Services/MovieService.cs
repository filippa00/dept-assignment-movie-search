using dept_assignment_movie_search.Models;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;

namespace dept_assignment_movie_search.Services
{
    public class MovieService
    {
        public IConfiguration _configuration;

        public MovieService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync(string searchTerm)
        {
           JObject jsonMovieArray = await RequestDataAsync(searchTerm);
           return ProcessData(jsonMovieArray);
        }

        private async Task<JObject> RequestDataAsync(string searchTerm)
        {
            JObject jsonMovieObject = new JObject();
            try
            {
                string apiKey = _configuration["apikeysettings:x-rapidapi-key"];
                string apiHost = _configuration["apikeysettings:x-rapidapi-host"];
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://{apiHost}/v2/search?searchTerm={searchTerm}&type=MOVIE&first=20"),
                    Headers =
                    {
                        { "x-rapidapi-key", apiKey},
                        { "x-rapidapi-host", apiHost},
                    },
                                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    jsonMovieObject = JObject.Parse(body);
                    return jsonMovieObject;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"Argument null error: {e.Message}");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine($"Null reference error: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }


            return jsonMovieObject;
        }

        private List<Movie> ProcessData(JObject jsonMovieObject)
        {
            List<Movie> movieList = new List<Movie>();
            try
            {
                var jsonMovie = jsonMovieObject["data"]?["mainSearch"]?["edges"];
                if (jsonMovie != null)
                {
                    foreach (var item in jsonMovie)
                    {
                        var titleText = item["node"]?["entity"]?["titleText"];
                        var releaseDate = item["node"]?["entity"]?["releaseDate"];
                        var primaryImage = item["node"]?["entity"]?["primaryImage"];
                        var creditsList = item["node"]?["entity"]?["principalCredits"]; //go throgh aray name and primary image

                        var movieDataReleaseMonth = (int?)releaseDate?["month"] ?? default(int);
                        var movieDataReleaseDay = (int?)releaseDate?["day"] ?? default(int);
                        var movieDataReleaseYear = (int?)releaseDate?["year"] ?? default(int);
                        DateTime dateTime = new DateTime(movieDataReleaseYear, movieDataReleaseMonth, movieDataReleaseDay);

                        if (titleText != null && releaseDate != null && primaryImage != null && creditsList != null)
                        {
                            Movie movie = CreateMovie(titleText, releaseDate, primaryImage, creditsList, dateTime);
                            movieList.Add(movie);
                        }    
                    }

                    return movieList;
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"Argument null error: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }

            return movieList;
        }

        private Movie CreateMovie(JToken titleText, JToken releaseDate, JToken primaryImage, JToken creditsList,DateTime dateTime )
        {
            Movie movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = (string?)titleText["text"],
                Image = (string?)primaryImage["url"],
                Country = (string?)releaseDate["country"]?["id"],
                ReleaseDate = dateTime,

            };

            List<Actor> actorList = new List<Actor>();

            foreach (var credit in creditsList)
            {
                foreach(var creditActor in credit["credits"])
                {
                    Actor actor = new Actor
                    {
                        Id = Guid.NewGuid(),
                        Name =  (string?)creditActor["name"]?["nameText"]?["text"],
                        Image = (string?)creditActor["name"]?["primaryImage"]?["url"]
                    };
                    actorList.Add(actor);
                    
                }
                break;
            }

            movie.Actors = actorList;

            return movie;
        }
    }
}
