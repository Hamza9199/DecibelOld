using Decibel.Models;

namespace Decibel.Dto
{
        public class PjesmaDto
        {
                public Pjesma Pjesma { get; set; }
                public long? prev { get; set; }
                public long next { get; set; }
                public string contextURL { get; set; }
                public KontekstPustanja context { get; set; }
                public DateTime DodanDatumVrijeme { get; set; }
                public bool lajkovana { get; set; } = false;
                public string korisnikIme { get; set; }
                public long? RedoslijedPjesama { get; set; }
        }
}
