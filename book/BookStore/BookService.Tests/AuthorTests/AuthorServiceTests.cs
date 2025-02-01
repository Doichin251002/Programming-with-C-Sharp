using BookStore.BL.Services;
using BookStore.DL.Interfaces;
using BookStore.Models.DTO;
using Moq;

namespace BookStore.Tests.AuthorTests
{
    public class AuthorServiceTests
    {
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly AuthorService _authorService;

        public AuthorServiceTests()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _authorService = new AuthorService(_authorRepositoryMock.Object);
        }

        [Fact]
        public void Add_ShouldAddAuthor()
        {
            var author = new Author { Id = "1", Name = "John Doe" };
            _authorRepositoryMock.Setup(repo => repo.GetAuthorById(author.Id)).Returns((Author)null);

            _authorService.Add(author);

            _authorRepositoryMock.Verify(repo => repo.AddAuthor(It.Is<Author>(a => a.Id == author.Id && a.Name == author.Name)), Times.Once);
        }

        [Fact]
        public void Add_ShouldThrowException_WhenAuthorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _authorService.Add(null));
        }

        [Fact]
        public void Add_ShouldThrowException_WhenAuthorNameIsNull()
        {
            var author = new Author { Id = "1", Name = null };

            var exception = Assert.Throws<ArgumentException>(() => _authorService.Add(author));

            Assert.Equal("Author name cannot be null or empty. (Parameter 'Name')", exception.Message);
        }

        [Fact]
        public void Add_ShouldThrowException_WhenAuthorNameIsEmpty()
        {
            var author = new Author { Id = "1", Name = "" };

            var exception = Assert.Throws<ArgumentException>(() => _authorService.Add(author));

            Assert.Equal("Author name cannot be null or empty. (Parameter 'Name')", exception.Message);
        }

        [Fact]
        public void Add_ShouldThrowException_WhenAuthorAlreadyExists()
        {
            var author = new Author { Id = "1", Name = "John Doe" };
            _authorRepositoryMock.Setup(repo => repo.GetAllAuthors()).Returns(new List<Author> { author });

            var exception = Assert.Throws<InvalidOperationException>(() => _authorService.Add(author));

            Assert.Equal($"Author with name {author.Name} already exists.", exception.Message);
        }

        [Fact]
        public void GetById_ShouldReturnAuthor_WhenAuthorExists()
        {
            var author = new Author { Id = "1", Name = "John Doe" };
            _authorRepositoryMock.Setup(repo => repo.GetAuthorById(author.Id)).Returns(author);

            var result = _authorService.GetById(author.Id);

            Assert.NotNull(result);
            Assert.Equal(author.Id, result.Id);
        }

        [Fact]
        public void GetById_ShouldThrowException_WhenAuthorDoesNotExist()
        {
            var authorId = "1";
            _authorRepositoryMock.Setup(repo => repo.GetAuthorById(authorId)).Returns((Author)null);

            var exception = Assert.Throws<KeyNotFoundException>(() => _authorService.GetById(authorId));

            Assert.Equal($"Author with ID {authorId} not found.", exception.Message);
        }

        [Fact]
        public void GetByIds_ShouldReturnAuthors_WhenAuthorsExist()
        {
            var authorIds = new List<string> { "1", "2" };
            var authors = new List<Author>
            {
                new Author { Id = "1", Name = "John Doe" },
                new Author { Id = "2", Name = "Jane Smith" }
            };

            _authorRepositoryMock.Setup(repo => repo.GetAuthorsByIds(authorIds)).Returns(authors);

            var result = _authorService.GetByIds(authorIds);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetByIds_ShouldThrowException_WhenNoAuthorsExist()
        {
            var authorIds = new List<string> { "1", "2" };
            _authorRepositoryMock.Setup(repo => repo.GetAuthorsByIds(authorIds)).Returns(new List<Author>());

            var exception = Assert.Throws<KeyNotFoundException>(() => _authorService.GetByIds(authorIds));

            Assert.Equal("No authors found with the provided IDs.", exception.Message);
        }

        [Fact]
        public void GetAll_ShouldReturnAllAuthors()
        {
            var authors = new List<Author>
            {
                new Author { Id = "1", Name = "John Doe" },
                new Author { Id = "2", Name = "Jane Smith" }
            };

            _authorRepositoryMock.Setup(repo => repo.GetAllAuthors()).Returns(authors);

            var result = _authorService.GetAll();

            Assert.NotNull(result);
            Assert.Equal(authors.Count, result.Count);
        }

        [Fact]
        public void Update_ShouldUpdateAuthor_WhenAuthorExists()
        {
            var author = new Author { Id = "1", Name = "John Doe" };
            _authorRepositoryMock.Setup(repo => repo.GetAuthorById(author.Id)).Returns(author);

            author.Name = "John Updated";
            _authorService.Update(author);

            _authorRepositoryMock.Verify(repo => repo.UpdateAuthor(It.Is<Author>(a => a.Id == author.Id && a.Name == "John Updated")), Times.Once);
        }

        [Fact]
        public void Update_ShouldThrowException_WhenAuthorDoesNotExist()
        {
            var author = new Author { Id = "1", Name = "John Doe" };
            _authorRepositoryMock.Setup(repo => repo.GetAuthorById(author.Id)).Returns((Author)null);

            var exception = Assert.Throws<KeyNotFoundException>(() => _authorService.Update(author));

            Assert.Equal($"Author with ID {author.Id} not found.", exception.Message);
        }

        [Fact]
        public void Update_ShouldThrowException_WhenAuthorIdIsNull()
        {
            var author = new Author { Id = null, Name = "John Doe" };

            var exception = Assert.Throws<ArgumentException>(() => _authorService.Update(author));

            Assert.Equal("Author ID cannot be null or empty. (Parameter 'Id')", exception.Message);
        }

        [Fact]
        public void Update_ShouldThrowException_WhenAuthorIdIsEmpty()
        {
            var author = new Author { Id = "", Name = "John Doe" };

            var exception = Assert.Throws<ArgumentException>(() => _authorService.Update(author));

            Assert.Equal("Author ID cannot be null or empty. (Parameter 'Id')", exception.Message);
        }

        [Fact]
        public void Update_ShouldThrowException_WhenAuthorNameIsNull()
        {
            var author = new Author { Id = "1", Name = null };

            var exception = Assert.Throws<ArgumentException>(() => _authorService.Update(author));

            Assert.Equal("Author name cannot be null or empty. (Parameter 'Name')", exception.Message);
        }

        [Fact]
        public void Update_ShouldThrowException_WhenAuthorNameIsEmpty()
        {
            var author = new Author { Id = "1", Name = "" };

            var exception = Assert.Throws<ArgumentException>(() => _authorService.Update(author));

            Assert.Equal("Author name cannot be null or empty. (Parameter 'Name')", exception.Message);
        }

        [Fact]
        public void Delete_ShouldDeleteAuthor_WhenAuthorExists()
        {
            var author = new Author { Id = "1", Name = "John Doe" };
            _authorRepositoryMock.Setup(repo => repo.GetAuthorById(author.Id)).Returns(author);

            _authorService.Delete(author.Id);

            _authorRepositoryMock.Verify(repo => repo.DeleteAuthor(It.Is<string>(id => id == author.Id)), Times.Once);
        }

        [Fact]
        public void Delete_ShouldThrowException_WhenAuthorIdIsNull()
        {
            var exception = Assert.Throws<ArgumentException>(() => _authorService.Delete(null));

            Assert.Equal("Author ID cannot be null or empty. (Parameter 'id')", exception.Message);
        }

        [Fact]
        public void Delete_ShouldThrowException_WhenAuthorIdIsEmpty()
        {
            var exception = Assert.Throws<ArgumentException>(() => _authorService.Delete(""));

            Assert.Equal("Author ID cannot be null or empty. (Parameter 'id')", exception.Message);
        }


        [Fact]
        public void Delete_ShouldThrowException_WhenAuthorDoesNotExist()
        {
            var authorId = "1";
            _authorRepositoryMock.Setup(repo => repo.GetAuthorById(authorId)).Returns((Author)null);

            var exception = Assert.Throws<KeyNotFoundException>(() => _authorService.Delete(authorId));

            Assert.Equal($"Author with ID {authorId} not found.", exception.Message);
        }
    }
}
