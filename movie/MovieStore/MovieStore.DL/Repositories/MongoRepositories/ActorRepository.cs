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
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor), "Actor cannot be null.");
            }

            if (string.IsNullOrEmpty(actor.Name))
            {
                throw new ArgumentException("Actor name cannot be empty.", nameof(actor.Name));
            }

            actor.Id = System.Guid.NewGuid().ToString();

            try
            {
                _actors.InsertOne(actor);
                _logger.LogInformation($"Actor with ID {actor.Id} successfully added.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while adding the actor: {ex.Message}");
                return;
            }
        }

        public Actor? GetActorById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            return _actors.Find(a => a.Id == id)
                .FirstOrDefault();
        }

        public IEnumerable<Actor> GetActorsByIds(IEnumerable<string> actorsIds)
        {
            if (actorsIds == null || !actorsIds.Any())
            {
                throw new ArgumentException("Actor IDs cannot be null or empty.", nameof(actorsIds));
            }

            var filter = Builders<Actor>.Filter.In(a => a.Id, actorsIds);
            var actors = _actors.Find(filter).ToList();

            if (actors == null || !actors.Any())
            {
                throw new KeyNotFoundException("No actors found with the provided IDs.");
            }

            return actors;
        }

        public List<Actor> GetAllActors()
        {
            return _actors.Find(a => true).ToList();
        }

        public void UpdateActor(Actor actor)
        {
            if (actor == null)
            {
                _logger.LogError("Actor is null");
                return;
            }

            if (string.IsNullOrEmpty(actor.Id))
            {
                _logger.LogError("Actor ID is null or empty");
                return;
            }

            var filter = Builders<Actor>.Filter.Eq(a => a.Id, actor.Id);
            var update = Builders<Actor>.Update
                .Set(a => a.Name, actor.Name);

            try
            {
                var result = _actors.UpdateOne(filter, update);

                if (result.ModifiedCount == 0)
                {
                    _logger.LogWarning($"Actor with ID {actor.Id} not found or no changes made.");
                    return;
                }

                _logger.LogInformation($"Successfully updated actor with ID {actor.Id}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error updating actor with ID {actor.Id} - {e.Message}");
            }
        }

        public void DeleteActor(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogError("Actor ID is null or empty");
                return;
            }

            var filter = Builders<Actor>.Filter.Eq(a => a.Id, id);

            try
            {
                var result = _actors.DeleteOne(filter);

                if (result.DeletedCount == 0)
                {
                    _logger.LogWarning($"Actor with ID {id} not found.");
                    return;
                }

                _logger.LogInformation($"Successfully deleted actor with ID {id}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error deleting actor with ID {id} - {e.Message}");
            }
        }
    }

}
