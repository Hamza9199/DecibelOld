using System.Text;
using Decibel.Data;
using Decibel.Models;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;

namespace Decibel.Services
{
        public class AutentifikacijaServis
        {
                private readonly UserManager<IdentityUser> _userManager;
                private readonly IEmailSender _emailSender;
                private readonly ApplicationDbContext _context;
                private readonly ILogger<AutentifikacijaServis> _logger;
                private readonly SignInManager<IdentityUser> _signInManager;

                public AutentifikacijaServis(
                        UserManager<IdentityUser> userManager,
                        IEmailSender emailSender,
                        ApplicationDbContext context,
                        ILogger<AutentifikacijaServis> logger,
                        SignInManager<IdentityUser> signInManager)
                {
                        _userManager = userManager;
                        _emailSender = emailSender;
                        _context = context;
                        _logger = logger;
                        _signInManager = signInManager;
                }

                public async Task<IdentityResult> RegisterUserAsync(IdentityUser user, string password, string korisnickoIme, string ime, string prezime, string returnUrl)
                {
                        var result = await _userManager.CreateAsync(user, password);

                        if (result.Succeeded)
                        {
                                _logger.LogInformation("User created a new account with password.");

                                _context.Korisnik.Add(new Korisnik
                                {
                                        ID = user.Id,
                                        korisnickoIme = korisnickoIme ?? "",
                                        ime = ime ?? "",
                                        prezime = prezime ?? ""
                                });

                                await _context.SaveChangesAsync();

                                var userId = await _userManager.GetUserIdAsync(user);
                                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                                var callbackUrl = returnUrl;

                                await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                                        $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.");

                                return result;
                        }

                        return result;
                }


                public async Task<SignInResult> LoginAsync(string email, string password, bool rememberMe)
                {
                        var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);

                        if (result.Succeeded)
                        {
                                _logger.LogInformation("User logged in.");
                        }
                        else if (result.RequiresTwoFactor)
                        {
                                _logger.LogInformation("Two-factor authentication required.");
                        }
                        else if (result.IsLockedOut)
                        {
                                _logger.LogWarning("User account locked out.");
                        }
                        else
                        {
                                _logger.LogWarning("Invalid login attempt.");
                        }

                        return result;
                }


                public async Task<bool> UpdateEmailConfirmedAsync(string userId, bool isConfirmed)
                {
                        var user = await _signInManager.UserManager.FindByIdAsync(userId);

                        if (user == null)
                        {
                                return false;
                        }

                        user.EmailConfirmed = isConfirmed;

                        var result = await _signInManager.UserManager.UpdateAsync(user);

                        return result.Succeeded;
                }

        }
}
