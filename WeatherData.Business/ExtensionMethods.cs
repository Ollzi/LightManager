﻿using System;
using System.Collections.Generic;

namespace WeatherData.Business
{
    public static class ExtensionMethods
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }
    }
}
