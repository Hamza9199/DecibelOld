using System.ComponentModel.DataAnnotations;

namespace Decibel.Models
{
        public class Zanr
        {
                [Key]
                public Int64 ID { get; set; }

                public String naziv { get; set; }

                public String opis { get; set; }

                public DateTime kreiranDatumVrijeme { get; set; }

                public ICollection<PjesmaZanr>? PjesmaZanr { get; set; }

        }
}
