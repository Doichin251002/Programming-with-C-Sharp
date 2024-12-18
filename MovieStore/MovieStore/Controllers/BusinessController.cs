using Microsoft.AspNetCore.Mvc;
using MovieStore.BL.Interfaces;
using MovieStore.Models.DTO;

namespace MovieStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusinessController : ControllerBase
    {
        private readonly IMovieBlService _movieService;
        private readonly IActorService _actorService;

        public BusinessController(IMovieBlService movieService, IActorService actorService)
        {
            _movieService = movieService;
            _actorService = actorService;
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("GetAllMovieWithDetails")]
        public IActionResult GetAllMovieWithDetails()
        {
            var result = _movieService.GetDetailedMovies();

            if (result == null || result.Count == 0)
            {
                return NotFound("No movies found");
            }

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("AddActor")]
        public IActionResult AddActor([FromBody] Actor actor)
        {
            _actorService.Add(actor);

            return Ok();
        }

    }
}
