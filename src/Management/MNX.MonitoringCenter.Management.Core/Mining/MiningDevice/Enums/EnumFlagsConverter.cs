using System.Text.Json;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

/// <summary>
/// Конвертирует enum-значения в Json список строк и наоборот.
/// </summary>
public class EnumFlagsConverter : JsonConverterFactory
{
    /// <inheritdoc/>
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum && Attribute.IsDefined(typeToConvert, typeof(FlagsAttribute));
    }

    /// <inheritdoc/>
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(EnumFlagsConversionHandler<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}

/// <summary>
/// Обработчик конвертации enum-значений в Json список строк и наоборот.
/// </summary>
/// <typeparam name="T"> Enum. </typeparam>
file class EnumFlagsConversionHandler<T> : JsonConverter<T> where T : Enum
{
    /// <inheritdoc/>
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException($"Expected a number token, but got {reader.TokenType}.");

        var intValue = reader.GetInt32();

        if (!Enum.IsDefined(typeof(T), intValue))
            throw new JsonException($"The value {intValue} is not valid for enum {typeof(T)}.");

        return (T)Enum.ToObject(typeof(T), intValue);
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var flagNames = GetFlagsAsList(value);
        writer.WriteStartArray();

        foreach (var flagName in flagNames)
        {
            writer.WriteStringValue(flagName);
        }

        writer.WriteEndArray();
    }

    private static List<string> GetFlagsAsList(T enumValue)
    {
        var enumFlags = new HashSet<string>();

        foreach (Enum flag in Enum.GetValues(typeof(T)))
        {
            if (flag.Equals(0))
                continue;

            if (enumValue.HasFlag(flag))
            {
                enumFlags.Add(flag.ToString());
            }
        }

        return enumFlags.ToList();
    }
}