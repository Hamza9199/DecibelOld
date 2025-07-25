using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Decibel.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Decibel.Data;
using Decibel.Models;
using Microsoft.AspNetCore.Identity;
using Azure.Core;
using Decibel.Services;
using Microsoft.AspNetCore.Identity.Data;
using System.Security.Claims;

namespace Decibel.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class KorisnikControllerAPI : ControllerBase
        {
                private readonly ApplicationDbContext _context;
                private readonly SignInManager<IdentityUser> _signInManager;
                private readonly UserManager<IdentityUser> _userManager;
                private readonly AutentifikacijaServis _autentifikacijaServis;
		private readonly IHttpContextAccessor _httpContextAccessor;


		public KorisnikControllerAPI(
                        ApplicationDbContext context,
                        SignInManager<IdentityUser> signInManager,
                        UserManager<IdentityUser> userManager,
						 IHttpContextAccessor httpContextAccessor,
						AutentifikacijaServis autentifikacijaServis)
                {
                        _context = context;
                        _signInManager = signInManager;
                        _userManager = userManager;
                        _autentifikacijaServis = autentifikacijaServis;
			_httpContextAccessor = httpContextAccessor;

		}

		[HttpPost("Registracija")]
                public async Task<IActionResult> Registracija([FromBody] RegisterRequest request)
                {
                        if (ModelState.IsValid)
                        {
                                var user = new IdentityUser { UserName = request.Email, Email = request.Email };
                                var result = await _autentifikacijaServis.RegisterUserAsync(user, request.Password, request.KorisnickoIme, request.Ime, request.Prezime, request.ReturnUrl);
                                if (result.Succeeded)
                                {
                                        return Ok(new { message = "User created successfully!", result = user });
                                }
                                return BadRequest(new { message = "Registration failed.", errors = result.Errors });
                        }

                        return BadRequest(new { message = "Invalid input." });
                }

                public class RegisterRequest
                {
                        public string Email { get; set; }
                        public string Password { get; set; }
                        public string ConfirmPassword { get; set; }
                        public string Ime { get; set; }
                        public string Prezime { get; set; }
                        public string KorisnickoIme { get; set; }
                        public string ReturnUrl { get; set; }
                }

                [HttpPost("Login")]
                public async Task<IActionResult> Login([FromBody] LoginRequest request, [FromQuery] string returnUrl = "/")
                {
                        if (ModelState.IsValid)
                        {
                                var result = await _autentifikacijaServis.LoginAsync(request.Email, request.Password, request.RememberMe);

                                if (result.Succeeded)
                                {
                                        return Ok(new { message = "Login successful", returnUrl = returnUrl, result = result});
                                }

                                if (result.RequiresTwoFactor)
                                {
                                        return BadRequest(new { message = "Two-factor authentication required" });
                                }

                                if (result.IsLockedOut)
                                {
                                        return BadRequest(new { message = "Account locked out" });
                                }

                                return BadRequest(new { message = "Invalid login attempt" });
                        }

                        return BadRequest(new { message = "Invalid input" });
                }

                public class LoginRequest
                {
                        public string Email { get; set; }
                        public string Password { get; set; }
                        public bool RememberMe { get; set; }
                }

                [HttpPut("UpdateEmailConfirmed")]
                public async Task<IActionResult> UpdateEmailConfirmed([FromBody] UpdateEmailConfirmedRequest request)
                {
                        if (string.IsNullOrEmpty(request.UserId))
                        {
                                return BadRequest(new { message = "UserId is required" });
                        }

                        var success = await _autentifikacijaServis.UpdateEmailConfirmedAsync(request.UserId, request.EmailConfirmed);

                        if (!success)
                        {
                                return NotFound(new { message = "User not found or update failed" });
                        }

                        return Ok(new { message = "EmailConfirmed status updated successfully" });
                }

                public class UpdateEmailConfirmedRequest
                {
                        public string UserId { get; set; }
                        public bool EmailConfirmed { get; set; }
                }

                // GET: api/KorisnikControllerAPI
                [HttpGet]
                public async Task<ActionResult<IEnumerable<Korisnik>>> GetKorisnik()
                {
                        return await _context.Korisnik.ToListAsync();
                }

                // GET: api/KorisnikControllerAPI/5
                [HttpGet("{id}")]
                public async Task<ActionResult<Korisnik>> GetKorisnik(string id)
                {
                        var korisnik = await _context.Korisnik.FindAsync(id);

                        if (korisnik == null)
                        {
                                return NotFound();
                        }

                        return korisnik;
                }

                [HttpGet("Email/{email}")]
                public async Task<ActionResult<object>> GetKorisnikByEmail(string email)
                {
                        var aspNetUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

                        if (aspNetUser == null)
                        {
                                return NotFound(new { Message = "AspNetUser nije pronadjen." });
                        }

                        var korisnik = await _context.Korisnik.FindAsync(aspNetUser.Id);

                        if (korisnik == null)
                        {
                                return NotFound(new { Message = "Korisnik nije pronadjen." });
                        }

                        var combinedResult = new
                        {
                                Korisnik = korisnik
                        };

                        return Ok(combinedResult);
                }


                // PUT: api/KorisnikControllerAPI/5
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPut("{id}")]
                public async Task<IActionResult> PutKorisnik(string id, Korisnik korisnik)
                {
                        if (id != korisnik.ID)
                        {
                                return BadRequest();
                        }

                        _context.Entry(korisnik).State = EntityState.Modified;

                        try
                        {
                                await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                                if (!KorisnikExists(id))
                                {
                                        return NotFound();
                                }
                                else
                                {
                                        throw;
                                }
                        }

                        return NoContent();
                }

                // POST: api/KorisnikControllerAPI
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPost]
                public async Task<ActionResult<Korisnik>> PostKorisnik(Korisnik korisnik)
                {
                        _context.Korisnik.Add(korisnik);
                        try
                        {
                                await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateException)
                        {
                                if (KorisnikExists(korisnik.ID))
                                {
                                        return Conflict();
                                }
                                else
                                {
                                        throw;
                                }
                        }

                        return CreatedAtAction("GetKorisnik", new { id = korisnik.ID }, korisnik);
                }

                // DELETE: api/KorisnikControllerAPI/5
                [HttpDelete("{id}")]
                public async Task<IActionResult> DeleteKorisnik(string id)
                {
                        var korisnik = await _context.Korisnik.FindAsync(id);
                        if (korisnik == null)
                        {
                                return NotFound();
                        }

                        _context.Korisnik.Remove(korisnik);
                        await _context.SaveChangesAsync();

                        return NoContent();
                }


		[HttpPost("dozvoli/{id}")]
		public async Task<IActionResult> DozvoliAsync(string id)
		{
			try
			{
				var zahtjev = await _userManager.FindByIdAsync(id);
				if (zahtjev == null)
				{
					return NotFound(new { message = "Zahtjev nije pronađen." });
				}

				var userRoles = await _context.UserRoles.Where(ur => ur.UserId == id).ToListAsync();
				var roleToRemove = userRoles.FirstOrDefault(ur => ur.RoleId == "4");
				if (roleToRemove != null)
				{
					_context.UserRoles.Remove(roleToRemove);
				}

				var userRoles2 = await _context.UserRoles.Where(ur => ur.UserId == id).ToListAsync();
				var roleToRemove2 = userRoles2.FirstOrDefault(ur => ur.RoleId == "2");
				if (roleToRemove2 != null)
				{
					_context.UserRoles.Remove(roleToRemove2);
				}

				var addRoleResult = await _context.UserRoles.AddAsync(new IdentityUserRole<string> { UserId = id, RoleId = "2" });
				if (addRoleResult == null)
				{
					return BadRequest(new { message = "Greška pri dodavanju nove uloge." });
				}

				await _context.SaveChangesAsync();

				return Ok(new { message = "Zahtjev je uspješno odobren." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Interna greška servera", error = ex.Message });
			}
		}


		[HttpPost("odbij/{id}")]
		public async Task<IActionResult> OdbijAsync(string id)
		{
            try
            {
                var zahtjev = await _userManager.FindByIdAsync(id);
                if (zahtjev == null)
                {
                    return NotFound(new { message = "Zahtjev nije pronađen." });
                }

				var userRoles = await _context.UserRoles.Where(ur => ur.UserId == id).ToListAsync();
				var roleToRemove = userRoles.FirstOrDefault(ur => ur.RoleId == "4");
				if (roleToRemove != null)
				{
					_context.UserRoles.Remove(roleToRemove);
				}

				

				await _context.SaveChangesAsync();

                return Ok(new { message = "Zahtjev je uspješno odbijen.", zahtjev });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Interna greška servera", error = ex.Message });
            }
		}

		[HttpPost("zahtjev/{id}")]
		public async Task<IActionResult> ZahtjevAsync(string id)
		{
			try
			{
				var zahtjev = await _userManager.FindByIdAsync(id);
				if (zahtjev == null)
				{
					return NotFound(new { message = "Zahtjev nije pronađen." });
				}

				var userRoles = await _context.UserRoles.Where(ur => ur.UserId == id).ToListAsync();
				var roleToRemove = userRoles.FirstOrDefault(ur => ur.RoleId == "4");
				if (roleToRemove != null)
				{
					_context.UserRoles.Remove(roleToRemove);
				}

				var addRoleResult = await _context.UserRoles.AddAsync(new IdentityUserRole<string> { UserId = id, RoleId = "4" });
				if (addRoleResult == null)
				{
					return BadRequest(new { message = "Greška pri dodavanju nove uloge." });
				}

				await _context.SaveChangesAsync();

				return Ok(new { message = "Zahtjev je uspješno odobren." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Interna greška servera", error = ex.Message });
			}
		}


		[HttpPost("updateStatus/{userId}")]
		public async Task<IActionResult> UpdateStatus(string userId, [FromBody] StatusUpdateModel statusUpdate)
		{
			var korisnik = await _context.Korisnik.FindAsync(userId);

			if (korisnik == null)
			{
				return NotFound(new { message = "Korisnik nije pronađen" });
			}

			korisnik.statusKorisnika = (KorisnikStatusEnum)statusUpdate.Status;

			_context.Korisnik.Update(korisnik);
			await _context.SaveChangesAsync();

			return Ok(new { message = "Status uspešno ažuriran na: " + statusUpdate.Status + korisnik.statusKorisnika });
		}

		public class StatusUpdateModel
		{
			public int Status { get; set; }
		}

		[HttpPost("follow/{id}")]
		public async Task<IActionResult> Follow(string id)
		{
			try
			{
				var korisnik = await _context.Korisnik.FindAsync(id);

				if (korisnik == null)
				{
					return NotFound(new { message = "Korisnik nije pronađen" });
				}

				var korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                PratilacKorisnik novi = new PratilacKorisnik();
                novi.pratilacID = korisnikID;

                novi.korisnikID = id;

                 _context.Pratilac.Add(novi);

                korisnik.brojPratilaca++;
				_context.Korisnik.Update(korisnik);


				try
				{
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateException)
				{
                    
				}

				return Ok(new { message = "Korisnik zapraćen." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Interna greška servera", error = ex.Message });
			}
		}



		[HttpPost("unfollow/{id}")]
		public async Task<IActionResult> Unfollow(string id)
		{
			try
			{
				var korisnik = await _context.Korisnik.FindAsync(id);

				if (korisnik == null)
				{
					return NotFound(new { message = "Korisnik nije pronađen" });
				}

				var korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

				var pratilac = await _context.Pratilac
											  .FirstOrDefaultAsync(p => p.korisnikID == id && p.pratilacID == korisnikID);

				if (pratilac == null)
				{
					return BadRequest(new { message = "Ne pratite ovog korisnika." });
				}

				_context.Pratilac.Remove(pratilac);



                if(korisnik.brojPratilaca > 0)
                {
					korisnik.brojPratilaca--;
					_context.Korisnik.Update(korisnik);
				}		

				try
				{
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateException)
				{
					return StatusCode(500, new { message = "Greška prilikom uklanjanja pratilaca" });
				}

				return Ok(new { message = "Korisnik je prestao biti praćen." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Interna greška servera", error = ex.Message });
			}
		}





		private bool KorisnikExists(string id)
                {
                        return _context.Korisnik.Any(e => e.ID == id);
                }
        }
}
