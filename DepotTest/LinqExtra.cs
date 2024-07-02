namespace Depot;

public static class LinqExtra
{
    public static string Glue<T>(this IEnumerable<T> items, string separator)
    {
        return string.Join(separator, items);
    }
}