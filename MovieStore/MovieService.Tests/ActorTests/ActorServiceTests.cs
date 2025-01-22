using Moq;
using MovieStore.Models.DTO;
using MovieStore.BL.Services;
using MovieStore.DL.Interfaces;

namespace MovieStore.Tests.ActorTests
{
    public class ActorServiceTests
    {
        private readonly Mock<IActorRepository> _actorRepositoryMock;
        private readonly ActorService _actorService;

        public ActorServiceTests()
        {
            _actorRepositoryMock = new Mock<IActorRepository>();
            _actorService = new ActorService(_actorRepositoryMock.Object);
        }

        [Fact]
        public void Add_ShouldAddActor()
        {
            var actor = new Actor { Id = "1", Name = "John Doe" };
            _actorRepositoryMock.Setup(repo => repo.GetActorById(actor.Id)).Returns((Actor)null);

            _actorService.Add(actor);

            _actorRepositoryMock.Verify(repo => repo.AddActor(It.Is<Actor>(a => a.Id == actor.Id && a.Name == actor.Name)), Times.Once);
        }

        [Fact]
        public void Add_ShouldThrowException_WhenActorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _actorService.Add(null));
        }

        [Fact]
        public void Add_ShouldThrowException_WhenActorAlreadyExists()
        {
            var actor = new Actor { Id = "1", Name = "John Doe" };
            _actorRepositoryMock.Setup(repo => repo.GetActorById(actor.Id)).Returns(actor);

            var exception = Assert.Throws<InvalidOperationException>(() => _actorService.Add(actor));

            Assert.Equal($"Actor with ID {actor.Id} already exists.", exception.Message);
        }

        [Fact]
        public void GetById_ShouldReturnActor_WhenActorExists()
        {
            var actor = new Actor { Id = "1", Name = "John Doe" };
            _actorRepositoryMock.Setup(repo => repo.GetActorById(actor.Id)).Returns(actor);

            var result = _actorService.GetById(actor.Id);

            Assert.NotNull(result);
            Assert.Equal(actor.Id, result.Id);
        }

        [Fact]
        public void GetById_ShouldThrowException_WhenActorDoesNotExist()
        {
            var actorId = "1";
            _actorRepositoryMock.Setup(repo => repo.GetActorById(actorId)).Returns((Actor)null);

            var exception = Assert.Throws<KeyNotFoundException>(() => _actorService.GetById(actorId));

            Assert.Equal($"Actor with ID {actorId} not found.", exception.Message);
        }

        [Fact]
        public void GetByIds_ShouldReturnActors_WhenActorsExist()
        {
            var actorIds = new List<string> { "1", "2" };
            var actors = new List<Actor>
            {
                new Actor { Id = "1", Name = "John Doe" },
                new Actor { Id = "2", Name = "Jane Smith" }
            };

            _actorRepositoryMock.Setup(repo => repo.GetActorsByIds(actorIds)).Returns(actors);

            var result = _actorService.GetByIds(actorIds);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetByIds_ShouldThrowException_WhenNoActorsExist()
        {
            var actorIds = new List<string> { "1", "2" };
            _actorRepositoryMock.Setup(repo => repo.GetActorsByIds(actorIds)).Returns(new List<Actor>());

            var exception = Assert.Throws<KeyNotFoundException>(() => _actorService.GetByIds(actorIds));

            Assert.Equal("No actors found with the provided IDs.", exception.Message);
        }

        [Fact]
        public void GetAll_ShouldReturnAllActors()
        {
            var actors = new List<Actor>
            {
                new Actor { Id = "1", Name = "John Doe" },
                new Actor { Id = "2", Name = "Jane Smith" }
            };

            _actorRepositoryMock.Setup(repo => repo.GetAllActors()).Returns(actors);

            var result = _actorService.GetAll();

            Assert.NotNull(result);
            Assert.Equal(actors.Count, result.Count);
        }

        [Fact]
        public void Update_ShouldUpdateActor_WhenActorExists()
        {
            var actor = new Actor { Id = "1", Name = "John Doe" };
            _actorRepositoryMock.Setup(repo => repo.GetActorById(actor.Id)).Returns(actor);

            actor.Name = "John Updated";
            _actorService.Update(actor);

            _actorRepositoryMock.Verify(repo => repo.UpdateActor(It.Is<Actor>(a => a.Id == actor.Id && a.Name == "John Updated")), Times.Once);
        }

        [Fact]
        public void Update_ShouldThrowException_WhenActorDoesNotExist()
        {
            var actor = new Actor { Id = "1", Name = "John Doe" };
            _actorRepositoryMock.Setup(repo => repo.GetActorById(actor.Id)).Returns((Actor)null);

            var exception = Assert.Throws<KeyNotFoundException>(() => _actorService.Update(actor));

            Assert.Equal($"Actor with ID {actor.Id} not found.", exception.Message);
        }

        [Fact]
        public void Delete_ShouldDeleteActor_WhenActorExists()
        {
            var actor = new Actor { Id = "1", Name = "John Doe" };
            _actorRepositoryMock.Setup(repo => repo.GetActorById(actor.Id)).Returns(actor);

            _actorService.Delete(actor.Id);

            _actorRepositoryMock.Verify(repo => repo.DeleteActor(It.Is<string>(id => id == actor.Id)), Times.Once);
        }

        [Fact]
        public void Delete_ShouldThrowException_WhenActorDoesNotExist()
        {
            var actorId = "1";
            _actorRepositoryMock.Setup(repo => repo.GetActorById(actorId)).Returns((Actor)null);

            var exception = Assert.Throws<KeyNotFoundException>(() => _actorService.Delete(actorId));

            Assert.Equal($"Actor with ID {actorId} not found.", exception.Message);
        }
    }
}
