namespace Decibel.ViewModels
{
        public class PlayListaViewModel
        {
                public long ID { get; set; }
                public string Title { get; set; }
                public List<IzvodjacViewModel>? Izvodjaci { get; set; }
                public int TrackCount { get; set; }
                public string? ImageUrl { get; set; }

        }
}
