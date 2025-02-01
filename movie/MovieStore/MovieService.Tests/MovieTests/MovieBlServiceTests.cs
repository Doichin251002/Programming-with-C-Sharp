using Moq;
using MovieStore.BL.Interfaces;
using MovieStore.BL.Services;
using MovieStore.DL.Interfaces;
using MovieStore.Models.DTO;

namespace MovieService.Tests.MovieTests
{
    public class MovieBlServiceTests
    {
        private Mock<IMovieService> _movieServiceMock;
        private Mock<IActorRepository> _actorRepositoryMock;

        public MovieBlServiceTests()
        {
            _movieServiceMock = new Mock<IMovieService>();
            _actorRepositoryMock = new Mock<IActorRepository>();
        }

        private List<Movie> _movies = new List<Movie>
        {
            new Movie()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Movie 1",
                Year = 2021,
                Actors = ["Actor 1", "Actor 2"]
            },
            new Movie()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Movie 2",
                Year = 2022,
                Actors = ["Actor 3", "Actor 4"]
            }
        };

        private List<Actor> _actors = new List<Actor>
        {
            new Actor()
            {
                Id = "157af604-7a4b-4538-b6a9-fed41a41cf3a",
                Name = "Actor 1"
            },
            new Actor()
            {
                Id = "baac2b19-bbd2-468d-bd3b-5bd18aba98d7",
                Name = "Actor 2"
            },
            new Actor()
            {
                Id = "5c93ba13-e803-49c1-b465-d471607e97b3",
                Name = "Actor 3"
            },
            new Actor()
            {
                Id = "9badefdc-0714-4581-80ae-161cd0a5abbe",
                Name = "Actor 4"
            }
        };

        [Fact]
        public void GetDetailedMovies_Ok()
        {
            //setup
            var expectedCount = 2;

            _movieServiceMock
                .Setup(x => x.GetAll())
                .Returns(_movies);
            _actorRepositoryMock.Setup(x =>
                    x.GetActorById(It.IsAny<string>()))
                .Returns((string id) =>
                    _actors.FirstOrDefault(x => x.Id == id));

            //inject
            var movieBlService = new MovieBlService(
                _movieServiceMock.Object,
                _actorRepositoryMock.Object);

            //Act
            var result =
                movieBlService.GetDetailedMovies();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCount, result.Count);
        }

    }
}