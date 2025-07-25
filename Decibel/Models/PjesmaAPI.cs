using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Decibel.Models
{
        public class PjesmaAPI
        {
                public Int64 ID { get; set; }

                public Int64? albumID { get; set; }

                public Int64? redniBrojUAlbumu { get; set; }

                public string naziv { get; set; }

                public string? opis { get; set; }

                public DateOnly datumObjave { get; set; }

                public uint trajanjeSekunde { get; set; }

                public bool javno { get; set; }

                public bool odobreno { get; set; }

                public string? putanjaAudio { get; set; }

                public string? putanjaSlika { get; set; }

                public string? putanjaGif { get; set; }

                public UInt64 brojReprodukcija { get; set; }

                public UInt64 brojLajkova { get; set; }

                public string? jezikPjesme { get; set; }

                public string? licenca { get; set; }

                public bool eksplicitniSadrzaj { get; set; }

                public string? tekst { get; set; }

                public DateTime kreiranDatumVrijeme { get; set; }

                public PjesmaAPI() { }
        }
}
