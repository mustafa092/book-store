using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace persistence.CustomValueConvertors;

public class JsonValueConverter<T> : ValueConverter<T, string> where T : class, new()
{
    public JsonValueConverter() : base(
        v => JsonSerializer.Serialize(v,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
        v => JsonSerializer.Deserialize<T>(v, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }))
    {
    }
}