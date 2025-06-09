using BookStore.DL.Cache;
using BookStore.Models.POCO;

namespace BookStore.DL.Interfaces
{
    public interface IAuthorRepository : ICacheRepository<string, Author>
    {
        Task<Author?> AddAuthor(Author author);

        Task<Author?> GetAuthorById(string id);

        Task<IEnumerable<Author>> GetAuthorsByIds(IEnumerable<string> authorsIds);

        Task<List<Author>> GetAllAuthors();

        Task<Author?> UpdateAuthor(Author author);

        Task DeleteAuthor(string id);
    }
}
