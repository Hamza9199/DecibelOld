using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Decibel.Models
{
        public class Album
        {
                [Key]
                public Int64 ID { get; set; }

                [ForeignKey("Korsnik")]
                public string? korisnikID { get; set; }

                [Display(Name = "Naziv")]
                [Required(ErrorMessage = "Naziv je obavezan.")]
                public string naziv { get; set; }

                [Display(Name = "Opis")]
                public string? opis { get; set; }

                public DateTime kreiranDatumVrijeme { get; set; }

                public bool odobreno { get; set; }

                public bool javno { get; set; }

                public UInt64 brojLajkova { get; set; }

                public string? putanjaSlika { get; set; }

                public string? putanjaGif { get; set; }

                public ICollection<Pjesma>? Pjesma { get; set; }

                public Korisnik? Korisnik { get; set; }

                [NotMapped]
                public List<int> PjesmeIDs { get; set; } = new List<int>();

                [NotMapped]
                [Display(Name = "Slika Datoteka")]
                public IFormFile? ImageFile { get; set; }

                [NotMapped]
                [Display(Name = "GIF Datoteka")]
                public IFormFile? GifFile { get; set; }


                public Album() { }

        }
}
