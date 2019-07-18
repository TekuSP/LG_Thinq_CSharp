using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace LGThingApi
{
    public static class Extensions
    {
        public class SingleOrArrayConverter<T> : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return (objectType == typeof(T[]));
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JToken token = JToken.Load(reader);
                if (token.Type == JTokenType.Array)
                {
                    return token.ToObject<List<T>>();
                }
                return new List<T> { token.ToObject<T>() }.ToArray();
            }

            public override bool CanWrite
            {
                get { return false; }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
        public class ParseStringConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                long l;
                if (Int64.TryParse(value, out l))
                {
                    return l;
                }
                throw new Exception("Cannot unmarshal type long");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (long)untypedValue;
                serializer.Serialize(writer, value.ToString());
                return;
            }

            public static readonly ParseStringConverter Singleton = new ParseStringConverter();
        }
        /// <summary>The Range class.</summary>
        /// <typeparam name="T">Generic parameter.</typeparam>
        public struct Range<T> where T : IComparable<T>
        {
            /// <summary>Minimum value of the range.</summary>
            public T Minimum { get; set; }

            /// <summary>Maximum value of the range.</summary>
            public T Maximum { get; set; }

            /// <summary>Presents the Range in readable format.</summary>
            /// <returns>String representation of the Range</returns>
            public override string ToString()
            {
                return string.Format("[{0} - {1}]", this.Minimum, this.Maximum);
            }

            /// <summary>Determines if the range is valid.</summary>
            /// <returns>True if range is valid, else false</returns>
            public bool IsValid()
            {
                return this.Minimum.CompareTo(this.Maximum) <= 0;
            }

            /// <summary>Determines if the provided value is inside the range.</summary>
            /// <param name="value">The value to test</param>
            /// <returns>True if the value is inside Range, else false</returns>
            public bool ContainsValue(T value)
            {
                return (this.Minimum.CompareTo(value) <= 0) && (value.CompareTo(this.Maximum) <= 0);
            }

            /// <summary>Determines if this Range is inside the bounds of another range.</summary>
            /// <param name="Range">The parent range to test on</param>
            /// <returns>True if range is inclusive, else false</returns>
            public bool IsInsideRange(Range<T> range)
            {
                return this.IsValid() && range.IsValid() && range.ContainsValue(this.Minimum) && range.ContainsValue(this.Maximum);
            }

            /// <summary>Determines if another range is inside the bounds of this range.</summary>
            /// <param name="Range">The child range to test</param>
            /// <returns>True if range is inside, else false</returns>
            public bool ContainsRange(Range<T> range)
            {
                return this.IsValid() && range.IsValid() && this.ContainsValue(range.Minimum) && this.ContainsValue(range.Maximum);
            }
        }
    }
}
