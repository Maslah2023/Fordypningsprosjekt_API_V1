using FastFoodHouse_API.Models.Dtos;
using FluentValidation;

namespace FastFoodHouse_API.Validators
{
    public class RegisterRequestDtoValidators : AbstractValidator<RegisterRequestDTO>
    {

        public RegisterRequestDtoValidators()
        {
            RuleFor(x => x.Name)
           .NotEmpty().WithMessage("Name kan ikke være null.")
           .MinimumLength(8).WithMessage("Name må være mist 3 tegn.");

            RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber kan ikke være null.");

            RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address kan ikke være null");

            RuleFor(x => x.Email)
            .NotEmpty().
             WithMessage("Email må være med")
            .EmailAddress().WithMessage("Det må være en gyldig e-mail");

            RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Passord kan ikke være null")
            .MinimumLength(8).WithMessage("Passord må være på minst 8 tegn")
            .MaximumLength(16).WithMessage("Passord kan ikke være lengre enn 16 tegn");
        }

    }
}
