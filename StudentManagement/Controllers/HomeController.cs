using Microsoft.AspNetCore.Mvc;
using StudentManagement.Interfaces;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    /// <summary>
    /// Serves the Movie Theater dashboard and generic error page.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IShowtimeService _showtimeService;
        private readonly IStudentService _studentService;

        public HomeController(
            IShowtimeService showtimeService,
            IStudentService studentService)
        {
            _showtimeService = showtimeService;
            _studentService = studentService;
        }

        public async Task<IActionResult> Index()
        {
            var showtimes =
                (await _showtimeService.GetAllAsync()).ToList();

            var students =
                (await _studentService.GetAllAsync()).ToList();

            var currentDateTime = DateTime.Now;

            ViewBag.TotalShowtimes = showtimes.Count;

            ViewBag.TotalGenres = showtimes
                .Select(s => s.Genre)
                .Where(g => !string.IsNullOrWhiteSpace(g))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count();

            ViewBag.UpcomingShowtimes = showtimes
                .Count(s => s.ShowDateTime >= currentDateTime);

            ViewBag.NextShowtimes = showtimes
                .Where(s => s.ShowDateTime >= currentDateTime)
                .OrderBy(s => s.ShowDateTime)
                .Take(5)
                .ToList();

            ViewBag.TotalStudents = students.Count;

            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = HttpContext.TraceIdentifier
            });
        }
    }
}