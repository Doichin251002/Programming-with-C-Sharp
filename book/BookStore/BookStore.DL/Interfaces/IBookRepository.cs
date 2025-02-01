using BookStore.Models.DTO;

namespace BookStore.DL.Interfaces
{
    public interface IBookRepository
    {
        void AddBook(Book book);

        Book? GetBookById(string id);

        List<Book> GetAllBooks();

        void UpdateBook(Book book);

        void DeleteBook(string id);
    }
}
