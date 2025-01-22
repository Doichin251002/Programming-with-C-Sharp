using MovieStore.Models.DTO;

namespace MovieStore.DL.Interfaces
{
    public interface IActorRepository
    {
        void AddActor(Actor actor);
        Actor? GetActorById(string id);
        IEnumerable<Actor> GetActorsByIds(IEnumerable<string> actorsIds);
        List<Actor> GetAllActors();
        void UpdateActor(Actor actor);
        void DeleteActor(string id);
    }
}
