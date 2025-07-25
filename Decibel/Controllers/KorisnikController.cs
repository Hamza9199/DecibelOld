using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Decibel.Data;
using Decibel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using static Dropbox.Api.Files.ListRevisionsMode;
using Firebase.Auth;
using Microsoft.AspNetCore.Http;
namespace Decibel.Controllers
{
        public class KorisnikController : Controller
        {
                private readonly ApplicationDbContext _context;
                private readonly UserManager<IdentityUser> _userManager;
		        private readonly IHttpContextAccessor _httpContextAccessor;

		public KorisnikController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
                {
                        _context = context;
                        _userManager = userManager;
			            _httpContextAccessor = httpContextAccessor;
		    }

                [HttpGet]
                [Route("[Controller]/[Action]")]
                public IActionResult GetStatistika()
                {
                        return View("~/Views/Admin/Index.cshtml");
                }

                [HttpGet]
                [Route("[Controller]/[Action]")]
                public IActionResult GetIzvodjaceSelekcija()
                {
                        return PartialView("_IzvodjacSelekcijaListaPartial");
                }

                
                [HttpGet]
                [Route("[Controller]/[Action]")]
                public async Task<IActionResult> Search(string query)
                {
                        string? korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        var tip = _context.UserRoles
                                .Where(ur => ur.UserId == korisnikID);

                        bool jeIzvodjac = !tip.Where(k => k.RoleId == "2").IsNullOrEmpty();

                        bool jeAdmin = !tip.Where(k => k.RoleId == "3").IsNullOrEmpty();

                        var izvodjaci = _context.Korisnik
                                .Include(nu => nu.AspNetUser);

                        var ret = Enumerable.Empty<Korisnik>().AsQueryable();

                        if (jeIzvodjac)
                        {

                                ret = izvodjaci.Where(i =>
                                        (i.korisnickoIme.Contains(query) || i.AspNetUser.Email.Contains(query)) && 
                                        i.ID != korisnikID);
                        }
                        else
                        {
                                ret = izvodjaci.Where(i =>
                                        i.korisnickoIme.Contains(query) || i.AspNetUser.Email.Contains(query));
                        }

                        var res = ret.Select(i => new { i.ID, i.korisnickoIme, i.putanjaProfilneSlike, i.AspNetUser.Email })
                               .Take(8)
                               .ToList();


                        return Json(res);
                }

                public async Task<bool> ImaPretplatuAsync()
                {
                        var korisnikID = _userManager.GetUserId(User);

                        if (string.IsNullOrEmpty(korisnikID))
                        {
                                return false;
                        }

                        return await _context.KorisnikPretplata
                                .AnyAsync(kp => kp.Korisnik.ID == korisnikID);
                }

                [HttpGet]
                [Route("[Controller]/[Action]")]
                public async Task<IActionResult> ProvjeriPretplatu()
                {
                        var imaPretplatu = await ImaPretplatuAsync();

                        return Json(new { imaPretplatu });
                }

		// GET: Korisnik
		public async Task<IActionResult> Index()
		{
			var korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var korisniciKojePrati = await _context.Pratilac
				.Where(p => p.pratilacID == korisnikID)
				.Select(p => p.korisnikID) 
				.ToListAsync();

			var korisnici = await _context.Korisnik
				.Where(k => korisniciKojePrati.Contains(k.ID)) 
				.Include(k => k.AspNetUser)
				.ToListAsync();

			return View(korisnici); 
		}


		// GET: Korisnik/Lista
		public async Task<IActionResult> Lista()
		        {
			        var applicationDbContext = _context.Korisnik.Include(k => k.AspNetUser);
			        var korisnikID = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);


			        var korisnici = await _context.UserRoles
		                .Where(ur => ur.RoleId == "4") 
		                .Join(_context.Korisnik, ur => ur.UserId, k => k.AspNetUser.Id, (ur, k) => k) 
		                .Include(k => k.AspNetUser) 
		                .ToListAsync();

			ViewData["Korisnici"] = korisnici;

			return View(await applicationDbContext.ToListAsync());
		        }

		// GET: Korisnik/PrijavaIzvodaca/id
		public async Task<IActionResult> PrijavaIzvodaca(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var korisnik = await _context.Korisnik
				.Include(k => k.AspNetUser)
				.FirstOrDefaultAsync(m => m.ID == id);

			if (korisnik == null)
			{
				return NotFound();
			}

			return View(korisnik);
		}

