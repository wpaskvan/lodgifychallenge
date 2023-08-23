using System;
using System.Collections.Generic;
using System.Text;

namespace SuperApp.Core.Exceptions
{
    public class NotFoundException : Exception
    {
        public int ResourceId { get; private set; }
        public NotFoundException(int resourceId, string message) : base(message)
        {
            ResourceId = resourceId;
        }
    }
}
