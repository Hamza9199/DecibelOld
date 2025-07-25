using Decibel.ViewModels;

namespace Decibel.ViewModels
{
        public class AlbumCreateViewModel
        {
		        public string naziv { get; set; }
                public string opis { get; set; }
                public string korisnikID { get; set; }
                public bool odobreno { get; set; }
                public List<PjesmaViewModel> Pjesme { get; set; } = new List<PjesmaViewModel>();
        }
}
