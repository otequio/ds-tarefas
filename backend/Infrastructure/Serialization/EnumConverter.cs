using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Serialization;

public class EnumConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var enumString = reader.GetString();
        if (Enum.TryParse(enumString, ignoreCase: true, out TEnum result))
            return result;

        throw new JsonException($"O valor '{enumString}' não é válido para o campo. " +
                               $"Valores válidos: {string.Join(", ", Enum.GetNames(typeof(TEnum)))}");
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}