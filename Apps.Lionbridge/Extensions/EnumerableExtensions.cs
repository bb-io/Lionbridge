namespace Apps.Lionbridge.Extensions;

public static class EnumerableExtensions
{
    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(IEnumerable<TKey>? first, 
        IEnumerable<TValue>? second) where TKey : notnull
    {
        if (first == null || second == null)
            return new Dictionary<TKey, TValue>();
        
        var firstArray = first.ToArray();
        var secondArray = second.ToArray();
        
        return firstArray
            .Take(Math.Min(firstArray.Length, secondArray.Length))
            .Zip(secondArray, (firstValue, secondValue) => new KeyValuePair<TKey, TValue>(firstValue, secondValue))
            .ToDictionary();
    }
}