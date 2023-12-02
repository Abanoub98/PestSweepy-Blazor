using Bookify.Web.Core.Consts;

namespace Dashboard.Blazor.Models.Dtos;

public class ResetPasswordDto
{
    [Required]
    [StringLength(100, ErrorMessage = Errors.MaxMinLengthError, MinimumLength = 8)]
    [Display(Name = "Current Password")]
    public string CurrentPassword { get; set; } = null!;

    [Required]
    [StringLength(100, ErrorMessage = Errors.MaxMinLengthError, MinimumLength = 8)]
    [RegularExpression(RegexPatterns.Password_LowerUppercaseAndDigitsAndSpecialCharacter, ErrorMessage = Errors.PasswordLowerUppercaseAndDigitsAndSpecialCharacterError)]
    [Display(Name = "New Password")]
    public string NewPassword { get; set; } = null!;

    [Required]
    [Compare("NewPassword", ErrorMessage = Errors.PasswordMatchError)]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = null!;

    public string? Message { get; set; }
}
