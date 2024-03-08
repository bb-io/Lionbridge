using System.Text.Json;

namespace Apps.Lionbridge.Extensions;

public static class StringExtensions
{
    public static bool IsJson(this string input)
    {
        try
        {
            JsonDocument.Parse(input);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}