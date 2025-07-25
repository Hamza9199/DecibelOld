using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Decibel.Models
{
        public class Pjesma
        {
                [Key]
                public Int64 ID { get; set; }

                [ForeignKey("Album")]
                public Int64? albumID { get; set; }

                [ForeignKey("Korisnik")]
                public string? korisnikID { get; set; }

                public Int64? redniBrojUAlbumu { get; set; }

                [Display(Name = "Naziv")]
                [Required(ErrorMessage = "Naziv je obavezan.")]
                public string naziv { get; set; }

                [Display(Name = "Opis")]
                public string? opis { get; set; }

                [Display(Name = "Datum Objave")]
                [Required(ErrorMessage = "Datum Objave je obavezan.")]
                public DateOnly datumObjave { get; set; }

                [Display(Name = "Trajanje Pjesme")]
                public uint trajanjeSekunde { get; set; }

                [Display(Name = "Javna Objava")]
                public bool javno { get; set; }

                [Display(Name = "Odobrena za Javnu Objavu")]
                public bool odobreno { get; set; }

                [Display(Name = "Pjesma Audio")]
                public string? putanjaAudio { get; set; }

                [Display(Name = "Pjesma Slika")]
                public string? putanjaSlika { get; set; }

                [Display(Name = "Pjesma GIF")]
                public string? putanjaGif { get; set; }

                public UInt64 brojReprodukcija { get; set; }

                public UInt64 brojLajkova { get; set; }

                [Required(ErrorMessage = "Ako smo se složili, onda ti ja složim šamar.")]
                public bool slazemSe { get; set; }

                public string? jezikPjesme { get; set; }

                [Display(Name = "Licenca")]
                public string? licenca { get; set; }

                [Display(Name = "Eksplicitni Sadržaj")]
                public bool eksplicitniSadrzaj { get; set; }

                public string? tekst { get; set; }

                public DateTime kreiranDatumVrijeme { get; set; }

                public ICollection<PjesmaPlayLista>? PjesmaPlayLista { get; set; }

                public ICollection<KorisnikPjesma>? KorisnikPjesma { get; set; }

                public ICollection<Komentar>? Komentar { get; set; }

                public ICollection<PjesmaZanr>? PjesmaZanr { get; set; }

                public Album? Album { get; set; }

                public Korisnik? Korisnik { get; set; }

                public ICollection<IzvodjacPjesma>? IzvodjacPjesma { get; set; }

                public ICollection<HistorijaSlusanja>? HistorijaSlusanja { get; set; }

                public ICollection<StatistikaReprodukcije>? StatistikaReprodukcije { get; set; }

                [NotMapped]
                [Display(Name = "Audio Datoteka")]
                [Required(ErrorMessage = "Audio Datoteka je obavezna.")]
                public IFormFile? AudioFile { get; set; }

                [NotMapped]
                [Display(Name = "Slika Datoteka")]
                public IFormFile? ImageFile { get; set; }

                [NotMapped]
                [Display(Name = "GIF Datoteka")]
                public IFormFile? GifFile { get; set; }

                [NotMapped]
                public List<string> IzvodjaciIDs { get; set; } = new List<string>();

                [NotMapped]
                public List<int> ZanrIDs { get; set; }


                public Pjesma() { }
        }
}
