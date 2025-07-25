using Decibel.Data;
using Decibel.Models;

namespace Decibel.Services
{
        public class KorisnikServis
        {
                private readonly ApplicationDbContext _context;

                public KorisnikServis(ApplicationDbContext context)
                {
                        _context = context;
                }

                public async Task<Korisnik> GetKorisnikByIdAsync(string id)
                {
                        return await _context.Korisnik.FindAsync(id);
                }

                public async Task<Korisnik> UpdateProfilePictureAsync(string userId, IFormFile slika, FirebaseService _firebaseService)
                {
                        var korisnik = await GetKorisnikByIdAsync(userId);

                        korisnik.ImageFile = slika;

                        if (korisnik != null)
                        {
                                await Upload(korisnik, _firebaseService);

                                await _context.SaveChangesAsync();
                        }

                        return korisnik;
                }

                public async static Task Upload(Korisnik k, FirebaseService _firebaseService)
                {

                        if (k.ImageFile != null)
                        {
                                try
                                {
                                        k.putanjaProfilneSlike = await _firebaseService.UploadFileAsync(
                                                k.ImageFile.OpenReadStream(),
                                                k.ImageFile.FileName,
                                                k.ImageFile.ContentType
                                        );
                                }
                                catch (Exception ex)
                                {
                                        Console.WriteLine($"Error tokom uploada: {ex.Message}");
                                        Console.WriteLine(ex.StackTrace);
                                }
                        }
                }
        }
}
