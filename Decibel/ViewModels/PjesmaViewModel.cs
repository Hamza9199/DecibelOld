using Decibel.Models;

namespace Decibel.ViewModels
{
        public class PjesmaViewModel
        {
                public string? albumIme { get; set; }

                public long? prev { get; set; }

                public long next { get; set; }

                public string contextURL { get; set; }

                public KontekstPustanja context { get; set; }

                public long? albumID { get; set; }

                public long ID { get; set; }

                public string naziv { get; set; }

                public string korisnikID { get; set; }

                public string korisnikIme { get; set; }

                public string putanjaSlika { get; set; }

                public string putanjaGIF { get; set; }

                public string putanjaAudio { get; set; }

                public bool javno { get; set; }

                public bool odobreno { get; set; }

                public DateTime datumDodano { get; set; }

                public string trajanje { get; set; }

                public bool eksplicitniSadrzaj { get; set; }

                public bool lajkovana { get; set; } = false;

                public List<IzvodjacViewModel>? Izvodjaci { get; set; }

                public List<Komentar>? Komentari { get; set; } = new List<Komentar>();
        }
}
