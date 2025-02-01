using BookStore.Models.DTO;
using BookStore.Models.Requests.Author;
using BookStore.Models.Requests.Book;
using Mapster;

namespace BookStore.MapsterConfig
{
    public class MapsterConfiguration
    {
        public static void Configure()
        {
            TypeAdapterConfig<Book, AddBookRequest>
                .NewConfig()
                .TwoWays();

            TypeAdapterConfig<Book, UpdateBookRequest>
                .NewConfig()
                .TwoWays();

            TypeAdapterConfig<Author, AddAuthorRequest>
                .NewConfig()
                .TwoWays();

            TypeAdapterConfig<Author, UpdateAuthorRequest>
                .NewConfig()
                .TwoWays();
        }
    }
}
