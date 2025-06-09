using BookStore.DL.Cache;
using BookStore.Models.POCO;

namespace BookStore.DL.Interfaces
{
    public interface IBookRepository : ICacheRepository<string, Book>
    {
        Task<Book?> AddBook(Book book);

        Task<Book?> GetBookById(string id);

        Task<List<Book>> GetAllBooks();

        Task<Book?> UpdateBook(Book book);

        Task DeleteBook(string id);
    }
}
