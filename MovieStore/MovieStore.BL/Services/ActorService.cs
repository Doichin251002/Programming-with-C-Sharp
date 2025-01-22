using MovieStore.BL.Interfaces;
using MovieStore.DL.Interfaces;
using MovieStore.Models.DTO;

namespace MovieStore.BL.Services
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _actorRepository;
        
        public ActorService(IActorRepository actorRepository)
        {
            _actorRepository = actorRepository;
        }
        public void Add(Actor actor)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor), "Actor cannot be null.");
            }

            if (string.IsNullOrEmpty(actor.Name))
            {
                throw new ArgumentException("Actor name cannot be null or empty.", nameof(actor.Name));
            }

            var existingActor = _actorRepository.GetActorById(actor.Id);

            if (existingActor != null)
            {
                throw new InvalidOperationException($"Actor with ID {actor.Id} already exists.");
            }

            _actorRepository.AddActor(actor);
        }

        public Actor? GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Actor ID cannot be null or empty.", nameof(id));
            }

            var actor = _actorRepository.GetActorById(id);

            if (actor == null)
            {
                throw new KeyNotFoundException($"Actor with ID {id} not found.");
            }

            return actor;
        }

        public IEnumerable<Actor> GetByIds(IEnumerable<string> actorsIds)
        {
            if (actorsIds == null || !actorsIds.Any())
            {
                throw new ArgumentException("Actor IDs cannot be null or empty.", nameof(actorsIds));
            }

            var actors = _actorRepository.GetActorsByIds(actorsIds);

            if (actors == null || !actors.Any())
            {
                throw new KeyNotFoundException("No actors found with the provided IDs.");
            }

            return actors;
        }


        public List<Actor> GetAll()
        {
            return _actorRepository.GetAllActors();
        }

        public void Update(Actor actor)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor), "Actor cannot be null.");
            }

            if (string.IsNullOrEmpty(actor.Id))
            {
                throw new ArgumentException("Actor ID cannot be null or empty.", nameof(actor.Id));
            }

            var existingActor = _actorRepository.GetActorById(actor.Id);

            if (existingActor == null)
            {
                throw new KeyNotFoundException($"Actor with ID {actor.Id} not found.");
            }

            _actorRepository.UpdateActor(actor);
        }

        public void Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Actor ID cannot be null or empty.", nameof(id));
            }

            var existingActor = _actorRepository.GetActorById(id);

            if (existingActor == null)
            {
                throw new KeyNotFoundException($"Actor with ID {id} not found.");
            }

            _actorRepository.DeleteActor(id);
        }

    }
}
