using System.ComponentModel.DataAnnotations;
namespace CODE_QUESTS.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string Code { get; set; } = string.Empty;
    }
}