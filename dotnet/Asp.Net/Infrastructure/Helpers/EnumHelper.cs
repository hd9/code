using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Web.Infrastructure.Helpers
{
    public static class EnumHelper
    {
        public static EnumType ConvertToEnum<EnumType>(this String enumValue)
        {
                return (EnumType) Enum.Parse(typeof (EnumType), enumValue,true);
        }

        public static string GetEnumDescription<T>(string value)
        {
            Type type = typeof(T);
            var name = Enum.GetNames(type).Where(f => f.Equals(value, StringComparison.CurrentCultureIgnoreCase)).Select(d => d).FirstOrDefault();

            if (name == null)
            {
                return string.Empty;
            }
            var field = type.GetField(name);
            var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return customAttribute.Length > 0 ? ((DescriptionAttribute)customAttribute[0]).Description : name;
        }

        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
            // or return default(T);
        }

        public static List<string> GetDescriptions<T>()
        {
            var descs = new List<string>();
            var type = typeof(T);
            foreach (var field in type.GetFields())
            {

                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    descs.Add(attribute.Description);
                }
                else
                {
                    descs.Add(field.Name);
                }
            }

            return descs;
        }

    }
}