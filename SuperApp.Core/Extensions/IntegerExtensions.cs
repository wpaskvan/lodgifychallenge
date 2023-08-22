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
    }
}
