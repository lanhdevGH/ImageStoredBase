using FluentValidation;
using ImageStoreBase.Api.DTOs.CommandDTO;

namespace ImageStoreBase.Api.FluentValidator
{
    public class CommandCreateRequestDTOValidator : AbstractValidator<CommandCreateRequestDTO>
    {
        public CommandCreateRequestDTOValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required");

            RuleFor(x => x.Name)
                .MaximumLength(50)
                .WithMessage("Name must not exceed 50 characters");
            
            RuleFor(x => x.Name)
                .MaximumLength(500)
                .WithMessage("Name must not exceed 500 characters");
        }
    }
}
