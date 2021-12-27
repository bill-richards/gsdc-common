namespace gsdc.common;

public static class RandomNumberGenerator
{
    private static readonly Random RandomGenerator = new Random((int)DateTime.UtcNow.Ticks);

    public static int NextInt(int maximum) => RandomGenerator.Next(maximum + 1);

    public static int NextInt(int minimum, int maximum) => RandomGenerator.Next(minimum, maximum + 1);
}