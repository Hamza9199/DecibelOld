using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Decibel.Models
{
        [PrimaryKey(nameof(pjesmaID), nameof(playlistaID))]
        public class PjesmaPlayLista
        {
                public Int64 pjesmaID { get; set; }

                public Int64 playlistaID { get; set; }

                public Int64? pokazivacNaSljedecuPjesmuID { get; set; }

                public Int64? pokazivacNaPrethodnuPjesmuID { get; set; }

                public DateTime kreiranDatumVrijeme { get; set; }

                public Pjesma Pjesma { get; set; }

                public PlayLista PlayLista { get; set; }

                public Pjesma sljedecaPjesma { get; set; }

                public Pjesma prethodnaPjesma { get; set; }


                public PjesmaPlayLista() { }
        }
}
