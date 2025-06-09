using BookStore.DL.Interfaces;
using BookStore.Models.Configurations;
using BookStore.Models.POCO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookStore.DL.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly IMongoCollection<Author> _authors;

        private readonly ILogger<AuthorRepository> _logger;

        public AuthorRepository(
            IOptionsMonitor<MongoDbConfiguration> mongoConfig,
            ILogger<AuthorRepository> logger)
        {
            _logger = logger;
            var client = new MongoClient(
                mongoConfig.CurrentValue.ConnectionString);

            var database = client.GetDatabase(
                mongoConfig.CurrentValue.DatabaseName);

            _authors = database.GetCollection<Author>(
                $"{nameof(Author)}s");
        }

        public async Task<Author?> AddAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author), "Author cannot be null.");
            }

            if (string.IsNullOrEmpty(author.Name))
            {
                throw new ArgumentException("Author name cannot be empty.", nameof(author.Name));
            }

            author.Id = System.Guid.NewGuid().ToString();

            try
            {
                await _authors.InsertOneAsync(author);
                _logger.LogInformation($"Author with ID {author.Id} successfully added.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while adding the author: {ex.Message}");
            }

            return author;
        }

        public async Task<Author?> GetAuthorById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            var result = await _authors.FindAsync(a => a.Id == id);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Author>> GetAuthorsByIds(IEnumerable<string> authorsIds)
        {
            if (authorsIds == null || !authorsIds.Any())
            {
                throw new ArgumentException("Author IDs cannot be null or empty.", nameof(authorsIds));
            }

            var filter = Builders<Author>.Filter.In(a => a.Id, authorsIds);
            var authors = await _authors.FindAsync(filter);
            var result = await authors.ToListAsync();

            if (result == null || !result.Any())
            {
                throw new KeyNotFoundException("No authors found with the provided IDs.");
            }

            return result;
        }

        public async Task<List<Author>> GetAllAuthors()
        {
            var result = await _authors.FindAsync(a => true);
            return await result.ToListAsync();
        }

        public async Task<Author?> UpdateAuthor(Author author)
        {
            if (author == null)
            {
                _logger.LogError("Author is null");
                return null;
            }

            if (string.IsNullOrEmpty(author.Id))
            {
                _logger.LogError("Author ID is null or empty");
                return null;
            }

            var filter = Builders<Author>.Filter.Eq(a => a.Id, author.Id);
            var update = Builders<Author>.Update
                .Set(a => a.Name, author.Name);

            try
            {
                var result = await _authors.UpdateOneAsync(filter, update);

                if (result.ModifiedCount == 0)
                {
                    _logger.LogWarning($"Author with ID {author.Id} not found or no changes made.");
                }
                else
                {
                    _logger.LogInformation($"Successfully updated author with ID {author.Id}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error updating author with ID {author.Id} - {e.Message}");
            }

            return author;
        }

        public async Task DeleteAuthor(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogError("Author ID is null or empty");
                return;
            }

            var filter = Builders<Author>.Filter.Eq(a => a.Id, id);

            try
            {
                var result = await _authors.DeleteOneAsync(filter);

                if (result.DeletedCount == 0)
                {
                    _logger.LogWarning($"Author with ID {id} not found.");
                    return;
                }

                _logger.LogInformation($"Successfully deleted author with ID {id}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error deleting author with ID {id} - {e.Message}");
            }
        }
    }
}
