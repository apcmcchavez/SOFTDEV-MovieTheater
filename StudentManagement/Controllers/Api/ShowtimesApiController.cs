using Microsoft.AspNetCore.Mvc;
using StudentManagement.Interfaces;
using StudentManagement.ViewModels;

namespace StudentManagement.Controllers.Api
{
    /// <summary>
    /// REST API for movie theater Showtime CRUD operations.
    /// Routed separately from the human-facing MVC pages.
    /// </summary>
    [ApiController]
    [Route("api/showtimes")]
    [Produces("application/json")]
    public class ShowtimesApiController : ControllerBase
    {
        private readonly IShowtimeService _showtimeService;
        private readonly ILogger<ShowtimesApiController> _logger;

        public ShowtimesApiController(
            IShowtimeService showtimeService,
            ILogger<ShowtimesApiController> logger)
        {
            _showtimeService = showtimeService;
            _logger = logger;
        }

        /// <summary>
        /// GET /api/showtimes
        /// Returns all Showtime records.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(
            typeof(IEnumerable<ShowtimeDto>),
            StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ShowtimeDto>>> GetAll()
        {
            var showtimes = await _showtimeService.GetAllAsync();

            return Ok(showtimes);
        }

        /// <summary>
        /// GET /api/showtimes/{id}
        /// Returns one Showtime or 404 when the ID does not exist.
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(
            typeof(ShowtimeDto),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ShowtimeDto>> GetById(Guid id)
        {
            var showtime = await _showtimeService.GetByIdAsync(id);

            if (showtime is null)
            {
                return NotFound(new
                {
                    message = $"Showtime with id {id} was not found."
                });
            }

            return Ok(showtime);
        }

        /// <summary>
        /// POST /api/showtimes
        /// Creates and saves a new Showtime.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(
            typeof(ShowtimeDto),
            StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ShowtimeDto>> Create(
            [FromBody] ShowtimeCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, created, error) =
                await _showtimeService.CreateAsync(dto);

            if (!success)
            {
                return BadRequest(new
                {
                    message = error
                });
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = created!.ShowtimeId },
                created);
        }

        /// <summary>
        /// PUT /api/showtimes/{id}
        /// Updates an existing Showtime.
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(
            typeof(ShowtimeDto),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ShowtimeDto>> Update(
            Guid id,
            [FromBody] ShowtimeCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, updated, error) =
                await _showtimeService.UpdateAsync(id, dto);

            if (!success)
            {
                if (error == "Showtime not found.")
                {
                    return NotFound(new
                    {
                        message = error
                    });
                }

                return BadRequest(new
                {
                    message = error
                });
            }

            return Ok(updated);
        }

        /// <summary>
        /// DELETE /api/showtimes/{id}
        /// Permanently deletes a Showtime.
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _showtimeService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = $"Showtime with id {id} was not found."
                });
            }

            return NoContent();
        }
    }
}