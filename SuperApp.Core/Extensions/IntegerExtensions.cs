using System;
using System.Collections.Generic;
using System.Text;

namespace SuperApp.Core.Extensions
{
    public static class IntegerExtensions
    {
        public static void EnsureNonNegative(this ref int value)
        {
            if (value < 0)
            {
                value = value * -1;
            }
        }

        public static void MustBePossitive(this int value, string parameterName)
        {
            if(value < 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, "Value must be positive");
            }
        }
    }
}
