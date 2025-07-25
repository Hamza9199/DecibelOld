using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Decibel.Models
{
        public class Korisnik
        {
                [Key]
                [ForeignKey("AspNetUsers")]
                public string ID { get; set; }

                public string korisnickoIme { get; set; }

                public string ime { get; set; }

                public string prezime { get; set; }

                public string? bio { get; set; }

                [EnumDataType(typeof(KorisnikStatusEnum))]
                public KorisnikStatusEnum statusKorisnika { get; set; }

                public string? putanjaProfilneSlike { get; set; }

                public DateTime datumRegistracije { get; set; }

                public DateTime? zadnjaPrijava { get; set; }

                public UInt64 brojPratilaca { get; set; }

                public bool obrisan { get; set; }

                [NotMapped]
                [Display(Name = "Slika Datoteka")]
                public IFormFile? ImageFile { get; set; }

                public IdentityUser AspNetUser { get; set; }

                public ICollection<PlayLista>? PlayLista { get; set; }

                public ICollection<Pjesma>? Pjesma { get; set; }

                public ICollection<KorisnikPjesma>? KorisnikPjesma { get; set; }

                public ICollection<Album>? Album { get; set; }

                public ICollection<Komentar>? Komentar { get; set; }

                public ICollection<IzvodjacPjesma> IzvodjacPjesma { get; set; }

                public ICollection<HistorijaSlusanja>? HistorijaSlusanja { get; set; }

                public ICollection<StatistikaReprodukcije>? StatistikaReprodukcije { get; set; }


                public Korisnik() { }

        }
}
