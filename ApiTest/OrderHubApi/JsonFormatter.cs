using System;
using System.Text.Json;

public class JsonFormatter
{
    public static string FlattenJson(string json)
    {
        var jsonDoc = JsonDocument.Parse(json);
        return JsonSerializer.Serialize(jsonDoc.RootElement);
    }
}