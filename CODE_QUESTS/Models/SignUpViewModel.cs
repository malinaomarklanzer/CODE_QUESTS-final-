using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; // Add this line

namespace CODE_QUESTS.Models
{
    public class SignUpViewModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string School { get; set; } = string.Empty;

        // --- ADD THIS LINE FOR THE FILE UPLOAD ---
        [Display(Name = "Avatar")]
        public IFormFile? AvatarImage { get; set; }
    }
}