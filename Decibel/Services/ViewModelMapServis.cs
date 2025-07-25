using Decibel.Data;
using Decibel.Dto;
using Decibel.Models;
using Decibel.ViewModels;
using Dropbox.Api.TeamLog;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Decibel.Services
{
        /**
         *      Utility klasa koja sluzi za Mapiranje Modela u ViewModele
         */
        public class ViewModelMapServis
        {
                public static List<PjesmaViewModel> MapirajUPjesmeViewModel(IQueryable<PjesmaDto> pjesmeDto)
                {
                        var query = pjesmeDto.Select(pjesmaDto => new PjesmaViewModel
                        {
                                ID = pjesmaDto.Pjesma.ID,
                                prev = pjesmaDto.prev,
                                next = pjesmaDto.next,
                                eksplicitniSadrzaj = pjesmaDto.Pjesma.eksplicitniSadrzaj,
                                contextURL = pjesmaDto.contextURL,
                                context = pjesmaDto.context,
                                javno = pjesmaDto.Pjesma.javno,
                                odobreno = pjesmaDto.Pjesma.odobreno,
                                albumID = pjesmaDto.Pjesma.albumID,
                                korisnikID = pjesmaDto.Pjesma.korisnikID,
                                korisnikIme = pjesmaDto.Pjesma.Korisnik != null ? pjesmaDto.Pjesma.Korisnik.korisnickoIme : null, // Null check for Korisnik
                                lajkovana = pjesmaDto.lajkovana,
                                albumIme = pjesmaDto.Pjesma.Album != null ? pjesmaDto.Pjesma.Album.naziv : null, // Null check for Album
                                naziv = pjesmaDto.Pjesma.naziv,
                                putanjaSlika = pjesmaDto.Pjesma.putanjaSlika == null && pjesmaDto.Pjesma.albumID != null
                                        ? pjesmaDto.Pjesma.Album != null ? pjesmaDto.Pjesma.Album.putanjaSlika : null // Null check for Album's putanjaSlika
                                        : pjesmaDto.Pjesma.putanjaSlika,
                                putanjaGIF = pjesmaDto.Pjesma.putanjaGif,
                                putanjaAudio = pjesmaDto.Pjesma.putanjaAudio,
                                trajanje = TimeSpan.FromSeconds(pjesmaDto.Pjesma.trajanjeSekunde).ToString(@"hh\:mm\:ss"),
                                datumDodano = pjesmaDto.DodanDatumVrijeme,
                                Izvodjaci = pjesmaDto.Pjesma.IzvodjacPjesma != null
                                        ? pjesmaDto.Pjesma.IzvodjacPjesma.Select(ip => ip.Izvodjac)
                                                .Where(izvodjac => izvodjac != null) // Ensure that Izvodjac is not null
                                                .Select(izvodjac => new IzvodjacViewModel
                                                {
                                                        ID = izvodjac.ID,
                                                        korisnickoIme = izvodjac.korisnickoIme,
                                                        putanjaProfilneSlike = izvodjac.putanjaProfilneSlike
                                                }).ToList()
                                        : new List<IzvodjacViewModel>() // If IzvodjacPjesma is null, return an empty list
                        }).ToList();

                        return query;
                }

                public static PjesmaViewModel MapirajUPjesmeViewModel(PjesmaDto pjesmaDto)
                {
                        var query = new PjesmaViewModel
                        {
                                ID = pjesmaDto.Pjesma.ID,
                                prev = pjesmaDto.prev,
                                next = pjesmaDto.next,
                                eksplicitniSadrzaj = pjesmaDto.Pjesma.eksplicitniSadrzaj,
                                contextURL = pjesmaDto.contextURL,
                                context = pjesmaDto.context,
                                javno = pjesmaDto.Pjesma.javno,
                                odobreno = pjesmaDto.Pjesma.odobreno,
                                albumID = pjesmaDto.Pjesma.albumID,
                                korisnikID = pjesmaDto.Pjesma.korisnikID,
                                korisnikIme = pjesmaDto.Pjesma.Korisnik != null ? pjesmaDto.Pjesma.Korisnik.korisnickoIme : null, // Null check for Korisnik
                                lajkovana = pjesmaDto.lajkovana,
                                albumIme = pjesmaDto.Pjesma.Album != null ? pjesmaDto.Pjesma.Album.naziv : null, // Null check for Album
                                naziv = pjesmaDto.Pjesma.naziv,
                                putanjaSlika = pjesmaDto.Pjesma.putanjaSlika == null && pjesmaDto.Pjesma.albumID != null
                                        ? pjesmaDto.Pjesma.Album != null ? pjesmaDto.Pjesma.Album.putanjaSlika : null // Null check for Album's putanjaSlika
                                        : pjesmaDto.Pjesma.putanjaSlika,
                                putanjaGIF = pjesmaDto.Pjesma.putanjaGif,
                                putanjaAudio = pjesmaDto.Pjesma.putanjaAudio,
                                trajanje = TimeSpan.FromSeconds(pjesmaDto.Pjesma.trajanjeSekunde).ToString(@"hh\:mm\:ss"),
                                datumDodano = pjesmaDto.DodanDatumVrijeme,
                                Izvodjaci = pjesmaDto.Pjesma.IzvodjacPjesma != null
                                        ? pjesmaDto.Pjesma.IzvodjacPjesma.Select(ip => ip.Izvodjac)
                                                .Where(izvodjac => izvodjac != null) // Ensure that Izvodjac is not null
                                                .Select(izvodjac => new IzvodjacViewModel
                                                {
                                                        ID = izvodjac.ID,
                                                        korisnickoIme = izvodjac.korisnickoIme,
                                                        putanjaProfilneSlike = izvodjac.putanjaProfilneSlike
                                                }).ToList()
                                        : new List<IzvodjacViewModel>() // If IzvodjacPjesma is null, return an empty list
                        };

                        return query;
                }

        }
}
