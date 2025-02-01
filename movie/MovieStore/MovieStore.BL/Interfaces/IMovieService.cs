using MovieStore.Models.DTO;

namespace MovieStore.BL.Interfaces
{
    public interface IMovieService
    {
        void Add(Movie movie);

        Movie? GetById(string id);

        List<Movie> GetAll();

        void Update(Movie movie);

        void Delete(string id);

    }
}
