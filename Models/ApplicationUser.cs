using Microsoft.AspNetCore.Identity;

namespace CarRent.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string? CNP { get; set; } 
        public string Adresa { get; set; }
        public string? Telefon { get; set; } 


    }
}