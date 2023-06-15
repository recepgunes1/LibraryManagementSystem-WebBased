using System.ComponentModel.DataAnnotations;

namespace LMS.Entity.ViewModels.User;

public class ForgetPasswordViewModel
{
    [Required]
    public string EmailOrUsername { get; set; } = default!;
}