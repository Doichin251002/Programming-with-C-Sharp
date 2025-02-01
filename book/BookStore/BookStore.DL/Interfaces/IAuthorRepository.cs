using BookStore.Models.DTO;

namespace BookStore.DL.Interfaces
{
    public interface IAuthorRepository
    {
        void AddAuthor(Author author);

        Author? GetAuthorById(string id);

        IEnumerable<Author> GetAuthorsByIds(IEnumerable<string> authorsIds);

        List<Author> GetAllAuthors();

        void UpdateAuthor(Author author);

        void DeleteAuthor(string id);
    }
}
