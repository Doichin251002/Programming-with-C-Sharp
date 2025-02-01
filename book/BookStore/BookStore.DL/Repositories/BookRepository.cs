using BookStore.DL.Interfaces;
using BookStore.Models.Configurations;
using BookStore.Models.DTO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookStore.DL.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IMongoCollection<Book> _books;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(
            IOptionsMonitor<MongoDbConfiguration> mongoConfig,
            ILogger<BookRepository> logger)
        {
            _logger = logger;
            var client = new MongoClient(
                mongoConfig.CurrentValue.ConnectionString);

            var database = client.GetDatabase(
                mongoConfig.CurrentValue.DatabaseName);

            _books = database.GetCollection<Book>(
                $"{nameof(Book)}s");
        }

        public void AddBook(Book book)
        {
            if (book == null)
            {
                _logger.LogError("Attempted to add a null book.");
                return;
            }

            if (string.IsNullOrWhiteSpace(book.Title))
            {
                _logger.LogError("Attempted to add a book with an empty or null title.");
                return;
            }

            if (book.Year <= 0)
            {
                _logger.LogError("Attempted to add a book with an invalid year.");
                return;
            }

            book.Id = Guid.NewGuid().ToString();

            try
            {
                _books.InsertOne(book);
                _logger.LogInformation($"Successfully added book with ID: {book.Id}, Title: {book.Title}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding book. Title: {book.Title}. Exception: {ex.Message}");
            }
        }

        public Book? GetBookById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            return _books.Find(m => m.Id.Equals(id))
                .FirstOrDefault();
        }

        public List<Book> GetAllBooks()
        {
            return _books.Find(book => true).ToList();
        }

        public void UpdateBook(Book book)
        {
            if (book == null)
            {
                _logger.LogError("Book is null");
                return;
            }
            else if (string.IsNullOrEmpty(book.Id))
            {
                string id = book.Id;
                _logger.LogError($"Book with id {id} not found");
                return;
            }

            var filter = Builders<Book>.Filter.Eq(b => b.Id, book.Id);

            try
            {
                var update = Builders<Book>.Update
                    .Set(b => b.Title, book.Title)
                    .Set(b => b.Year, book.Year)
                    .Set(b => b.Authors, book.Authors);

                var result = _books.UpdateOne(filter, update);

                if (result.ModifiedCount > 0)
                {
                    _logger.LogInformation($"Successfully updated book with id {book.Id}");
                }
                else
                {
                    _logger.LogWarning($"No changes made to book with id {book.Id}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error updating book {book.Id}: {e.Message} - {e.StackTrace}");
            }
        }

        public void DeleteBook(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Book ID cannot be null or empty", nameof(id));
            }

            try
            {
                var filter = Builders<Book>.Filter.Eq(b => b.Id, id);

                var deletedbook = _books.FindOneAndDelete(filter);

                if (deletedbook != null)
                {
                    _logger.LogInformation($"Successfully deleted book with ID: {id}");
                }
                else
                {
                    _logger.LogWarning($"Book with ID: {id} was not found in the database.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting book with ID: {id}");
            }
        }

    }
}
