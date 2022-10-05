using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Domain.Exceptions
{
    public class IdentityException : Exception
    {
        private readonly static string _BaseMessage = "Identity exception occured: ";

        public IdentityException(string message) : base(_BaseMessage + message) { }
    }
}
