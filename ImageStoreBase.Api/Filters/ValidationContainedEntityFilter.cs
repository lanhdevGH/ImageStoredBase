using ImageStoreBase.Api.Data;
using ImageStoreBase.Common.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace ImageStoreBase.Api.Filters
{
    public class ValidationContainedEntityFilter<D, E, K> : ActionFilterAttribute where D : class where E : class
    {
        private readonly string _nameParamRequest;

        public ValidationContainedEntityFilter(string nameParamRequest)
        {
            _nameParamRequest = nameParamRequest;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue(_nameParamRequest, out var dtoObj) && dtoObj is D dto)
            {
                if (await EntityExistsAsync(dto, context))
                {
                    context.Result = new BadRequestObjectResult($"Entity already exists in the database.");
                    return;
                }
            }

            await next();
        }

        private async Task<bool> EntityExistsAsync(D enityDTO, ActionExecutingContext context)
        {
            var _dbContext = context.HttpContext.RequestServices.GetService(typeof(AppDbContext)) as AppDbContext;
            if (_dbContext == null)
            {
                throw new InvalidOperationException("AppDbContext is not available.");
            }
            var idValue = ClassHelper.GetPropertyValue<K>(enityDTO, "Id");
            if (idValue == null) return false;
            return await _dbContext.Set<E>().AnyAsync(e => EF.Property<object>(e, "Id").Equals(idValue));
        }
    }

    //========================================================================================
    public class ValidationContainedEntityFilter<T, K> : ActionFilterAttribute where T : class
    {
        private readonly string _nameParamRequest;

        public ValidationContainedEntityFilter(string nameParamRequest)
        {
            _nameParamRequest = nameParamRequest;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue(_nameParamRequest, out var entityIdVal))
            {
                if (entityIdVal is K value)
                {
                    if (await EntityExistsAsync("Id", value, context))
                    {
                        context.Result = new BadRequestObjectResult($"Entity already exists in the database.");
                        return;
                    }
                }

                if (entityIdVal is IEnumerable<K> listvalue)
                {
                    foreach (var item in listvalue)
                    {
                        if (await EntityExistsAsync("CommandId", item, context))
                        {
                            context.Result = new BadRequestObjectResult($"{item} already exists in the database.");
                            return;
                        }
                    }
                }
            }
            await next();
        }

        public async Task<bool> EntityExistsAsync(string nameProCheck,K entityid, ActionExecutingContext context)
        {
            var _dbContext = context.HttpContext.RequestServices.GetService(typeof(AppDbContext)) as AppDbContext;
            if (_dbContext == null)
            {
                throw new InvalidOperationException("AppDbContext is not available.");
            }
            return await _dbContext.Set<T>().AnyAsync(e => EF.Property<object>(e, nameProCheck).Equals(entityid));
        }
    }
}
