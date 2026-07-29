using Microsoft.AspNetCore.Mvc;
using StudentManagement.Interfaces;
using StudentManagement.ViewModels;

namespace StudentManagement.Controllers
{
    /// <summary>
    /// MVC controller for the human-facing Showtime Razor pages.
    /// </summary>
    public class ShowtimesController : Controller
    {
        private readonly IShowtimeService _showtimeService;
        private readonly ILogger<ShowtimesController> _logger;

        public ShowtimesController(
            IShowtimeService showtimeService,
            ILogger<ShowtimesController> logger)
        {
            _showtimeService = showtimeService;
            _logger = logger;
        }

        // GET: /Showtimes
        public async Task<IActionResult> Index(
            string? searchTerm,
            string sortColumn = "ShowDateTime",
            string sortDirection = "asc",
            int page = 1,
            int pageSize = 10)
        {
            var viewModel =
                await _showtimeService.GetShowtimeListAsync(
                    searchTerm,
                    sortColumn,
                    sortDirection,
                    page,
                    pageSize);

            return View(viewModel);
        }

        // GET: /Showtimes/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            var showtime = await _showtimeService.GetByIdAsync(id);

            if (showtime is null)
            {
                return NotFound();
            }

            return View(showtime);
        }

        // GET: /Showtimes/Create
        public IActionResult Create()
        {
            return View(new ShowtimeViewModel
            {
                ShowDateTime = DateTime.Now.AddHours(1)
            });
        }

        // POST: /Showtimes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            ShowtimeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dto = ToDto(model);

            var (success, created, error) =
                await _showtimeService.CreateAsync(dto);

            if (!success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    error ?? "Unable to create the showtime.");

                return View(model);
            }

            TempData["SuccessMessage"] =
                $"Showtime for '{created!.Title}' was created successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: /Showtimes/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var showtime = await _showtimeService.GetByIdAsync(id);

            if (showtime is null)
            {
                return NotFound();
            }

            var model = new ShowtimeViewModel
            {
                ShowtimeId = showtime.ShowtimeId,
                MovieCode = showtime.MovieCode,
                Title = showtime.Title,
                Genre = showtime.Genre,
                ShowDateTime = showtime.ShowDateTime,
                TicketPrice = showtime.TicketPrice,
                DateCreated = showtime.DateCreated,
                DateUpdated = showtime.DateUpdated
            };

            return View(model);
        }

        // POST: /Showtimes/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id,
            ShowtimeViewModel model)
        {
            if (id != model.ShowtimeId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dto = ToDto(model);

            var (success, updated, error) =
                await _showtimeService.UpdateAsync(id, dto);

            if (!success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    error ?? "Unable to update the showtime.");

                return View(model);
            }

            TempData["SuccessMessage"] =
                $"Showtime for '{updated!.Title}' was updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: /Showtimes/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            var showtime = await _showtimeService.GetByIdAsync(id);

            if (showtime is null)
            {
                return NotFound();
            }

            return View(showtime);
        }

        // POST: /Showtimes/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var deleted = await _showtimeService.DeleteAsync(id);

            if (!deleted)
            {
                TempData["ErrorMessage"] =
                    "Showtime could not be found or was already deleted.";
            }
            else
            {
                TempData["SuccessMessage"] =
                    "Showtime was deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        private static ShowtimeCreateUpdateDto ToDto(
            ShowtimeViewModel model)
        {
            return new ShowtimeCreateUpdateDto
            {
                MovieCode = model.MovieCode,
                Title = model.Title,
                Genre = model.Genre,
                ShowDateTime = model.ShowDateTime,
                TicketPrice = model.TicketPrice
            };
        }
    }
}