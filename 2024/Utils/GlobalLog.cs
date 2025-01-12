// Wird Global in der csproj gesteuert
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
