using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using MovieStore.BL.Interfaces;
using MovieStore.Models.DTO;
using MovieStore.Models.Requests.Actor;

namespace MovieStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusinessController : ControllerBase
    {
        private readonly IMovieBlService _movieService;
        private readonly IActorService _actorService;
        private readonly IMapper _mapper;
        private readonly ILogger<BusinessController> _logger;

        public BusinessController(IMovieBlService movieService,
            IActorService actorService,
            IMapper mapper,
            ILogger<BusinessController> logger)
        {
            _movieService = movieService;
            _actorService = actorService;
            _mapper = mapper;
            _logger = logger;
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("GetAllMoviesWithDetails")]
        public IActionResult GetAllMoviesWithDetails()
        {
            try
            {
                var result = _movieService.GetDetailedMovies();

                if (result == null || result.Count == 0)
                {
                    return NotFound("No movies found");
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(400, "An unexpected error occurred.");
            }
        }

        [HttpPost("AddActor")]
        public IActionResult Add([FromBody] AddActorRequest actor)
        {
            try
            {
                var actorDto = _mapper.Map<Actor>(actor);

                if (actorDto == null)
                {
                    return BadRequest("No valid data.");
                }

                _actorService.Add(actorDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding actor with");
                return BadRequest(ex.Message);
            }

            return Ok($"Actor {actor.Name} added successfully.");
        }


        [HttpGet("GetActorById")]
        public IActionResult GetById([FromQuery] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Actor ID cannot be null or empty.");
            }

            try
            {
                var actor = _actorService.GetById(id);

                if (actor == null)
                {
                    return NotFound($"Actor with ID {id} not found.");
                }

                return Ok(actor);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }


        }

        [HttpGet("GetActorsByIds")]
        public IActionResult GetByIds([FromQuery] IEnumerable<string> actorsIds)
        {
            if (actorsIds == null || !actorsIds.Any())
            {
                return BadRequest("Actor IDs cannot be null or empty.");
            }

            try
            {
                var actors = _actorService.GetByIds(actorsIds);

                if (actors == null || !actors.Any())
                {
                    return NotFound("No actors found with the provided IDs.");
                }

                return Ok(actors);
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

        [HttpGet("GetAllActors")]
        public IActionResult GetAll()
        {
            var result = _actorService.GetAll();

            if (result == null || result.Count == 0)
            {
                return NotFound("No actors found");
            }

            return Ok(result);
        }

        [HttpPut("UpdateActor")]
        public IActionResult Update([FromBody] UpdateActorRequest actor)
        {
            var actorDTO = _mapper.Map<Actor>(actor);

            try
            {
                if (actor == null)
                {
                    return BadRequest("Actor cannot be null.");
                }

                _actorService.Update(actorDTO);
                return Ok($"Actor with id {actor.Id} updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating actor.");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteActor")]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Actor cannot be null or empty.");
            }

            try
            {
                _actorService.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting actor.");
                return BadRequest(ex.Message);
            }

            return Ok($"Actor with id {id} deleted successfully.");
        }

    }

}
