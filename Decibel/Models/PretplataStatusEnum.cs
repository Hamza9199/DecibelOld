using System.ComponentModel.DataAnnotations;

namespace Decibel.Models
{
        public enum PretplataStatusEnum
        {
                [Display(Name = "Aktivna")]
                Aktivna,
                [Display(Name = "Neaktivna")]
                Neaktivna,
                [Display(Name = "Pauzirana")]
                Pauzirana,
                [Display(Name = "Istekla")]
                Istekla
        }
}
