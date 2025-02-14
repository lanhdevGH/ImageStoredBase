using FluentValidation;
using ImageStoreBase.Api.DTOs.CollectionDTOs;
using ImageStoreBase.Api.DTOs.CommandDTO;

namespace ImageStoreBase.Api.FluentValidator
{
    public class CollectionCreateRequestDTOValidator : AbstractValidator<CollectionCreateRequestDTO>
    {
        public CollectionCreateRequestDTOValidator()
        {

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required");
            RuleFor(x => x.Name)
                .MaximumLength(200)
                .WithMessage("Name must not exceed 200 characters");
            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Name must not exceed 500 characters");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required");
        }
    }
}
