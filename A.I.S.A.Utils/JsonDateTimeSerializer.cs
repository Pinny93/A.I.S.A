using System;
using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace A.I.S.A.Utils
{
    /// <summary>
    /// Base Custom Format DateTime Handler <br/>
    /// using System.Text.Json.Serialization;
    /// </summary>
    [SuppressMessage("ReSharper", "RedundantBaseQualifier")]
    public class MsBaseDateTimeConverter<T> : JsonConverter<T>
    {
        private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

        private readonly string _format;
        private readonly CultureInfo _culture;
        private readonly DateTimeStyles _dateTimeStyles;

        public MsBaseDateTimeConverter(string format, CultureInfo culture = null, DateTimeStyles dateTimeStyles = DateTimeStyles.RoundtripKind)
        {
            _format = format;

            if (culture == null)
            {
                _culture = CultureInfo.CurrentCulture;
            }

            _dateTimeStyles = dateTimeStyles;

        }

        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert == typeof(DateTime) || typeToConvert == typeof(DateTime?))
            {
                return true;
            }

            return false;
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            bool nullable = ReflectionUtils.IsNullableType(typeToConvert);
            if (reader.TokenType == JsonTokenType.Null)
            {
                if (!nullable)
                {
                    throw new JsonException();
                }

                return default;
            }

            if (_format != null)
            {
                if (DateTime.TryParseExact(reader.GetString(), _format, _culture, _dateTimeStyles,
                    out var dtValue))
                {
                    return (T)(object)dtValue;
                }

                throw new JsonException();
            }
            else
            {
                // try to parse number directly from bytes
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out DateTime dtValue, out int bytesConsumed) &&
                    span.Length == bytesConsumed)
                    return (T)(object)dtValue;

                // try to parse from a string if the above failed, this covers cases with other escaped/UTF characters
                if (DateTime.TryParse(reader.GetString(), out dtValue))
                    return (T)(object)dtValue;

                return (T)(object)reader.GetDateTime();
            }
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            if (value != null)
            {
                if (value is DateTime dateTime)
                {
                    if ((_dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal
                        || (_dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
                    {
                        dateTime = dateTime.ToUniversalTime();
                    }

                    var text = dateTime.ToString(_format ?? DefaultDateTimeFormat, _culture);
                    writer.WriteStringValue(text);
                }
                else
                {
                    throw new JsonException();
                }
                return;
            }

            writer.WriteNullValue();

        }
    }
    
    /// <summary>
    /// Format: dd.MM.yyyy - NOT NULL <br/>
    /// Microsoft <br/>
    /// using System.Text.Json.Serialization;
    /// </summary>
    [SuppressMessage("ReSharper", "RedundantBaseQualifier")]
    public class MsCustomDateConverter 
        : MsBaseDateTimeConverter<DateTime>
    {
        public MsCustomDateConverter() 
            : base("dd.MM.yyyy")
        {
            //base.DateTimeFormat = "yyyy-MM-dd";
        }
    }

    /// <summary>
    /// Format: dd.MM.yyyy - NOT NULL <br/>
    /// Microsoft <br/>
    /// using System.Text.Json.Serialization;
    /// </summary>
    [SuppressMessage("ReSharper", "RedundantBaseQualifier")]
    public class MsCustomDateTimeConverter 
        : MsBaseDateTimeConverter<DateTime>
    {
        public MsCustomDateTimeConverter() 
            : base("yyyy-MM-dd'T'HH:mm:ss")
        {
            //base.DateTimeFormat = "yyyy-MM-dd";
        }
    }

    /// <summary>
    /// Format: dd.MM.yyyy - NULLABLE <br/>
    /// Microsoft <br/>
    /// using System.Text.Json.Serialization;
    /// </summary>
    [SuppressMessage("ReSharper", "RedundantBaseQualifier")]
    public class MsCustomDateConverterNullable 
        : MsBaseDateTimeConverter<DateTime?>
    {
        public MsCustomDateConverterNullable() 
            : base("dd.MM.yyyy")
        {
            //base.DateTimeFormat = "yyyy-MM-dd";
        }
    }
    
}


