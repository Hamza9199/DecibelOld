using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Decibel.Models
{
        public class HistorijaSlusanja
        {
                [Key]
                public Int64 ID { get; set; }

                [ForeignKey("Korisnik")]
                public string korisnikID { get; set; }

                [ForeignKey("Pjesma")]
                public Int64 pjesmaID { get; set; }

                [ForeignKey("PlayLista")]
                public Int64? playlistaID { get; set; }

                public DateTime kreiranDatumVrijeme { get; set; }

                public int trajanje { get; set; }

                public string kontekstPustanjaURL { get; set; }

                public bool offline { get; set; }

                public KontekstPustanja kontekstPustanja { get; set; }


                public HistorijaSlusanja() { }
        }
}
