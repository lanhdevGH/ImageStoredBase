using ImageStoreBase.Api.UnitTest.Helpers;

namespace ImageStoreBase.Api.UnitTest.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> AsAsyncQueryable<T>(this IEnumerable<T> input)
        {
            return new NotInDbSet<T>(input);
        }
    }
}
