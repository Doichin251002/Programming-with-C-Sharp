using BookStore.BL.Interfaces;
using BookStore.DL.Interfaces;
using BookStore.Models.DTO;

namespace BookStore.BL.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public void Add(Author? author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author), "Author cannot be null.");
            }

            if (string.IsNullOrEmpty(author.Name))
            {
                throw new ArgumentException("Author name cannot be null or empty.", nameof(author.Name));
            }

            bool isExistingAuthor = isExist(author);

            if (isExistingAuthor)
            {
                throw new InvalidOperationException($"Author with name {author.Name} already exists.");
            }

            _authorRepository.AddAuthor(author);
        }

        public Author? GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Author ID cannot be null or empty.", nameof(id));
            }

            var author = _authorRepository.GetAuthorById(id);

            if (author == null)
            {
                throw new KeyNotFoundException($"Author with ID {id} not found.");
            }

            return author;
        }

        public IEnumerable<Author> GetByIds(IEnumerable<string> authorsIds)
        {
            if (authorsIds == null || !authorsIds.Any())
            {
                throw new ArgumentException("Author IDs cannot be null or empty.", nameof(authorsIds));
            }

            var authors = _authorRepository.GetAuthorsByIds(authorsIds);

            if (authors == null || !authors.Any())
            {
                throw new KeyNotFoundException("No authors found with the provided IDs.");
            }

            return authors;
        }


        public List<Author> GetAll()
        {
            return _authorRepository.GetAllAuthors();
        }

        public void Update(Author author)
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

            _authorRepository.UpdateAuthor(author);
        }

        public void Delete(string id)
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

            _authorRepository.DeleteAuthor(id);
        }

        private bool isExist(Author? author)
        {
            var allAuthors = _authorRepository.GetAllAuthors();

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
