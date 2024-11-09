using System;

namespace SmartMatrix.Core.ErrorHandling
{
    public static class ThrowExtensions
    {
        public static void IfNull<T>(this IThrow obj, T value, string propertyName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(propertyName);
            }
        }

        public static void IfNull<T>(this IThrow obj, T value, string propertyName, string message)
        {
            if (value == null)
            {
                throw new ArgumentException($"{propertyName} is NULL. {message}");
            }
        }

        public static void IfNotNull<T>(this IThrow obj, T value, string message)
        {
            if (value != null)
            {
                throw new ArgumentException(message);
            }
        }

        public static void IfNullOrWhiteSpace(this IThrow obj, string value, string propertyName)
        {
            Throw.Exception.IfNull(value, propertyName);
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"{propertyName} cannot be empty.");
            }
        }
        
        public static void IfNotEqual<T>(this IThrow obj, int valueOne, int valueTwo, string propertyName)
        {
            if (valueOne != valueTwo)
            {
                throw new ArgumentException($"{propertyName} values are not equal.");
            }
        }

        public static void IfFalse(this IThrow obj, bool value, string message)
        {
            if (value == false)
            {
                throw new ArgumentException(message);
            }
        }

        public static void IfTrue(this IThrow obj, bool value, string message)
        {
            if (value == true)
            {
                throw new ArgumentException(message);
            }
        }

        public static void IfZero(this IThrow obj, int value, string propertyName)
        {
            if (value == 0)
            {
                throw new ArgumentException($"{propertyName} cannot be zero");
            }
        }
    }
}