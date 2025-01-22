using MovieStore.DL.Interfaces;
using MovieStore.Models.DTO;
using Moq;

namespace MovieStore.Tests.MovieTests
{
    public class MovieServiceTests
    {
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly Mock<IActorRepository> _actorRepositoryMock;
        private readonly BL.Services.MovieService _movieService;

        public MovieServiceTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _actorRepositoryMock = new Mock<IActorRepository>();
            _movieService = new BL.Services.MovieService(_movieRepositoryMock.Object, _actorRepositoryMock.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnAllMovies()
        {
            var movies = new List<Movie>
            {
                new Movie { Id = "1", Title = "Movie 1" },
                new Movie { Id = "2", Title = "Movie 2" }
            };

            _movieRepositoryMock.Setup(repo => repo.GetAllMovies()).Returns(movies);

            var result = _movieService.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, m => m.Title == "Movie 1");
        }

        [Fact]
        public void Add_ShouldAddMovie_WhenActorsExist()
        {
            var movie = new Movie { Id = "1", Title = "New Movie", Actors = new List<string> { "Actor1", "Actor2" } };

            _actorRepositoryMock.Setup(repo => repo.GetActorById(It.IsAny<string>())).Returns(new Actor { Id = "Actor1" });

            _movieService.Add(movie);

            _movieRepositoryMock.Verify(repo => repo.AddMovie(movie), Times.Once);
        }

        [Fact]
        public void Add_ShouldThrowException_WhenActorDoesNotExist()
        {
            var movie = new Movie { Id = "1", Title = "New Movie", Actors = new List<string> { "Actor1" } };

            _actorRepositoryMock.Setup(repo => repo.GetActorById("Actor1")).Returns((Actor)null);

            var exception = Assert.Throws<Exception>(() => _movieService.Add(movie));

            Assert.Equal("Actor with id Actor1 does not exist", exception.Message);
        }

        [Fact]
        public void GetById_ShouldReturnMovie_WhenMovieExists()
        {
            var movie = new Movie { Id = "1", Title = "Movie 1" };

            _movieRepositoryMock.Setup(repo => repo.GetMovieById(movie.Id)).Returns(movie);

            var result = _movieService.GetById(movie.Id);

            Assert.NotNull(result);
            Assert.Equal(movie.Id, result.Id);
        }

        [Fact]
        public void GetById_ShouldThrowException_WhenMovieIdIsNullOrEmpty()
        {
            var exception = Assert.Throws<ArgumentException>(() => _movieService.GetById(string.Empty));

            Assert.Equal("Movie ID cannot be null or empty. (Parameter 'id')", exception.Message);
        }

        [Fact]
        public void GetById_ShouldThrowException_WhenMovieDoesNotExist()
        {
            _movieRepositoryMock.Setup(repo => repo.GetMovieById("1")).Returns((Movie)null);

            var exception = Assert.Throws<KeyNotFoundException>(() => _movieService.GetById("1"));

            Assert.Equal("Movie with ID 1 not found.", exception.Message);
        }

        [Fact]
        public void Delete_ShouldDeleteMovie_WhenMovieExists()
        {
            var movie = new Movie { Id = "1", Title = "Movie 1" };

            _movieRepositoryMock.Setup(repo => repo.GetMovieById(movie.Id)).Returns(movie);

            _movieService.Delete(movie.Id);

            _movieRepositoryMock.Verify(repo => repo.DeleteMovie(movie.Id), Times.Once);
        }

        [Fact]
        public void Delete_ShouldThrowException_WhenMovieDoesNotExist()
        {
            _movieRepositoryMock.Setup(repo => repo.GetMovieById("1")).Returns((Movie)null);

            var exception = Assert.Throws<Exception>(() => _movieService.Delete("1"));

            Assert.Equal("Movie with id 1 does not exist", exception.Message);
        }

        [Fact]
        public void Update_ShouldUpdateMovie_WhenMovieExists()
        {
            var movie = new Movie { Id = "1", Title = "Updated Movie" };

            _movieRepositoryMock.Setup(repo => repo.GetMovieById(movie.Id)).Returns(movie);

            _movieService.Update(movie);

            _movieRepositoryMock.Verify(repo => repo.UpdateMovie(movie), Times.Once);
        }

        [Fact]
        public void Update_ShouldThrowException_WhenMovieIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _movieService.Update(null));

            Assert.Equal("Movie cannot be null (Parameter 'movie')", exception.Message);
        }

        [Fact]
        public void Update_ShouldThrowException_WhenMovieIdIsNullOrEmpty()
        {
            var movie = new Movie { Id = string.Empty, Title = "Invalid Movie" };

            var exception = Assert.Throws<ArgumentException>(() => _movieService.Update(movie));

            Assert.Equal("Movie ID cannot be null or empty (Parameter 'Id')", exception.Message);
        }

        [Fact]
        public void Update_ShouldThrowException_WhenMovieDoesNotExist()
        {
            var movie = new Movie { Id = "1", Title = "Non-existent Movie" };

            _movieRepositoryMock.Setup(repo => repo.GetMovieById(movie.Id)).Returns((Movie)null);

            var exception = Assert.Throws<KeyNotFoundException>(() => _movieService.Update(movie));

            Assert.Equal("Movie with ID 1 does not exist", exception.Message);
        }
    }
}
