using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Decibel.Models
{
        public class PlayLista
        {
                [Key]
                public Int64 ID { get; set; }

                [BindNever]
                [ForeignKey("Korisnik")]
                public string? korisnikID { get; set; }

                public string naziv { get; set; }

                public string? opis { get; set; }

                public DateTime kreiranDatumVrijeme { get; set; }

                public bool javno { get; set; }

                public UInt64 brojLajkova { get; set; }

                public string? putanjaSlika { get; set; }

                public string? putanjaGif { get; set; }


                [BindNever]
                public Korisnik? Korisnik { get; set; }

                public ICollection<PjesmaPlayLista>? PjesmaPlayLista { get; set; }

                public ICollection<HistorijaSlusanja>? HistorijaSlusanja { get; set; }

                [NotMapped]
                public IFormFile? ImageFile { get; set; }

                [NotMapped]
                public IFormFile? GifFile { get; set; }

                public PlayLista() { }
        }
}
