using MovieStore.Models.DTO;

namespace MovieStore.BL.Interfaces
{
    public interface IMovieService
    {
        List<Movie> GetAll();

        void Add(Movie movie);

        Movie? GetById(string id);

        void Update(Movie movie);

        void Delete(string id);

    }
}
