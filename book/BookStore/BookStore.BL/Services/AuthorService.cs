using BookStore.BL.Interfaces;
using BookStore.DL.Interfaces;
using BookStore.Models.POCO;
using Microsoft.Extensions.Logging;

namespace BookStore.BL.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        private readonly ILogger<AuthorService> _logger;

        public AuthorService(IAuthorRepository authorRepository, ILogger<AuthorService> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<Author?> Add(Author? author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author), "Author cannot be null.");
            }

            if (string.IsNullOrEmpty(author.Name))
            {
                throw new ArgumentException("Author name cannot be null or empty.", nameof(author.Name));
            }

            bool isExistingAuthor = await isExist(author);

            if (isExistingAuthor)
            {
                throw new InvalidOperationException($"Author with name {author.Name} already exists.");
            }

            return await _authorRepository.AddAuthor(author);
        }

        public async Task<Author?> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Author ID cannot be null or empty.", nameof(id));
            }

            var author = await _authorRepository.GetAuthorById(id);

            if (author == null)
            {
                throw new KeyNotFoundException($"Author with ID {id} not found.");
            }

            return author;
        }

        public async Task<IEnumerable<Author>> GetByIds(IEnumerable<string> authorsIds)
        {
            if (authorsIds == null || !authorsIds.Any())
            {
                throw new ArgumentException("Author IDs cannot be null or empty.", nameof(authorsIds));
            }

            var authors = await _authorRepository.GetAuthorsByIds(authorsIds);

            if (authors == null || !authors.Any())
            {
                throw new KeyNotFoundException("No authors found with the provided IDs.");
            }

            return authors;
        }

        public async Task<List<Author>> GetAll()
        {
            return await _authorRepository.GetAllAuthors();
        }

        public async Task<Author?> Update(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author), "Author cannot be null.");
            }

            if (string.IsNullOrEmpty(author.Id))
            {
                throw new ArgumentException("Author ID cannot be null or empty.", nameof(author.Id));
            }

            if (string.IsNullOrEmpty(author.Name))
            {
                throw new ArgumentException("Author name cannot be null or empty.", nameof(author.Name));
            }

            var existingAuthor = _authorRepository.GetAuthorById(author.Id);

            if (existingAuthor == null)
            {
                throw new KeyNotFoundException($"Author with ID {author.Id} not found.");
            }

            return await _authorRepository.UpdateAuthor(author);
        }

        public async Task Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Author ID cannot be null or empty.", nameof(id));
            }

            var existingAuthor = _authorRepository.GetAuthorById(id);

            if (existingAuthor == null)
            {
                throw new KeyNotFoundException($"Author with ID {id} not found.");
            }

            await _authorRepository.DeleteAuthor(id);
        }

        private async Task<Boolean> isExist(Author? author)
        {
            var allAuthors = await _authorRepository.GetAllAuthors();

            if (allAuthors is not null)
            {
                foreach (Author a in allAuthors)
                {
                    if (author.Name == a.Name)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
