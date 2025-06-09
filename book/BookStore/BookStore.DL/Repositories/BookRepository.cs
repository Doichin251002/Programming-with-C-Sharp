using BookStore.DL.Interfaces;
using BookStore.Models.Configurations;
using BookStore.Models.POCO;
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

        public async Task<Book?> AddBook(Book book)
        {
            await _books.InsertOneAsync(book);
            _logger.LogInformation($"Successfully added book with ID: {book.Id}, Title: {book.Title}");

            return book;
        }

        public async Task<Book?> GetBookById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            var result = await _books.FindAsync(m => m.Id.Equals(id));
            return await result.FirstOrDefaultAsync();
        }

        public async Task<List<Book>> GetAllBooks()
        {
            return await _books.Find(book => true).ToListAsync();
        }

        public async Task<Book?> UpdateBook(Book book)
        {
            if (book == null)
            {
                _logger.LogError("Book is null");
            }

            else if (string.IsNullOrEmpty(book.Id))
            {
                string id = book.Id;
                _logger.LogError($"Book with id {id} not found");
            }

            var filter = Builders<Book>.Filter.Eq(b => b.Id, book.Id);

            try
            {
                var update = Builders<Book>.Update
                    .Set(b => b.Title, book.Title)
                    .Set(b => b.Year, book.Year)
                    .Set(b => b.Authors, book.Authors);

                var result = await _books.UpdateOneAsync(filter, update);

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
                throw;
            }

            return book;
        }

        public async Task DeleteBook(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Book ID cannot be null or empty", nameof(id));
            }

            try
            {
                var filter = Builders<Book>.Filter.Eq(b => b.Id, id);

                var deletedbook = await _books.FindOneAndDeleteAsync(filter);

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
                throw;
            }
        }

    }
}
