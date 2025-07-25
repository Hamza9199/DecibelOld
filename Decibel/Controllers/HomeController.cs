using Decibel.Data;
using Decibel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;
	private readonly ApplicationDbContext _context;

	public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
	{
		_logger = logger;
		_context = context;
	}

	[HttpGet]
	[Route("")]
	[Route("[Controller]/[Action]")]
	public async Task<IActionResult> Index()
	{
		var songs = await _context.Pjesma
						.Where(doz => doz.odobreno == true)
						.ToListAsync();

		var playlists = await _context.PlayLista
						.ToListAsync();

		var albumi = await _context.Album
						.Where(doz => doz.odobreno == true)
						.ToListAsync();

		var izvodaci = await _context.UserRoles
						.Where(ur => ur.RoleId == "2")
						.Join(_context.Korisnik, ur => ur.UserId, k => k.AspNetUser.Id, (ur, k) => k)
						.Where(k => k.statusKorisnika == KorisnikStatusEnum.Aktivan)
						.Include(k => k.AspNetUser)
						.ToListAsync();

		ViewData["Songs"] = songs;
		ViewData["Playlists"] = playlists;
		ViewData["Albums"] = albumi;
		ViewData["Izvodaci"] = izvodaci;

		return View();
	}

	[HttpGet]
	[Route("[Controller]/[Action]")]
	public IActionResult Privacy()
	{
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
