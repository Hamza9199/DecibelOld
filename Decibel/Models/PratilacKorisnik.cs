using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Decibel.Models
{
        [PrimaryKey(nameof(korisnikID), nameof(pratilacID))]
        public class PratilacKorisnik
        {
                public string korisnikID { get; set; }

                public string pratilacID { get; set; }

                [DeleteBehavior(DeleteBehavior.Restrict)]
                public Korisnik? Korisnik { get; set; }

                [DeleteBehavior(DeleteBehavior.Restrict)]
                public Korisnik? Pratilac { get; set; }

                public DateTime kreiranDatumVrijeme { get; set; }


                public PratilacKorisnik() { }
        }
}
