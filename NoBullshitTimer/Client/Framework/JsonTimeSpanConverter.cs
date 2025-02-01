using System.Text.Json;
using System.Text.Json.Serialization;

namespace NoBullshitTimer.Client.Framework;

public class JsonTimeSpanConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        if (string.IsNullOrEmpty(value))
            return TimeSpan.Zero;

        return TimeFormat.ParseTime(value, 0);
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(TimeFormat.UnparseTime(value));
    }
}