using ImageStoreBase.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Reflection;

namespace ImageStoreBase.Api.Filters
{
    public class ValidateEntityExistsFilter<T> : ActionFilterAttribute where T : class
    {
        private readonly string _nameParamRequest;
        private readonly string _propertyToCheck;

        public ValidateEntityExistsFilter(string nameParamRequest, string propertyToCheck = "Id")
        {
            _nameParamRequest = nameParamRequest;
            _propertyToCheck = propertyToCheck;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionArguments.TryGetValue(_nameParamRequest, out var requestObject) || requestObject == null)
            {
                await next();
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
                        if (item != null && !await EntityExistsAsync(dbContext, _propertyToCheck, item))
                        {
                            context.Result = new NotFoundObjectResult($"Entity with {_propertyToCheck}={item} does not exist in the database.");
                            return;
                        }
                    }
                }
                // Xử lý nếu requestObject là một object chứa thuộc tính cần kiểm tra
                else if (requestObject.GetType().GetProperty(_propertyToCheck) is PropertyInfo property)
                {
                    var idValue = property.GetValue(requestObject);
                    if (idValue != null && !await EntityExistsAsync(dbContext, _propertyToCheck, idValue))
                    {
                        context.Result = new NotFoundObjectResult($"Entity with {_propertyToCheck}={idValue} does not exist in the database.");
                        return;
                    }
                }
                // Xử lý nếu requestObject là một giá trị đơn giản (int, string, GUID, ...)
                else if (!await EntityExistsAsync(dbContext, _propertyToCheck, requestObject))
                {
                    context.Result = new NotFoundObjectResult($"Entity with {_propertyToCheck}={requestObject} does not exist in the database.");
                    return;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error while validating entity existence: {ex.Message}");
            }

            await next();
        }

        private async Task<bool> EntityExistsAsync(AppDbContext dbContext, string propertyName, object entityId)
        {
            if (entityId == null) return false;

            var propertyType = typeof(T).GetProperty(propertyName)?.PropertyType;
            if (propertyType == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found in entity '{typeof(T).Name}'.");
            }

            // Chuyển đổi entityId sang kiểu dữ liệu phù hợp
            var convertedValue = Convert.ChangeType(entityId, propertyType);

            return await dbContext.Set<T>().AnyAsync(e => EF.Property<object>(e, propertyName).Equals(convertedValue));
        }
    }

}
