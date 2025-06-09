using BookStore.Models.POCO;

namespace BookStore.BL.Interfaces
{
    public interface IBookService
    {
        Task<Book?> Add(Book book);

        Task<Book?> GetById(string id);

        Task<List<Book>> GetAll();

        Task<Book?> Update(Book book);

        Task Delete(string id);

        Task<string> GetLocations();

    }
}
