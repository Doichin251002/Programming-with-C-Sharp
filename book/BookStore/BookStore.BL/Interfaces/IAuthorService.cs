using BookStore.Models.DTO;

namespace BookStore.BL.Interfaces
{
    public interface IAuthorService
    {
        void Add(Author author);

        Author? GetById(string id);

        IEnumerable<Author> GetByIds(IEnumerable<string> authorsIds);

        List<Author> GetAll();

        void Update(Author author);

        void Delete(string id);

    }
}
