using BookStore.BL.Interfaces;
using BookStore.BL.Services;
using BookStore.DL;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.BL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IBookService, BookService>()
                .AddSingleton<IAuthorService, AuthorService>();

            return services;
        }
    }
}
