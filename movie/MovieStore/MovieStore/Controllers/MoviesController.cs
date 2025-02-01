using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using MovieStore.BL.Interfaces;
using MovieStore.Models.DTO;
using MovieStore.Models.Requests.Movie;

namespace MovieStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(
            IMovieService movieService,
            IMapper mapper,
            ILogger<MoviesController> logger)
        {
            _movieService = movieService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("AddMovie")]
        public IActionResult Add(AddMovieRequest movie)
        {
            try
            {
                var movieDto = _mapper.Map<Movie>(movie);

                if (movieDto == null)
                {
                    return BadRequest("No valid data.");
                }

                _movieService.Add(movieDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding movie with");
                return BadRequest(ex.Message);
            }

            return Ok($"Movie {movie.Title} added successfully.");
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("GetAllMovies")]
        public IActionResult Get()
        {
            var result = _movieService.GetAll();

            if (result == null || result.Count == 0)
            {
                return NotFound("No movies found");
            }

            return Ok(result);
        }

        [HttpGet("GetMovieById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Id can't be null or empty");
            }

            try
            {
                var result = _movieService.GetById(id);

                if (result == null)
                {
                    return NotFound($"Movie with ID:{id} not found");
                }

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("UpdateMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Update(UpdateMovieRequest movie)
        {
            var movieDto = _mapper.Map<Movie>(movie);

            if (movieDto == null)
            {
                return BadRequest("Failed to map request to movie DTO.");
            }

            if (string.IsNullOrEmpty(movieDto.Id))
            {
                return BadRequest("Invalid movie data or missing ID.");
            }

            try
            {
                var existingMovie = _movieService.GetById(movieDto.Id);

                if (existingMovie == null)
                {
                    return NotFound($"Movie with ID {movie.Id} not found.");
                }

                _movieService.Update(movieDto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating movie with ID {movie.Id}");
                return StatusCode(400, ex.Message);
            }

            return Ok($"Movie with ID {movie.Id} successfully updated.");
        }

        [HttpDelete("DeleteMovie")]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest($"Movie with id {id} not found");
            }

            try
            {
                _movieService.Delete(id);
            }
            catch (Exception)
            {
                return BadRequest($"Movie with ID {id} not found.");
            }

            return Ok($"Movie with {id} deleted successfully.");
        }
    }
}
