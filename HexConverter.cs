namespace gsdc.common;

/// <summary>
/// These extension methods
/// </summary>
public static class HexConverter
{
    public static IEnumerable<int> HexStringToIntegers(this string hexString)
        => hexString.Split(' ').Select(HexStringToInteger);

    public static int HexStringToInteger(this string hexString)
        => int.Parse(hexString, System.Globalization.NumberStyles.HexNumber);

    public static IEnumerable<int> HexStringsToIntegers(this IEnumerable<string> hexStrings) 
        => hexStrings.SelectMany(HexStringToIntegers);

    public static IEnumerable<string> IntegersToHexStrings(this IEnumerable<int> integers)
        => integers.Select(IntegerToHexString);
    public static string IntegersToHexString(this IEnumerable<int> integers)
        => string.Join(" ", integers.Select(IntegerToHexString));

    public static string IntegerToHexString(this int integer)
        => integer.ToString("X4");
}