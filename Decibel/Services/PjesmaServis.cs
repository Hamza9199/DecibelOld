using Decibel.Models;

namespace Decibel.Services
{
        public class PjesmaServis
        {
                public static async Task<Pjesma> Kreiraj(string korisnikID, Pjesma pjesma, FirebaseService _firebaseService)
                {
                        int trajanjePjesme = (int)AudioServis.GetAudioTrajanje(pjesma.AudioFile.OpenReadStream());

                        pjesma.trajanjeSekunde = (uint)trajanjePjesme;

                        if (pjesma.eksplicitniSadrzaj == null)
                        {
                                pjesma.eksplicitniSadrzaj = false;
                        }

                        await Upload(pjesma, _firebaseService);

                        pjesma.korisnikID = korisnikID;

                        return pjesma;
                }

                public static async Task<Album> KreirajAlbum(string korisnikID, Album album, FirebaseService _firebaseService)
                {
                        await Upload(album, _firebaseService);

                        return album;
                }

                public async static Task Upload(Album album, FirebaseService _firebaseService)
                {
                                Console.WriteLine("\n\n\nIN HERE\n\n\n");
                        if (album.ImageFile != null)
                        {
                                try
                                {
                                        album.putanjaSlika = await _firebaseService.UploadFileAsync(
                                                album.ImageFile.OpenReadStream(),
                                                album.ImageFile.FileName,
                                                album.ImageFile.ContentType
                                        );
                                }
                                catch (Exception ex)
                                {
                                        Console.WriteLine($"Error tokom uploada: {ex.Message}");
                                        Console.WriteLine(ex.StackTrace);
                                }
                        }

                        if (album.GifFile != null)
                        {
                                try
                                {
                                        album.putanjaGif = await _firebaseService.UploadFileAsync(
                                                album.GifFile.OpenReadStream(),
                                                album.GifFile.FileName,
                                                album.GifFile.ContentType
                                        );
                                }
                                catch (Exception ex)
                                {
                                        Console.WriteLine($"Error tokom uploada: {ex.Message}");
                                        Console.WriteLine(ex.StackTrace);
                                }
                        }
                }

                public async static Task Upload(Pjesma pjesma, FirebaseService _firebaseService)
                {
                        if (pjesma.AudioFile != null)
                        {
                                try
                                {
                                        pjesma.putanjaAudio = await _firebaseService.UploadFileAsync(
                                                pjesma.AudioFile.OpenReadStream(),
                                                pjesma.AudioFile.FileName,
                                                pjesma.AudioFile.ContentType
                                        );
                                }
                                catch (Exception ex)
                                {
                                        Console.WriteLine($"Error tokom uploada: {ex.Message}");
                                        Console.WriteLine(ex.StackTrace);
                                }
                        }

                        if (pjesma.ImageFile != null)
                        {
                                try
                                {
                                        pjesma.putanjaSlika = await _firebaseService.UploadFileAsync(
                                                pjesma.ImageFile.OpenReadStream(),
                                                pjesma.ImageFile.FileName,
                                                pjesma.ImageFile.ContentType
                                        );
                                }
                                catch (Exception ex)
                                {
                                        Console.WriteLine($"Error tokom uploada: {ex.Message}");
                                        Console.WriteLine(ex.StackTrace);
                                }
                        }

                        if (pjesma.GifFile != null)
                        {
                                try
                                {
                                        pjesma.putanjaGif = await _firebaseService.UploadFileAsync(
                                                pjesma.GifFile.OpenReadStream(),
                                                pjesma.GifFile.FileName,
                                                pjesma.GifFile.ContentType
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
