using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieStore.DL.Interfaces;
using MovieStore.Models.Configurations;
using MovieStore.Models.DTO;

namespace MovieStore.DL.Repositories.MongoRepositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IMongoCollection<Movie> _movies;
        private readonly ILogger<MovieRepository> _logger;

        public MovieRepository(
            IOptionsMonitor<MongoDbConfiguration> mongoConfig,
            ILogger<MovieRepository> logger)
        {
            _logger = logger;
            var client = new MongoClient(
                mongoConfig.CurrentValue.ConnectionString);

            var database = client.GetDatabase(
                mongoConfig.CurrentValue.DatabaseName);

            _movies = database.GetCollection<Movie>(
                $"{nameof(Movie)}s");
        }

        public void AddMovie(Movie movie)
        {
            if (movie == null)
            {
                _logger.LogError("Movie is null");
                return;
            }

            try
            {
                movie.Id = Guid.NewGuid().ToString();

                _movies.InsertOne(movie);
            }
            catch (Exception e)
            {
               _logger.LogError(e,
                   $"Error adding movie {e.Message}-{e.StackTrace}");
            }
           
        }

        public Movie? GetMovieById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            return _movies.Find(m => m.Id.Equals(id))
                .FirstOrDefault();
        }

        public List<Movie> GetAllMovies()
        {
            return _movies.Find(movie => true).ToList();
        }

        public void UpdateMovie(Movie movie)
        {
            if (movie == null)
            {
                _logger.LogError("Movie is null");
                return;
            }
            else if (string.IsNullOrEmpty(movie.Id))
            {
                string id = movie.Id;
                _logger.LogError($"Movie with id {id} not found");
                return;
            }

            var filter = Builders<Movie>.Filter.Eq(m => m.Id, movie.Id);

            try
            {
                var update = Builders<Movie>.Update
                    .Set(m => m.Title, movie.Title)
                    .Set(m => m.Year, movie.Year)
                    .Set(m => m.Actors, movie.Actors);

                var result = _movies.UpdateOne(filter, update);

                if (result.ModifiedCount > 0)
                {
                    _logger.LogInformation($"Successfully updated movie with id {movie.Id}");
                }
                else
                {
                    _logger.LogWarning($"No changes made to movie with id {movie.Id}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error updating movie {movie.Id}: {e.Message} - {e.StackTrace}");
            }
        }


        public void DeleteMovie(string id)
        {
            if (string.IsNullOrEmpty(id)) return;

            _movies.FindOneAndDelete(m => m.Id.Equals(id));
        }
    }
}
