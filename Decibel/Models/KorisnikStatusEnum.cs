using System.ComponentModel.DataAnnotations;

namespace Decibel.Models
{
        public enum KorisnikStatusEnum
        {
                [Display(Name = "Aktivan")]
                Aktivan,
                [Display(Name = "Neaktivan")]
                Neaktivan,
                [Display(Name = "Suspendovan")]
                Suspendovan
        }
}
