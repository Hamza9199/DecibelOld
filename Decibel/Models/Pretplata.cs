using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace Decibel.Models
{
        public class Pretplata
        {
                [Key]
                public Int64 ID { get; set; }

                public string naziv { get; set; }

                public string opis { get; set; }

                [Column(TypeName = "money")]
                public decimal cijena { get; set; }

                public bool dostupno { get; set; }

                public DateTime kreiranDatumVrijeme { get; set; }

                public Pretplata() { }
        }
}
