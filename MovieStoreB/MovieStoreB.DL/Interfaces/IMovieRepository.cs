
using MovieStoreB.Models.DTO;

namespace MovieStoreB.DL.Interfaces
{
    public interface IMovieRepository
    {
        List<Movie> GetMovies();

        void AddMovie(Movie movie);

        void DeleteMovie(int id);

        Movie? GetMoviesById(int id);
    }
}
