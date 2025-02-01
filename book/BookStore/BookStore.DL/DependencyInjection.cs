using BookStore.DL.Interfaces;
using BookStore.DL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.DL
{
    public static class DependencyInjection
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services
                .AddSingleton<IBookRepository, BookRepository>()
                .AddSingleton<IAuthorRepository, AuthorRepository>();
        }
    }
}
