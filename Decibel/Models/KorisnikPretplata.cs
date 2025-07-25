using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace Decibel.Models
{
        [Index(nameof(korisnikID), IsUnique = true)]
        public class KorisnikPretplata
        {
                [Key]
                public Int64 ID { get; set; }

                [ForeignKey("Korisnik")]
                public string korisnikID { get; set; }

                [ForeignKey("Pretplata")]
                public Int64 pretplataID { get; set; }

                public Korisnik Korisnik { get; set; }

                public Pretplata Pretplata { get; set; }

                [EnumDataType(typeof(PretplataStatusEnum))]
                public PretplataStatusEnum PretplataStatus { get; set; }

                public DateTime kreiranDatumVrijeme { get; set; }

                public DateTime datumVrijemeObnove { get; set; }

                public DateTime datumIsteka { get; set; }

                public KorisnikPretplata() { }

        }
}
