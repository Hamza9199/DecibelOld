using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Decibel.Models
{
        public class StatistikaReprodukcije
        {
                [Key]
                public Int64 ID { get; set; }

                [ForeignKey("Korisnik")]
                public string korisnikID { get; set; }

                [ForeignKey("Pjesma")]
                public Int64 pjesmaID { get; set; }

                public int brojReprodukcija { get; set; }

                public int ukupnoTrajanje { get; set; }

                public DateTime zadnjePustanje { get; set; }

                public Pjesma Pjesma { get; set; }

                public StatistikaReprodukcije() { }
        }
}
