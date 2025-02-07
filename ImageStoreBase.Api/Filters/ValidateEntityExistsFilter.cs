using ImageStoreBase.Api.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImageStoreBase.Api.Filters
{
    public class ValidateEntityExistsFilter<T, K> : ActionFilterAttribute where T : class
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
            if (context.ActionArguments.TryGetValue(_nameParamRequest, out var requestObject))
            {
                var dbContext = context.HttpContext.RequestServices.GetService(typeof(AppDbContext)) as AppDbContext;
                if (dbContext == null)
                {
                    throw new InvalidOperationException("AppDbContext is not available.");
                }

                if (requestObject is K value)
                {
                    if (!await EntityExistsAsync(dbContext, _propertyToCheck, value))
                    {
                        context.Result = new NotFoundObjectResult($"Entity with {_propertyToCheck}={value} does not exist in the database.");
                        return;
                    }
                }
                else if (requestObject is IEnumerable<K> listValue)
                {
                    foreach (var item in listValue)
                    {
                        if (!await EntityExistsAsync(dbContext, _propertyToCheck, item))
                        {
                            context.Result = new NotFoundObjectResult($"Entity with {_propertyToCheck}={item} does not exist in the database.");
                            return;
                        }
                    }
                }
                else if (requestObject is not null)
                {
                    var idProperty = requestObject.GetType().GetProperty(_propertyToCheck);
                    if (idProperty != null)
                    {
                        var idValue = idProperty.GetValue(requestObject);
                        if (idValue is K validId && !await EntityExistsAsync(dbContext, _propertyToCheck, validId))
                        {
                            context.Result = new NotFoundObjectResult($"Entity with {_propertyToCheck}={validId} does not exist in the database.");
                            return;
                        }
                    }
                }
            }
            await next();
        }

        private async Task<bool> EntityExistsAsync(AppDbContext dbContext, string propertyName, K entityId)
        {
            return await dbContext.Set<T>().AnyAsync(e => EF.Property<object>(e, propertyName).Equals(entityId));
        }
    }

}
