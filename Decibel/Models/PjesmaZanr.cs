using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Decibel.Models
{
        [PrimaryKey(nameof(pjesmaID), nameof(zanrID))]
        public class PjesmaZanr
        {
                public Int64 pjesmaID { get; set; }

                public Int64 zanrID { get; set; }

                public DateTime? kreiranDatumVrijeme { get; set; }

                public Pjesma? Pjesma { get; set; }

                public Zanr? Zanr { get; set; }
        }
}
