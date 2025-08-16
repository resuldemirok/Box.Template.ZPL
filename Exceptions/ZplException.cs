using System;

namespace Box.Template.ZPL.Exceptions;

public class ZplException : Exception
{
    public ZplException(string message) : base(message) { }
}
