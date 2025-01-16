namespace ImageStoreBase.Api.ViewModels
{
    public class PagedResult<T>
    {
        public List<T> Items { set; get; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }
    }
}
