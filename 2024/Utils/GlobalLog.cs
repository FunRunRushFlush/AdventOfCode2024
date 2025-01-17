// Is managed Globaly in the csproj config
//< PropertyGroup >
//    < DefineConstants > LOGGING_ENABLED </ DefineConstants >
//</ PropertyGroup >
public static class GlobalLog
{
    [System.Diagnostics.Conditional("LOGGING_ENABLED")]
    public static void LogLine(string message)
    {
        Console.WriteLine(message);
    }
    [System.Diagnostics.Conditional("LOGGING_ENABLED")]
    public static void Log(string message)
    {
        Console.Write(message);
    }
}
