using ImageStoreBase.Api.DTOs.GenericDTO;
using ImageStoreBase.Api.MyExceptions;
using System.Net;

namespace ImageStoreBase.Api.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly ILoggerManager _logger;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            // Thiết lập mã trạng thái mặc định
            var statusCode = (int)HttpStatusCode.InternalServerError;
            string message = "Internal Server Error";
            // Xử lý các loại exception khác nhau
            switch (exception)
            {
                case ArgumentException ex:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = ex.Message;
                    break;
                case UnauthorizedAccessException ex:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    message = "You are not authorized to perform this action.";
                    break;
                case KeyNotFoundException ex:
                    statusCode = (int)HttpStatusCode.NotFound;
                    message = ex.Message;
                    break;
                case MyPasswordException ex:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = ex.Message;
                    break;
                case CreateUserException ex:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = ex.Message;
                    break;
                case ChangePasswordException ex:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = ex.Message;
                    break;
                default:
                    message = "Internal Server Error";
                    break;
            }
            Console.WriteLine(exception.Message);
            // Thiết lập mã trạng thái và trả về response
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(new ExceptionModel()
            {
                StatusCode = statusCode,
                Message = message
            }.ToString());
        }
    }
}
