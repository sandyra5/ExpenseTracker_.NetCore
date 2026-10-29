using System.ComponentModel.DataAnnotations;

namespace CoreExpenseTracker.Models
{
    public class RegisterModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "*")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "*")]
        public string? Photo { get; set; }
        public IFormFile? PhotoFile { get; set; }

        [Required(ErrorMessage = "*")]
        public string? Profession { get; set; }

        [Required(ErrorMessage = "*")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "*")]
        public string? Password { get; set; }

        public DateTime CreatedDt { get; set; }
    }
    public class LoginModel
    {
        [Required(ErrorMessage = "*")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "*")]
        public string? Password { get; set; }
    }
    public class UserProfile
    {
        public int UserId { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Photo { get; set; }

        public string? Profession { get; set; }

        public string? Username { get; set; }
    }
}
