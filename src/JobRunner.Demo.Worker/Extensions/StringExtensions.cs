using Newtonsoft.Json.Linq;

namespace JobRunner.Demo.Worker.Extensions;

public static class StringExtensions
{
    public static bool IsValidJson(this string jsonCustomParams)
    {
        try
        {
            JToken.Parse(jsonCustomParams);
            return true;
        }
        catch
        {
            return false;
        }
    }
}