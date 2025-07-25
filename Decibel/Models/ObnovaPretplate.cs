using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace Decibel.Models
{
        public class ObnovaPretplate
        {
                [Key]
                public Int64 ID { get; set; }

                [ForeignKey("KorisnikPretplata")]
                public Int64 korisnikPretplataID { get; set; }

                public KorisnikPretplata KorisnikPretplata { get; set; }

                public DateTime datumObnove { get; set; }

                [Column(TypeName = "money")]
                public decimal iznosObnove { get; set; }

                public DateTime kreiranDatumVrijeme { get; set; }
        }
}
