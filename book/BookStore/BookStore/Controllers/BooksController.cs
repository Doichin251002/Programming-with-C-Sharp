using BookStore.BL.Interfaces;
using BookStore.Models.POCO;
using BookStore.Models.Requests.Book;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookService bookService, IMapper mapper, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("AddBook")]
        public async Task<IActionResult> Add(AddBookRequest book)
        {
            try
            {
                var bookDto = _mapper.Map<Book>(book);

                if (bookDto == null)
                {
                    return BadRequest("No valid data.");
                }

                await _bookService.Add(bookDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding book with");
                return BadRequest(ex.Message);
            }

            return Ok($"Book {book.Title} added successfully.");
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("GetAllBooks")]
        public async Task<IActionResult> Get()
        {
            var result = await _bookService.GetAll();

            if (result == null || result.Count == 0)
            {
                return NotFound("No books found");
            }

            return Ok(result);
        }

        [HttpGet("GetBookById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Book ID can't be null or empty");
            }

            try
            {
                var result = await _bookService.GetById(id);

                if (result == null)
                {
                    return NotFound($"Book with ID:{id} not found");
                }

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("UpdateBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(UpdateBookRequest book)
        {
            var bookDto = _mapper.Map<Book>(book);

            if (bookDto == null)
            {
                return BadRequest("Failed to map request to book DTO.");
            }

            if (string.IsNullOrEmpty(bookDto.Id))
            {
                return BadRequest("Invalid book data or missing ID.");
            }

            try
            {
                var existingBook = await _bookService.GetById(bookDto.Id);

                if (existingBook == null)
                {
                    return NotFound($"Book with ID {book.Id} not found.");
                }

                await _bookService.Update(bookDto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating book with ID {book.Id}");
                return StatusCode(400, ex.Message);
            }

            return Ok($"Book with ID {book.Id} successfully updated.");
        }

        [HttpDelete("DeleteBook")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest($"Book with id {id} not found");
            }

            try
            {
                await _bookService.Delete(id);
            }
            catch (Exception)
            {
                return BadRequest($"Book with ID {id} not found.");
            }

            return Ok($"Book with {id} deleted successfully.");
        }

        [HttpGet("GetLocations")]
        public async Task<IActionResult> GetLocations()
        {
            try
            {
                var result = await _bookService.GetLocations();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot fetch locations. {ex.Message} | {ex.StackTrace}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
