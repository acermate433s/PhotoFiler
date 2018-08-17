using System;
using System.ComponentModel;
using System.Globalization;

namespace PhotoFiler.Photo
{
    [TypeConverter(typeof(HashConverter))]
    public class Hash : IEquatable<Hash>
    {
        private readonly string value;

        private Hash(string value)
        {
            this.value = value;
        }

        public string Value => this.value;

        public static bool operator ==(Hash c1, Hash c2) => c1?.Equals(c2) ?? false;

        public static bool operator !=(Hash c1, Hash c2) => !c1?.Equals(c2) ?? true;

        public static explicit operator Hash(string hash) => Parse(hash);

        public static explicit operator string(Hash hash) => hash.Value;

        public static Hash Parse(string hash)
        {
            if (TryParse(hash, out var result))
            {
                return result;
            }
            else
            {
                throw new InvalidCastException($"'{hash}' cannot be converted to {nameof(Hash)}");
            }
        }

        public static bool TryParse(string hash, out Hash result)
        {
            if (!string.IsNullOrEmpty(hash))
            {
                result = new Hash(hash);
                return true;
            }
            else
            {
                result = default(Hash);
                return false;
            }
        }

        public bool Equals(Hash other)
        {
            return this.value.Equals(other?.Value, StringComparison.CurrentCulture);
        }

        public override bool Equals(object obj)
        {
            if (obj is Hash hashComparator)
            {
                return this.Equals(hashComparator);
            }
            else if (obj is string stringComparator)
            {
                if (!string.IsNullOrEmpty(stringComparator))
                {
                    return this.Equals(Hash.Parse(stringComparator));
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        public override string ToString()
        {
            return this.value;
        }

        private class HashConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string) ? true : base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string hash)
                {
                    return Hash.Parse(hash);
                }

                return base.ConvertFrom(context, culture, value);
            }
        }
    }
}
