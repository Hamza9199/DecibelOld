using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Decibel.Models
{
        public class PlayListaAPI
        {
                public Int64 ID { get; set; }

                public string? korisnikID { get; set; }

                public string naziv { get; set; }

                public string? opis { get; set; }

                public DateTime kreiranDatumVrijeme { get; set; }

                public bool javno { get; set; }

                public UInt64 brojLajkova { get; set; }

                public string? putanjaSlika { get; set; }

                public string? putanjaGif { get; set; }


                public PlayListaAPI() { }
        }
}
