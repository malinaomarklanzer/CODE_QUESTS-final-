using System.ComponentModel.DataAnnotations;

namespace CODE_QUESTS.Models
{
    // This is the main "wrapper" model for your Manage.cshtml page
    // It holds an instance of the other two models.
    public class ManageViewModel
    {
        public ChangeNameViewModel ChangeName { get; set; } = new ChangeNameViewModel();
        public ChangePasswordViewModel ChangePassword { get; set; } = new ChangePasswordViewModel();
    }

    // This model is for the "Change Name" form
    public class ChangeNameViewModel
    {
        [Required]
        [Display(Name = "New Username")]
        public string NewUserName { get; set; } = string.Empty;
    }

    // This model is for the "Change Password" form
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}