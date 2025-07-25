using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Decibel.Models
{
        [PrimaryKey(nameof(izvodjacID), nameof(pjesmaID))]
        public class IzvodjacPjesma
        {
                public string izvodjacID { get; set; }

                public Int64 pjesmaID { get; set; }

                public DateTime kreiranDatumVrijeme { get; set; }

                public bool potvrdjeno { get; set; }

                public Decimal udio { get; set; }

                public Korisnik? Izvodjac { get; set; }

                public Pjesma? Pjesma { get; set; }

                public IzvodjacPjesma() { }
        }
}
