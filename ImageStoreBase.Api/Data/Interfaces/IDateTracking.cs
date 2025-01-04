namespace ImageStoreBase.Api.Data.Interfaces
{
    public interface IDateTracking
    {
        DateTime CreatedAt { get; set; }

        DateTime UpdatedAt { get; set; }
    }
}
