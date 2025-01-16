using System.Text.Json;

namespace ImageStoreBase.Api.ViewModels
{
    public class ExceptionModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
