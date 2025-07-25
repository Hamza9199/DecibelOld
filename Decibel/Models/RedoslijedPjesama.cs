using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Decibel.Models
{
        
        public class RedoslijedPjesama
        {
                [Key]
                public Int64 redoslijedPjesama { get; set; }

                public Int64 pjesmaID { get; set; }

                public Int64 playlistaID { get; set; }

                public Int64 pokazivacNaSljedecuPjesmuID { get; set; }

                public Int64? pokazivacNaPrethodnuPjesmuID { get; set; }

                public DateTime kreiranDatumVrijeme { get; set; }
        }
}
