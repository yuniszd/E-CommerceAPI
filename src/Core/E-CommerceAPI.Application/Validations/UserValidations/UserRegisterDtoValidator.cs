using E_CommerceAPI.Application.DTOs.Users;
using FluentValidation;

namespace E_CommerceAPI.Application.Validations.UserValidations;

public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
{
    public UserRegisterDtoValidator()
    {
        RuleFor(x => x.Fullname)
            .NotEmpty().WithMessage("Fullname is required.")
            .MinimumLength(3).WithMessage("Fullname must be at least 3 characters.")
            .MaximumLength(100).WithMessage("Fullname cannot exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
            .MaximumLength(100).WithMessage("Password cannot exceed 100 characters.");
    }
}
