using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ImageStoreBase.Api.Filters
{
    public class FluentValidationEntityFilter<T, TValidator> : ActionFilterAttribute where TValidator : AbstractValidator<T>, new()
    {
        private readonly string _entityParamName;

        public FluentValidationEntityFilter(string entityParamName)
        {
            _entityParamName = entityParamName;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue(_entityParamName, out var entityObj) && entityObj is T entity)
            {
                var validator = new TValidator();
                var validationResult = await validator.ValidateAsync(entity);

                if (!validationResult.IsValid)
                {
                    context.Result = new BadRequestObjectResult(validationResult.Errors);
                    return;
                }
            }
            await next();
        }
    }
}
