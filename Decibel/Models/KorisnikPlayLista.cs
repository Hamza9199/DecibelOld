using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Decibel.Models
{
        [PrimaryKey(nameof(korisnikID), nameof(playlistaID))]
        public class KorisnikPlayLista
        {
                public string korisnikID { get; set; }

                public Int64 playlistaID { get; set; }

                public DateTime kreiranDatumVrijeme { get; set; }

                [DeleteBehavior(DeleteBehavior.Restrict)]
                public Korisnik Korisnik { get; set; }

                [DeleteBehavior(DeleteBehavior.Restrict)]
                public PlayLista PlayLista { get; set; }


                public KorisnikPlayLista() { }

        }
}
