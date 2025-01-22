using MovieStore.BL.Interfaces;
using MovieStore.DL.Interfaces;
using MovieStore.Models.DTO;

namespace MovieStore.BL.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IActorRepository _actorRepository;

        public MovieService(IMovieRepository movieRepository, IActorRepository actorRepository)
        {
            _movieRepository = movieRepository;
            _actorRepository = actorRepository;
        }
        
        public List<Movie> GetAll()
        {
            return _movieRepository.GetAllMovies();
        }

        public void Add(Movie? movie)
        {
            if (movie is null ) return;

            foreach (var movieActor in movie.Actors)
            {
                var actor = _actorRepository.GetActorById(movieActor);

                if (actor is null)
                {
                    throw new Exception(
                        $"Actor with id {movieActor} does not exist");
                }
            }

            _movieRepository.AddMovie(movie);
        }

        public Movie? GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Movie ID cannot be null or empty.", nameof(id));
            }

            var movie = _movieRepository.GetMovieById(id);

            if (movie == null)
            {
                throw new KeyNotFoundException($"Movie with ID {id} not found.");
            }

            return movie;
        }

        public void Update(Movie movie)
        {
            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie), "Movie cannot be null");
            }

            if (string.IsNullOrEmpty(movie.Id))
            {
                throw new ArgumentException("Movie ID cannot be null or empty", nameof(movie.Id));
            }

            var existingMovie = _movieRepository.GetMovieById(movie.Id);

            if (existingMovie == null)
            {
                throw new KeyNotFoundException($"Movie with ID {movie.Id} does not exist");
            }

            if (!existingActor(movie))
            {
                throw new KeyNotFoundException($"Actors with IDs not found.");
            }

            _movieRepository.UpdateMovie(movie);

        }

        public void Delete(string id)
        {
            var movie = _movieRepository.GetMovieById(id);

            if (movie is null)
            {
                throw new Exception($"Movie with id {id} does not exist");
            }

            _movieRepository.DeleteMovie(id);
        }

        private bool existingActor(Movie movie)
        {
            var allActorIds = _actorRepository.GetAllActors().Select(a => a.Id).ToList();

            foreach (var actorId in allActorIds)
            {
                foreach (var movieActorId in movie.Actors)
                {
                    if (movieActorId.Equals(actorId))
                    {
                        return true;
                    }
                }

            }

            return false;
        }

    }
}
