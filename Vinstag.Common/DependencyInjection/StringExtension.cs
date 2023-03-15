namespace Vinstag.Common.DependencyInjection;

public static class StringExtension
{
    public static bool ContainsInvariant(this string firstValue, string secondValue)
    {
        return firstValue.ToUpperInvariant().Contains(secondValue.Trim().ToUpperInvariant());
    }
}