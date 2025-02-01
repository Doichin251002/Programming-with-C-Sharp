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

        public void Add(Movie? movie)
        {
            if (movie is null) return;

            if (!IsExistingActor(movie))
            {
                throw new KeyNotFoundException($"Actor with current ID does not exist");
            }

            bool IsExist = IsExistMovie(movie);

            if (IsExist)
            {
                throw new InvalidOperationException($"Movie with title {movie.Title} already exists.");
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

        public List<Movie> GetAll()
        {
            return _movieRepository.GetAllMovies();
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

            if (!IsExistingActor(movie))
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
                throw new KeyNotFoundException($"Movie with ID {id} does not exist");
            }

            _movieRepository.DeleteMovie(id);
        }

        private bool IsExistMovie(Movie? movie)
        {
            var allMovies = _movieRepository.GetAllMovies();

            if (allMovies is not null && allMovies.Any())
            {
                foreach (Movie m in allMovies)
                {
                    bool matchingTitle = m.Title == movie.Title;
                    bool matchingYear = m.Year == movie.Year;
                    bool matchingActor = IsExistingActor(movie);

                    if (matchingTitle && matchingYear && matchingActor)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsExistingActor(Movie movie)
        {
            var allActorIds = _actorRepository.GetAllActors().Select(a => a.Id).ToList();

            if (allActorIds is not null && allActorIds.Any())
            {
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
            }

            return false;
        }
    }
}
