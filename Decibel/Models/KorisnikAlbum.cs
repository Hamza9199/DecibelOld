using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Decibel.Models
{
        [PrimaryKey(nameof(korisnikID), nameof(albumID))]
        public class KorisnikAlbum
        {
                public string korisnikID { get; set; }

                public Int64 albumID { get; set; }

                public DateTime kreiranDatumVrijeme { get; set; }

                [DeleteBehavior(DeleteBehavior.Restrict)]
                public Korisnik Korisnik { get; set; }

                [DeleteBehavior(DeleteBehavior.Restrict)]
                public Album Album { get; set; }


                public KorisnikAlbum() { }


        }
}
