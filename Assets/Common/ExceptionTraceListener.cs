using System;
using System.Diagnostics;

public class ExceptionTraceListener : TraceListener
{
    public override void Write(string msg)
    {
        throw new Exception(msg);
    }
    public override void WriteLine(string msg)
    {
        throw new Exception(msg);
    }
}