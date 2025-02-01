using MovieStore.Models.DTO;

namespace MovieStore.DL.Interfaces
{
    public interface IMovieRepository
    {
        void AddMovie(Movie movie);

        Movie? GetMovieById(string id);

        List<Movie> GetAllMovies();

        void UpdateMovie(Movie movie);

        void DeleteMovie(string id);
    }
}
