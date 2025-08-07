using System.Diagnostics;
using System.Text;
using DateTime = System.DateTime;

namespace Semanix.Application.Utilities;

public static class Common
{
    public static string GenerateReference(string prefix)
    {
        return $"{prefix}{DateTime.Now:yyyyMMddhhmmssffffff}";
    }

    public static int GenerateEventId(Exception? ex = null)
    {
        var trace = ex == null ? new StackTrace() : new StackTrace(ex);
        var builder = new StringBuilder();
        builder.Append(Environment.StackTrace);
        foreach (var frame in trace.GetFrames())
        {
            builder.Append(frame.GetILOffset());
            builder.Append(',');
        }

        return builder.ToString().GetHashCode() & 0xFFFF;
    }
}