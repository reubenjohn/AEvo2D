using System.Diagnostics;
using UnityEditor;

[InitializeOnLoad]
public class Startup
{
    static Startup()
    {
        Trace.Listeners.Add(new ExceptionTraceListener());
    }
}