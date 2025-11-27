using System.ComponentModel.DataAnnotations;
namespace CODE_QUESTS.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}