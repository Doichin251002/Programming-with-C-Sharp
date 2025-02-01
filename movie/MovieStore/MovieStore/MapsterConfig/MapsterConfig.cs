using Mapster;
using MovieStore.Models.DTO;
using MovieStore.Models.Requests.Actor;
using MovieStore.Models.Requests.Movie;

namespace MovieStore.MapsterConfig
{
    public class MapsterConfiguration
    {
        public static void Configure()
        {
            TypeAdapterConfig<Movie, AddMovieRequest>
                .NewConfig()
                .TwoWays();

            TypeAdapterConfig<Movie, UpdateMovieRequest>
                .NewConfig()
                .TwoWays();

            TypeAdapterConfig<Actor, AddActorRequest>
                .NewConfig()
                .TwoWays();

            TypeAdapterConfig<Actor, UpdateActorRequest>
                .NewConfig()
                .TwoWays();
        }
    }
}
