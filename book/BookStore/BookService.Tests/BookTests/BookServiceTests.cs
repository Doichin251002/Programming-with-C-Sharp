using BookStore.DL.Interfaces;
using BookStore.Models.DTO;
using Moq;

namespace BookStore.Tests.BookTests
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly BL.Services.BookService _bookService;

        public BookServiceTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _bookService = new BL.Services.BookService(_bookRepositoryMock.Object, _authorRepositoryMock.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnAllBooks()
        {
            var books = new List<Book>
            {
                new Book { Id = "1", Title = "Book 1" },
                new Book { Id = "2", Title = "Book 2" }
            };

            _bookRepositoryMock.Setup(repo => repo.GetAllBooks()).Returns(books);

            var result = _bookService.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, m => m.Title == "Book 1");
        }

        [Fact]
        public void Add_ShouldAddBook_WhenAuthorsExist()
        {
            var book = new Book { Id = "1", Title = "New Book", Authors = new List<string> { "Author1" } };

            _authorRepositoryMock.Setup(repo => repo.GetAllAuthors()).Returns(new List<Author> { new Author { Id = "Author1" } });

            _bookService.Add(book);

            _bookRepositoryMock.Verify(repo => repo.AddBook(book), Times.Once);
        }

        [Fact]
        public void Add_ShouldThrowException_WhenAuthorDoesNotExist()
        {
            var book = new Book { Id = "1", Title = "New Book", Authors = new List<string> { "Author1" } };

            _authorRepositoryMock.Setup(repo => repo.GetAllAuthors()).Returns(new List<Author> { });

            var exception = Assert.Throws<KeyNotFoundException>(() => _bookService.Add(book));

            Assert.Equal($"Author with ID {book.Authors.First()} does not exist", exception.Message);
        }

        [Fact]
        public void GetById_ShouldReturnBook_WhenBookExists()
        {
            var book = new Book { Id = "1", Title = "Book 1" };

            _bookRepositoryMock.Setup(repo => repo.GetBookById(book.Id)).Returns(book);

            var result = _bookService.GetById(book.Id);

            Assert.NotNull(result);
            Assert.Equal(book.Id, result.Id);
        }

        [Fact]
        public void GetById_ShouldThrowException_WhenBookIdIsNullOrEmpty()
        {
            var exception = Assert.Throws<ArgumentException>(() => _bookService.GetById(string.Empty));

            Assert.Equal("Book ID cannot be null or empty. (Parameter 'id')", exception.Message);
        }

        [Fact]
        public void GetById_ShouldThrowException_WhenBookDoesNotExist()
        {
            var id = "1";
            _bookRepositoryMock.Setup(repo => repo.GetBookById(id)).Returns((Book)null);

            var exception = Assert.Throws<KeyNotFoundException>(() => _bookService.GetById("1"));

            Assert.Equal($"Book with ID {id} not found.", exception.Message);
        }

        [Fact]
        public void Delete_ShouldDeleteBook_WhenBookExists()
        {
            var book = new Book { Id = "1", Title = "Book 1" };

            _bookRepositoryMock.Setup(repo => repo.GetBookById(book.Id)).Returns(book);

            _bookService.Delete(book.Id);

            _bookRepositoryMock.Verify(repo => repo.DeleteBook(book.Id), Times.Once);
        }

        [Fact]
        public void Delete_ShouldThrowException_WhenBookDoesNotExist()
        {
            var id = "1";

            _bookRepositoryMock.Setup(repo => repo.GetBookById(id)).Returns((Book)null);

            var exception = Assert.Throws<KeyNotFoundException>(() => _bookService.Delete(id));

            Assert.Equal($"Book with ID {id} does not exist", exception.Message);
        }

        [Fact]
        public void Update_ShouldUpdateBook_WhenBookExists()
        {
            var book = new Book { Id = "1", Title = "Updated Book", Year = 2000, Authors = new List<string> { "Author1" } };

            _authorRepositoryMock.Setup(repo => repo.GetAllAuthors()).Returns(new List<Author>
            {
              new Author { Id = "Author1" }
            });

            _bookRepositoryMock.Setup(repo => repo.GetBookById(book.Id)).Returns(book);

            _bookService.Update(book);

            _bookRepositoryMock.Verify(repo => repo.UpdateBook(book), Times.Once);
        }

        [Fact]
        public void Update_ShouldThrowException_WhenBookIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _bookService.Update(null));

            Assert.Equal("Book cannot be null (Parameter 'book')", exception.Message);
        }

        [Fact]
        public void Update_ShouldThrowException_WhenBookIdIsNullOrEmpty()
        {
            var book = new Book { Id = string.Empty, Title = "Invalid Book" };

            var exception = Assert.Throws<ArgumentException>(() => _bookService.Update(book));

            Assert.Equal("Book ID cannot be null or empty (Parameter 'Id')", exception.Message);
        }

        [Fact]
        public void Update_ShouldThrowException_WhenBookDoesNotExist()
        {
            var book = new Book { Id = "1", Title = "Non-existent Book" };

            _bookRepositoryMock.Setup(repo => repo.GetBookById(book.Id)).Returns((Book)null);

            var exception = Assert.Throws<KeyNotFoundException>(() => _bookService.Update(book));

            Assert.Equal($"Book with ID {book.Id} does not exist", exception.Message);
        }
    }
}
