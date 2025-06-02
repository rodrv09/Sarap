namespace CAAP2.ARCHITECTURE.Exceptions;

public static class StringExtensions
{
    public static string AddASalt(this string value)
    {
        return value.EndsWith("==")
            ? value.Replace("==", "QAB001")
            : value;
    }
}
