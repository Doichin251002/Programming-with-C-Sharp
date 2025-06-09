using BookStore.Models.POCO;

namespace BookStore.BL.Interfaces
{
    public interface IAuthorService
    {
        Task<Author?> Add(Author author);

        Task<Author?> GetById(string id);

        Task<IEnumerable<Author>> GetByIds(IEnumerable<string> authorsIds);

        Task<List<Author>> GetAll();

        Task<Author?> Update(Author author);

        Task Delete(string id);

    }
}
