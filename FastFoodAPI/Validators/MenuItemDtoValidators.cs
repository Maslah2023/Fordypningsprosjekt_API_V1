using FastFoodHouse_API.Models.Dtos;
using FluentValidation;

namespace FastFoodHouse_API.Validators
{
    public class MenuItemDtoValidators : AbstractValidator<MenuItemDTO>
    {
        public MenuItemDtoValidators()
        {
            RuleFor(x => x.Name)
           .NotEmpty().WithMessage("Name kan ikke være null.")
           .MinimumLength(8).WithMessage("Name må være mist 3 tegn.");

            RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category kan ikke være null.")
            .MaximumLength(3).WithMessage("Category må være mist 3 tegn");

            RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Address kan ikke være null")
            .MaximumLength(3).WithMessage("Address må være mist 3 tegn");

            RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description kan ikke være null.")
           .MinimumLength(8).WithMessage("Description må være mist 8 tegn."); 

            RuleFor(x => x.Price)
           .NotEmpty().WithMessage("Price kan ikke være null.");
           
        }
    }
}
