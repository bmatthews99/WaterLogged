﻿using System;
using System.Collections.Generic;

namespace WaterLogged.Serialization.StringConversion
{
    public static class Converter
    {
        public static List<IStringConverter> Converters { get; private set; }

        static Converter()
        {
            Converters = new List<IStringConverter>
            {
                new ArrayConverter(),
                new BoolConverter(),
                new DateTimeConverter(),
                new NumericConverters(),
                new StringConverter()
            };
        }

        public static object Convert(string input, Type target)
        {
            foreach (var stringConverter in Converters)
            {
                if (stringConverter.SupportsType(target))
                {
                    return stringConverter.Convert(input, target);
                }
            }
            throw new KeyNotFoundException("Failed to find a StringConverter for the specified target type.");
        }

        public static string Convert(object input)
        {
            foreach (var stringConverter in Converters)
            {
                if (stringConverter.SupportsType(input.GetType()))
                {
                    return stringConverter.Convert(input);
                }
            }
            throw new KeyNotFoundException("Failed to find a StringConverter for the specified type.");
        }
    }
}
