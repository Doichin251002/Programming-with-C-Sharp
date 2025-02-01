using MovieStore.Models.DTO;

namespace MovieStore.BL.Interfaces
{
    public interface IActorService
    {
        void Add(Actor actor);

        Actor? GetById(string id);

        IEnumerable<Actor> GetByIds(IEnumerable<string> actorsIds);

        List<Actor> GetAll();

        void Update(Actor actor);

        void Delete(string id);

    }
}
