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
                _logger.LogError("Attempted to add a null movie.");
                return;
            }

            if (string.IsNullOrWhiteSpace(movie.Title))
            {
                _logger.LogError("Attempted to add a movie with an empty or null title.");
                return;
            }

            if (movie.Year <= 0)
            {
                _logger.LogError("Attempted to add a movie with an invalid year.");
                return;
            }

            try
            {
                movie.Id = Guid.NewGuid().ToString();
                _movies.InsertOne(movie);
                _logger.LogInformation($"Successfully added movie with ID: {movie.Id}, Title: {movie.Title}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding movie. Title: {movie.Title}. Exception: {ex.Message}");
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
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Movie ID cannot be null or empty", nameof(id));
            }

            try
            {
                var filter = Builders<Movie>.Filter.Eq(m => m.Id, id);

                var deletedMovie = _movies.FindOneAndDelete(filter);

                if (deletedMovie != null)
                {
                    _logger.LogInformation($"Successfully deleted movie with ID: {id}");
                }
                else
                {
                    _logger.LogWarning($"Movie with ID: {id} was not found in the database.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting movie with ID: {id}");
            }
        }

    }
}
