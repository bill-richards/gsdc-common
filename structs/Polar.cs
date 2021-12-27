using gsdc.common.enums;

namespace gsdc.common.structs;

public readonly struct Polar
{
    public Polar(int magnitude, CompassPoints direction)
    {
        Magnitude = magnitude;
        Direction = Direction.Create(direction);
    }
    public Polar(int magnitude, Direction direction)
    {
        Magnitude = magnitude;
        Direction = direction;
    }

    private int Magnitude { get; }
    private Direction Direction { get; }

    private const double S = (3.14159265359 * 2) / 8; // radians per slice
    private readonly double[] _compassToRadians = { 5*S, 6*S, 7*S, 4*S, 0, 0*S, 3*S, 2*S, 1*S };

    public Coordinates AsCoordinates()
    {
        if (Direction == CompassPoints.Center) {
            return new Coordinates(0, 0);
        }

        var x = (int)Math.Round(Magnitude * Math.Cos(_compassToRadians[Direction.ToInt32()]));
        var y = (int)Math.Round(Magnitude * Math.Sin(_compassToRadians[Direction.ToInt32()]));
        return new Coordinates(x, y);
    }
}