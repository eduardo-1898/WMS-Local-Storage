using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Enums
{
    public sealed class StringValueAttribute : Attribute
    {
        public string StringValue { get; private set; }

        public StringValueAttribute(string value)
        {
            StringValue = value;
        }
    }

    /// <summary>
    /// Helper class for working with 'extended' enums using <see cref="StringValueAttribute"/> attributes.
    /// The StringEnum class acts as a wrapper for string value access in enumerations. 
    /// It assumes that enums wishing to expose string values do so via the StringValue attribute
    /// </summary>

    public static class StringEnum
    {

        private static ConcurrentDictionary<string, string> _stringValues = new ConcurrentDictionary<string, string>();

        public static string GetStringValue(string key)
        {
            var value = string.Empty;
            return _stringValues.TryGetValue(key, out value) ? value : string.Empty;

        }
        /// <summary>
        /// Stores into memory a string value already calculated
        /// </summary>
        /// <param name="key">the key of the string value to store</param>
        /// <param name="value">the value of the string value to store</param>
        /// <returns>true or false</returns>
        public static bool SetStringValue(string key, string value)
        {
            return _stringValues.TryAdd(key, value);
        }

        public static string GetStringValue(this Enum value)
        {
            var stringValue = value.ToString();
            var type = value.GetType();
            var key = type.Name + "." + stringValue;

            // look for it in memory first
            var foundValue = GetStringValue(key);
            if (foundValue != string.Empty)
            {
                return foundValue;
            }

            // Get fieldinfo for this type
            var fieldInfo = type.GetField(stringValue);

            // Get the stringvalue attributes
            var attribs = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            if (attribs != null && attribs.Length > 0)
            {
                foundValue = attribs.First().StringValue;

                // store it in memory
                SetStringValue(key, foundValue);

                return foundValue;
            }

            return null;
        }
    }

}
