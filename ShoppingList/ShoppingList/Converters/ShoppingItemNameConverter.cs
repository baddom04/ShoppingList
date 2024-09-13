﻿using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace ShoppingList.Converters
{
    internal class ShoppingItemNameConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not string input) return value;

            return " - " + input;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
