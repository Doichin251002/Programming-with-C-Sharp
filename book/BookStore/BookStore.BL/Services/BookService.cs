using BookStore.BL.Interfaces;
using BookStore.DL.Interfaces;
using BookStore.Models.DTO;

namespace BookStore.BL.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        public void Add(Book? book)
        {
            if (book is null) return;

            if (!IsExistingAuthor(book))
            {
                throw new KeyNotFoundException($"Author with ID {book.Authors.First()} does not exist");
            }

            bool IsExist = IsExistBook(book);

            if (IsExist)
            {
                throw new InvalidOperationException($"Book with title {book.Title} already exists.");
            }

            _bookRepository.AddBook(book);
        }

        public Book? GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Book ID cannot be null or empty.", nameof(id));
            }

            var book = _bookRepository.GetBookById(id);

            if (book == null)
            {
                throw new KeyNotFoundException($"Book with ID {id} not found.");
            }

            return book;
        }

        public List<Book> GetAll()
        {
            return _bookRepository.GetAllBooks();
        }

        public void Update(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book), "Book cannot be null");
            }

            if (string.IsNullOrEmpty(book.Id))
            {
                throw new ArgumentException("Book ID cannot be null or empty", nameof(book.Id));
            }

            var existingBook = _bookRepository.GetBookById(book.Id);

            if (existingBook == null)
            {
                throw new KeyNotFoundException($"Book with ID {book.Id} does not exist");
            }

            if (!IsExistingAuthor(book))
            {
                throw new KeyNotFoundException($"Authors with ID {book.Authors.First()} not found.");
            }

            _bookRepository.UpdateBook(book);
        }

        public void Delete(string id)
        {
            var book = _bookRepository.GetBookById(id);

            if (book is null)
            {
                throw new KeyNotFoundException($"Book with ID {id} does not exist");
            }

            _bookRepository.DeleteBook(id);
        }

        private bool IsExistBook(Book? book)
        {
            var allBooks = _bookRepository.GetAllBooks();

            if (allBooks is not null && allBooks.Any())
            {
                foreach (Book b in allBooks)
                {
                    bool matchingTitle = b.Title == book.Title;
                    bool matchingYear = b.Year == book.Year;
                    bool matchingAuthor = IsExistingAuthor(book);

                    if (matchingTitle && matchingYear && matchingAuthor)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsExistingAuthor(Book book)
        {
            var allAuthorIds = _authorRepository.GetAllAuthors().Select(a => a.Id).ToList();

            if (allAuthorIds is not null && allAuthorIds.Any())
            {
                foreach (var authorId in allAuthorIds)
                {
                    foreach (var bookAuthorId in book.Authors)
                    {
                        if (bookAuthorId.Equals(authorId))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
