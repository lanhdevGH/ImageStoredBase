using FluentValidation;
using ImageStoreBase.Api.DTOs.FunctionDTOs;

namespace ImageStoreBase.Api.FluentValidator
{
    public class FunctionCreateRequestDTOValidator : AbstractValidator<FunctionCreateRequestDTO>
    {
        public FunctionCreateRequestDTOValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(200)
                .WithMessage("Name must not exceed 200 characters");

            RuleFor(x => x.ParentId)
                .NotEqual(x => x.Id)
                .WithMessage("ParentId must be different from Id");

            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required");
        }
    }
}
