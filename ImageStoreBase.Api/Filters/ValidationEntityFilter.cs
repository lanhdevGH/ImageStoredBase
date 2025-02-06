using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ImageStoreBase.Api.Filters
{
    public class ValidationEntityFilter<T, TValidator> : ActionFilterAttribute
    where TValidator : AbstractValidator<T>, new()
    {
        private readonly string _nameEntity;
        public ValidationEntityFilter(string nameParamEntity)
        {
            _nameEntity = nameParamEntity;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Lấy thực thể cần validate từ action arguments
            if (context.ActionArguments.TryGetValue(_nameEntity, out var entityObj) && entityObj is T entity)
            {
                // Tạo validator và thực hiện validate
                var validator = new TValidator();
                var validationResult = await validator.ValidateAsync(entity);

                if (!validationResult.IsValid)
                {
                    // Nếu không hợp lệ, trả về lỗi
                    context.Result = new BadRequestObjectResult(validationResult.Errors);
                    return;
                }
            }

            // Tiếp tục thực thi nếu hợp lệ
            await next();
        }
    }
}
