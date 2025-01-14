using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieStore.DL.Interfaces;
using MovieStore.Models.Configurations;
using MovieStore.Models.DTO;

namespace MovieStore.DL.Repositories.MongoRepositories
{
    public class ActorRepository : IActorRepository
    {
        private readonly IMongoCollection<Actor> _actors;
        private readonly ILogger<ActorRepository> _logger;

        public ActorRepository(
            IOptionsMonitor<MongoDbConfiguration> mongoConfig,
            ILogger<ActorRepository> logger)
        {
            _logger = logger;
            var client = new MongoClient(
                mongoConfig.CurrentValue.ConnectionString);

            var database = client.GetDatabase(
                mongoConfig.CurrentValue.DatabaseName);

            _actors = database.GetCollection<Actor>(
                $"{nameof(Actor)}s");
        }

        public void AddActor(Actor actor)
        {
            actor.Id = System.Guid.NewGuid().ToString();
            _actors.InsertOne(actor);
        }

        public void AddMovie(Actor movie)
        {
            if (movie == null)
            {
                _logger.LogError("Movie is null");
                return;
            }

            try
            {
                movie.Id = Guid.NewGuid().ToString();

                _actors.InsertOne(movie);
            }
            catch (Exception e)
            {
               _logger.LogError(e,
                   $"Error adding movie {e.Message}-{e.StackTrace}");
            }
           
        }


        public IEnumerable<Actor> GetActorsByIds(IEnumerable<string> actorsIds)
        {
            var result = _actors.Find(actor => actorsIds.Contains(actor.Id)).ToList();
            return result;
        }

        public Actor? GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            return _actors.Find(m => m.Id == id)
                .FirstOrDefault();
        }
    }
}
