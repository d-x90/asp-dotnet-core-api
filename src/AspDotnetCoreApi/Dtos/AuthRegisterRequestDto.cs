using System.ComponentModel.DataAnnotations;

namespace AspDotnetCoreApi.Dtos {
    public class AuthRegisterRequestDto {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }
        
        [Required]
        public string PasswordConfirm { get; set; }
    }
}