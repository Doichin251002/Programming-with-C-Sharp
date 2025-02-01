using BookStore.BL.Interfaces;
using BookStore.Models.DTO;
using BookStore.Models.Requests.Author;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusinessController : ControllerBase
    {
        private readonly IBookBlService _bookService;
        private readonly IAuthorService _authorService;
        private readonly IMapper _mapper;
        private readonly ILogger<BusinessController> _logger;

        public BusinessController(IBookBlService bookService,
            IAuthorService authorService,
            IMapper mapper,
            ILogger<BusinessController> logger)
        {
            _bookService = bookService;
            _authorService = authorService;
            _mapper = mapper;
            _logger = logger;
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("GetAllBooksWithDetails")]
        public IActionResult GetAllBooksWithDetails()
        {
            try
            {
                var result = _bookService.GetDetailedBooks();

                if (result == null || result.Count == 0)
                {
                    return NotFound("No books found");
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(400, "An unexpected error occurred.");
            }
        }

        [HttpPost("AddAuthor")]
        public IActionResult Add([FromBody] AddAuthorRequest author)
        {
            try
            {
                var authorDto = _mapper.Map<Author>(author);

                if (authorDto == null)
                {
                    return BadRequest("No valid data.");
                }

                _authorService.Add(authorDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding author with");
                return BadRequest(ex.Message);
            }

            return Ok($"Author {author.Name} added successfully.");
        }


        [HttpGet("GetAuthorById")]
        public IActionResult GetById([FromQuery] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Author ID cannot be null or empty.");
            }

            try
            {
                var author = _authorService.GetById(id);

                if (author == null)
                {
                    return NotFound($"Author with ID {id} not found.");
                }

                return Ok(author);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetAuthorsByIds")]
        public IActionResult GetByIds([FromQuery] IEnumerable<string> authorsIds)
        {
            if (authorsIds == null || !authorsIds.Any())
            {
                return BadRequest("Author IDs cannot be null or empty.");
            }

            try
            {
                var authors = _authorService.GetByIds(authorsIds);

                if (authors == null || !authors.Any())
                {
                    return NotFound("No authors found with the provided IDs.");
                }

                return Ok(authors);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(400, "An unexpected error occurred: " + ex.Message);
            }
        }

        [HttpGet("GetAllAuthors")]
        public IActionResult GetAll()
        {
            var result = _authorService.GetAll();

            if (result == null || result.Count == 0)
            {
                return NotFound("No authors found");
            }

            return Ok(result);
        }

        [HttpPut("UpdateAuthor")]
        public IActionResult Update([FromBody] UpdateAuthorRequest author)
        {
            var authorDTO = _mapper.Map<Author>(author);

            try
            {
                if (author == null)
                {
                    return BadRequest("Author cannot be null.");
                }

                _authorService.Update(authorDTO);
                return Ok($"Author with ID {author.Id} updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating author.");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteAuthor")]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Author cannot be null or empty.");
            }

            try
            {
                _authorService.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting author.");
                return BadRequest(ex.Message);
            }

            return Ok($"Author with ID {id} deleted successfully.");
        }

    }

}
