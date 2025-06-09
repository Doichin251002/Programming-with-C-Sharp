using BookStore.BL.Interfaces;
using BookStore.DL.Interfaces;
using BookStore.Models.POCO;
using Microsoft.Extensions.Logging;

namespace BookStore.BL.Services
{
    public class BookService : IBookService
    {
        private readonly ILogger<BookService> _logger;
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IStoreLocationGateway _locationGateway;

        public BookService(ILogger<BookService> logger, IBookRepository bookRepository, IAuthorRepository authorRepository, IStoreLocationGateway locationGateway)
        {
            _logger = logger;
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _locationGateway = locationGateway;
        }

        public async Task<Book?> Add(Book? book)
        {
            if (book is null)
            {
                throw new ArgumentNullException(nameof(book), "Book cannot be null.");
            };

            if (!await IsExistingAuthor(book))
            {
                throw new KeyNotFoundException($"Author with ID {book.Authors.First()} does not exist");
            }

            bool IsExist = await IsExistBook(book);

            if (IsExist)
            {
                throw new InvalidOperationException($"Book with title {book.Title} already exists.");
            }

            return await _bookRepository.AddBook(book);
        }

        public async Task<Book?> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Book ID cannot be null or empty.", nameof(id));
            }

            var book = await _bookRepository.GetBookById(id);

            if (book == null)
            {
                throw new KeyNotFoundException($"Book with ID {id} not found.");
            }

            return book;
        }

        public async Task<List<Book>> GetAll()
        {
            return await _bookRepository.GetAllBooks();
        }

        public async Task<Book?> Update(Book book)
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

            if (!await IsExistingAuthor(book))
            {
                throw new KeyNotFoundException($"Authors with ID {book.Authors.First()} not found.");
            }

            return await _bookRepository.UpdateBook(book);
        }

        public async Task Delete(string id)
        {
            var book = _bookRepository.GetBookById(id);

            if (book is null)
            {
                throw new KeyNotFoundException($"Book with ID {id} does not exist");
            }

            await _bookRepository.DeleteBook(id);
        }

        public async Task<string> GetLocations()
        {
            try
            {
                return await _locationGateway.GetAllLocations();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Cannot fetch bikes. {ex.Message}");
                throw;
            }
        }

        private async Task<Boolean> IsExistBook(Book? book)
        {
            var allBooks = await _bookRepository.GetAllBooks();

            if (allBooks is not null && allBooks.Any())
            {
                foreach (Book b in allBooks)
                {
                    bool matchingTitle = b.Title == book.Title;
                    bool matchingYear = b.Year == book.Year;
                    bool matchingAuthor = await IsExistingAuthor(book);

                    if (matchingTitle && matchingYear && matchingAuthor)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private async Task<bool> IsExistingAuthor(Book book)
        {
            if (book?.Authors == null || !book.Authors.Any()) return false;

            var allAuthorIds = (await _authorRepository.GetAllAuthors())
                .Select(a => a.Id)
                .ToHashSet();

            return book.Authors.Any(id => allAuthorIds.Contains(id));
        }
    }
}
