using System;

namespace ARTS.Core.Extensions.EnumExtensions
{

    public static class EnumExtensions
    {
        /// <summary>
        /// <para>
        /// This can be used to get the string from a nullable enum.
        /// </para>
        /// <para>
        /// The defaultValue of any object can be passed to be returned 
        /// in case of a null enum. 
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static object ToString<T>(this T? self, object defaultValue = null) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");
            var returnValue = self.HasValue ? Enum.GetName(typeof(T), self).ToNullable<T>() : defaultValue;
            return returnValue != null ? returnValue.ToString() : defaultValue;
        }


        /// <summary>
        /// <para>
        /// This can be used to parse a nullable enum.
        /// </para>
        /// <para>
        /// An object can be passed as default value to be returned when the enum is null.
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static object ParseEnum<T>(this T? self, object defaultValue = null) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");
            var returnValue = self.HasValue ? Enum.GetName(typeof(T), self).ToNullable<T>() : defaultValue;
            return returnValue ?? defaultValue;
        }

        /// <summary>
        /// <para>
        /// This can be used to get the enum for an int.
        /// </para>
        /// <para>
        /// A default value can be passed to be returned when the int value does not have an enum (null).
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T? ParseEnum<T>(this int self, T? defaultValue = null) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");
            //var x = (T)(object)self;
            return Enum.GetName(typeof(T), self).ToNullable<T>() ?? defaultValue;
        }

        /// <summary>
        /// <para>
        /// This can be used to get the enum for an int.
        /// </para>
        /// <para>
        /// A default value can be passed to be returned when the int value does not have an enum (null).
        /// </para> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T? ParseEnum<T>(this T? self, T? defaultValue = null) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            return !self.HasValue ? defaultValue : Enum.GetName(typeof(T), self).ToNullable<T>() ?? defaultValue;
        }

        /// <summary>
        /// <para>
        /// This can be used to parse a nullable enum.
        /// </para>
        /// <para>
        /// A default enum value can be passed to be returned when the enum is null.
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int? ParseEnumToInt<T>(this T? self, T? defaultValue = null) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            return !self.HasValue ? (int?)(object)defaultValue : (int)(object)self;
        }

        /// <summary>
        /// <para>More convenient than using T.TryParse(string, out T). 
        /// Works with primitive types, structs, and enums.
        /// Tries to parse the string to an instance of the type specified.
        /// If the input cannot be parsed, null will be returned.
        /// </para>
        /// <para>
        /// If the value of the caller is null, null will be returned.
        /// So if you have "string s = null;" and then you try "s.ToNullable...",
        /// null will be returned. No null exception will be thrown. 
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns>A nullable T</returns>
        public static T? ToNullable<T>(this string self) where T : struct, IConvertible
        {
            if (!string.IsNullOrEmpty(self))
            {
                var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
                if (converter.IsValid(self)) return (T)converter.ConvertFromString(self);
                //if (typeof(T).IsEnum) { T t; if (Enum.TryParse<T>(p_self, out t)) return t;}
                if (typeof(T).IsEnum)
                {
                    T? t;
                    if (TryParse<T>(self, out t))
                        return t;
                }
            }

            return null;
        }

        /// <summary>
        /// <para>
        /// This can be used to for .Net Framework 3.5. where TryParse is not available for enum.
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse<T>(this string self, out T? result) where T : struct, IConvertible
        {
            try
            {
                result = (T)Enum.Parse(typeof(T), self);
            }
            catch
            {
                result = null;
                return false;
            }

            return true;
        }
    }
}
