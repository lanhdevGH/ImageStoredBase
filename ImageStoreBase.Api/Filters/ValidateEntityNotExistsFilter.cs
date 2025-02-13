using ImageStoreBase.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Reflection;

namespace ImageStoreBase.Api.Filters
{
    public class ValidateEntityNotExistsFilter<T> : ActionFilterAttribute where T : class
    {
        private readonly string _nameParamRequest;
        private readonly string _propertyToCheck;

        public ValidateEntityNotExistsFilter(string nameParamRequest, string propertyToCheck = "Id")
        {
            _nameParamRequest = nameParamRequest;
            _propertyToCheck = propertyToCheck;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionArguments.TryGetValue(_nameParamRequest, out var requestObject) || requestObject == null)
            {
                context.Result = new BadRequestObjectResult($"Missing or null value for parameter '{_nameParamRequest}'.");
                return;
            }

            var dbContext = context.HttpContext.RequestServices.GetService<AppDbContext>();
            if (dbContext == null)
            {
                throw new InvalidOperationException("AppDbContext is not available.");
            }

            try
            {
                // Xử lý nếu requestObject là một danh sách
                if (requestObject is IEnumerable collection && requestObject is not string)
                {
                    foreach (var item in collection)
                    {
                        if (item != null && !await EntityNotExistsAsync(dbContext, _propertyToCheck, item))
                        {
                            context.Result = new ConflictObjectResult(new { message = $"Entity with {_propertyToCheck} '{item}' already exists." });
                            return;
                        }
                    }
                }
                // Xử lý nếu requestObject là một object chứa thuộc tính cần kiểm tra
                else if (requestObject.GetType().GetProperty(_propertyToCheck) is PropertyInfo property)
                {
                    var idValue = property.GetValue(requestObject);
                    if (idValue != null && !await EntityNotExistsAsync(dbContext, _propertyToCheck, idValue))
                    {
                        context.Result = new ConflictObjectResult(new { message = $"Entity with {_propertyToCheck} '{idValue}' already exists." });
                        return;
                    }
                }
                // Xử lý nếu requestObject là một giá trị đơn giản (int, string, GUID, ...)
                else if (!await EntityNotExistsAsync(dbContext, _propertyToCheck, requestObject))
                {
                    context.Result = new ConflictObjectResult(new { message = $"Entity with {_propertyToCheck} '{requestObject}' already exists." });
                    return;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error while validating entity existence: {ex.Message}");
            }

            await next();
        }

        private async Task<bool> EntityNotExistsAsync(AppDbContext dbContext, string propertyName, object entityId)
        {
            if (entityId == null) return false;

            var propertyType = typeof(T).GetProperty(propertyName)?.PropertyType;
            if (propertyType == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found in entity '{typeof(T).Name}'.");
            }

            // Chuyển đổi entityId sang kiểu dữ liệu phù hợp
            var convertedValue = Convert.ChangeType(entityId, propertyType);

            var result = await dbContext.Set<T>().AnyAsync(e => EF.Property<object>(e, propertyName).Equals(convertedValue));
            return !result;
        }
    }
}
