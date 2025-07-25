namespace Decibel.ViewModels
{
        public class PretragaViewModel
        {
                public List<PjesmaViewModel> pjesme { get; set; }
                public List<IzvodjacViewModel> korisnici { get; set; }
                public List<PlayListaViewModel> playliste { get; set; }
                public List<PlayListaViewModel> albumi { get; set; }
        }
}
