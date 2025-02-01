using BookStore.Models.DTO;

namespace BookStore.BL.Interfaces
{
    public interface IBookService
    {
        void Add(Book book);

        Book? GetById(string id);

        List<Book> GetAll();

        void Update(Book book);

        void Delete(string id);

    }
}
