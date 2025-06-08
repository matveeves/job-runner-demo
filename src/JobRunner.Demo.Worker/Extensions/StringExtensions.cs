using Newtonsoft.Json.Linq;

namespace JobRunner.Demo.Worker.Extensions;

public static class StringExtensions
{
    public static bool IsValidJson(this string json)
    {
        try
        {
            JToken.Parse(json);
            return true;
        }
        catch
        {
            return false;
        }
    }
}