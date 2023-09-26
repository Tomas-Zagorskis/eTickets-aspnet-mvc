using eTickets.Data.Services;
using eTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eTickets.Controllers {
    [Authorize]
    public class MoviesController : Controller {
        private readonly IMoviesService _service;

        public MoviesController(IMoviesService service) {
            _service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index() {
            var allMovies = await _service.GetAllAsync(n => n.Cinema);
            return View(allMovies);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Filter(string searchString) {
            var allMovies = await _service.GetAllAsync(n => n.Cinema);

            if (!string.IsNullOrEmpty(searchString)) {
                var filteredResult = allMovies.Where(n => n.Name.ToLower().Contains(searchString.ToLower()) || n.Description.ToLower().Contains(searchString.ToLower())).ToList();
                return View("Index", filteredResult);
            }
            return View("Index", allMovies);
        }

        //GET: Movies/Details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id) {
            var movieDetail = await _service.GetMovieByIdAsync(id);
            return View(movieDetail);
        }

        //GET: Movies/Create
        public async Task<IActionResult> Create() {
            var movieDropdowns = await _service.GetNewMovieDropdownsValues();

            ViewBag.Cinemas = new SelectList(movieDropdowns.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdowns.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdowns.Actors, "Id", "FullName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewMovieVM movie) {
            if (!ModelState.IsValid) {
				var movieDropdowns = await _service.GetNewMovieDropdownsValues();

				ViewBag.Cinemas = new SelectList(movieDropdowns.Cinemas, "Id", "Name");
				ViewBag.Producers = new SelectList(movieDropdowns.Producers, "Id", "FullName");
				ViewBag.Actors = new SelectList(movieDropdowns.Actors, "Id", "FullName");

				return View(movie);
            }

            await _service.AddNewMovieAsync(movie);
            return RedirectToAction(nameof(Index));
        }

		//GET: Movies/Edit/1
		public async Task<IActionResult> Edit(int id) {
            var movieDetails = await _service.GetMovieByIdAsync(id);
            if (movieDetails == null) return View("NotFound");

            var response = new NewMovieVM()
            {
                Id = movieDetails.Id,
                Name = movieDetails.Name,
                Description = movieDetails.Description,
                Price = movieDetails.Price,
                ImageURL = movieDetails.ImageURL,
                MovieCategory = movieDetails.MovieCategory,
                CinemaId = movieDetails.CinemaId,
                ProducerId = movieDetails.ProducerId,
                EndDate = movieDetails.EndDate,
                StartDate = movieDetails.StartDate,
                ActorIds = movieDetails.Actors_Movies.Select(n => n.ActorId).ToList(),
            };

			var movieDropdowns = await _service.GetNewMovieDropdownsValues();

			ViewBag.Cinemas = new SelectList(movieDropdowns.Cinemas, "Id", "Name");
			ViewBag.Producers = new SelectList(movieDropdowns.Producers, "Id", "FullName");
			ViewBag.Actors = new SelectList(movieDropdowns.Actors, "Id", "FullName");

			return View(response);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, NewMovieVM movie) {
            if (id != movie.Id) return View("NotFound");

			if (!ModelState.IsValid) {
				var movieDropdowns = await _service.GetNewMovieDropdownsValues();

				ViewBag.Cinemas = new SelectList(movieDropdowns.Cinemas, "Id", "Name");
				ViewBag.Producers = new SelectList(movieDropdowns.Producers, "Id", "FullName");
				ViewBag.Actors = new SelectList(movieDropdowns.Actors, "Id", "FullName");

				return View(movie);
			}

			await _service.UpdateMovieAsync(movie);
			return RedirectToAction(nameof(Index));
		}

	}
}
