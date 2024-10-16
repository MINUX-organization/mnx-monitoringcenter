using System.Text.Json;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Core.Enums;

/// <summary>
/// Конвертирует enum-значения в и из Json списка строк.
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
        return (JsonConverter) Activator.CreateInstance(converterType)!;
    }
}

/// <summary>
/// Обработчик конвертации enum-значений в Json список строк.
/// </summary>
/// <typeparam name="T"> Enum. </typeparam>
file class EnumFlagsConversionHandler<T> : JsonConverter<T> where T : Enum
{
    /// <inheritdoc/>
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var intValue = reader.GetInt32();
        return (T) Enum.ToObject(typeof(T), intValue);
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
        var enumFlags = new List<string>();
        foreach (Enum flag in Enum.GetValues(typeof(T)))
        {
            if (enumValue.HasFlag(flag) && Convert.ToInt32(flag) != 0)
            {
                enumFlags.Add(flag.ToString());
            }
        }
        return enumFlags;
    }
}