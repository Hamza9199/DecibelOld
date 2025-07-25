using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Decibel.Models
{
        [PrimaryKey(nameof(korisnikID), nameof(pjesmaID))]
        public class KorisnikPjesma
        {
                public string korisnikID { get; set; }

                public Int64 pjesmaID { get; set; }

                public DateTime kreiranDatumVrijeme { get; set; }

                public Korisnik? Korisnik { get; set; }

                public Pjesma? Pjesma { get; set; }


                public KorisnikPjesma() { }
        }
}