		// GET: Korisnik/Details/5
		public async Task<IActionResult> Details(string id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var korisnik = await _context.Korisnik
                            .Include(k => k.AspNetUser)
                            .FirstOrDefaultAsync(m => m.ID == id);
                        if (korisnik == null)
                        {
                                return NotFound();
                        }

			            var korisnikovePjesmeIds = await _context.IzvodjacPjesma
                            .Where(ip => ip.izvodjacID == id)
                            .Select(ip => ip.pjesmaID)
                            .ToListAsync();

                        

			            var songs = await _context.Pjesma
				            .Where(p => korisnikovePjesmeIds.Contains(p.ID))
				            .ToListAsync();
			            var playlists = await _context.PlayLista
                            .Where(p => p.korisnikID == id)
                            .ToListAsync();

			            var albumi = await _context.Album
				            .Where(a => a.korisnikID == id)
				            .ToListAsync();

			            ViewData["Songs"] = songs;
			            ViewData["Playlists"] = playlists;
			            ViewData["Albums"] = albumi;

			        return View(korisnik);
                }

                // GET: Korisnik/Create
                public IActionResult Create()
                {
                        ViewData["ID"] = new SelectList(_context.Users, "Id", "Id");
                        return View();
                }

                // POST: Korisnik/Create
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create([Bind("ID,ime,prezime,bio,statusKorisnika,putanjaProfilneSlike,datumRegistracije,zadnjaPrijava,brojPratilaca,obrisan")] Korisnik korisnik)
                {
                        if (ModelState.IsValid)
                        {
                                _context.Add(korisnik);
                                await _context.SaveChangesAsync();
                                return RedirectToAction(nameof(Index));
                        }
                        ViewData["ID"] = new SelectList(_context.Users, "Id", "Id", korisnik.ID);
                        return View(korisnik);
                }

                // GET: Korisnik/Edit/5
                public async Task<IActionResult> Edit(string id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var korisnik = await _context.Korisnik.FindAsync(id);
                        if (korisnik == null)
                        {
                                return NotFound();
                        }
                        ViewData["ID"] = new SelectList(_context.Users, "Id", "Id", korisnik.ID);
                        return View(korisnik);
                }

                // POST: Korisnik/Edit/5
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(string id, [Bind("ID,ime,prezime,bio,statusKorisnika,putanjaProfilneSlike,datumRegistracije,zadnjaPrijava,brojPratilaca,obrisan")] Korisnik korisnik)
                {
                        if (id != korisnik.ID)
                        {
                                return NotFound();
                        }

                        if (ModelState.IsValid)
                        {
                                try
                                {
                                        _context.Update(korisnik);
                                        await _context.SaveChangesAsync();
                                }
                                catch (DbUpdateConcurrencyException)
                                {
                                        if (!KorisnikExists(korisnik.ID))
                                        {
                                                return NotFound();
                                        }
                                        else
                                        {
                                                throw;
                                        }
                                }
                                return RedirectToAction(nameof(Index));
                        }
                        ViewData["ID"] = new SelectList(_context.Users, "Id", "Id", korisnik.ID);
                        return View(korisnik);
                }

                // GET: Korisnik/Delete/5
                public async Task<IActionResult> Delete(string id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var korisnik = await _context.Korisnik
                            .Include(k => k.AspNetUser)
                            .FirstOrDefaultAsync(m => m.ID == id);
                        if (korisnik == null)
                        {
                                return NotFound();
                        }

                        return View(korisnik);
                }

                // POST: Korisnik/Delete/5
                [HttpPost, ActionName("Delete")]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> DeleteConfirmed(string id)
                {
                        var korisnik = await _context.Korisnik.FindAsync(id);
                        if (korisnik != null)
                        {
                                _context.Korisnik.Remove(korisnik);
                        }

                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                }



		// POST: Korisnik/Dozvoli/5
		[HttpPost, ActionName("Dozvoli")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Dozvoli(string id)
		        {
			            Console.WriteLine("USOOOO!!!");
			            Console.WriteLine("USOOOO!!!");
			            Console.WriteLine("USOOOO!!!");
			            Console.WriteLine("USOOOO!!!");
			            Console.WriteLine("USOOOO!!!");


			            var korisnik = await _userManager.FindByIdAsync(id);

			            if (korisnik == null)
			            {
				            return NotFound();
			            }

   
			            await _userManager.RemoveFromRoleAsync(korisnik, "4");

			            await _userManager.AddToRoleAsync(korisnik, "2");

		        

			        return RedirectToAction("Lista");
		        }


                // POST: Korisnik/Odbij/5
		        [HttpPost, ActionName("Odbij")]
		[ValidateAntiForgeryToken]

		public async Task<IActionResult> Odbij(string id)
		        {
		    	        Console.WriteLine("USOOOO!!!");
			            Console.WriteLine("USOOOO!!!");
			            Console.WriteLine("USOOOO!!!");
			            Console.WriteLine("USOOOO!!!");
			            Console.WriteLine("USOOOO!!!");


			            var korisnik = await _userManager.FindByIdAsync(id);

			            if (korisnik == null)
			            {
				            return NotFound();
			            }
		

			            await _userManager.RemoveFromRoleAsync(korisnik, "4");


			        return RedirectToAction("Lista");
		        }


		private bool KorisnikExists(string id)
                {
                        return _context.Korisnik.Any(e => e.ID == id);
                }
        }
}
