namespace PersonalFinanceManagement.API.Extensions
{
    public static class ListExtenstions
    {
        public static List<T> ToListOrNullIfEmpty<T>(this IEnumerable<T> collection) =>
            collection.Any() ? collection.ToList() : null;
    }
}
