using System.Globalization;
using gsdc.common.enums;

namespace gsdc.common.structs;

public readonly struct Coordinates
{
    public Coordinates(int x, int y)
    {
        X = x;
        Y = y;
    }

    private readonly int X;
    private readonly int Y;

    public bool IsNormalized() => X is >= -1 and <= 1 && Y is >= -1 and <= 1;

    private int Length() => int.Parse($"{Math.Sqrt((double)X * X + Y * Y)}", NumberStyles.Any); // round down

    public Coordinates Normalize() => AsDirection().AsNormalizedCoordinates();

    public Polar AsPolar() => new(Length(), AsDirection());

    // Calculate the angle, round to the nearest 360/8 degree slice, then
    // convert the slice to a Dir8Compass value.
    private Direction AsDirection()
    {
        if (X == 0 && Y == 0) 
            return Direction.Create(CompassPoints.Center);
        

        const double twoPi = 3.1415927f * 2.0d;
        var angle = Math.Atan2((float)Y, (float)X);

        if (angle < 0.0f)
        {
            angle = 3.1415927f + (3.1415927f + angle);
        }

        angle += (twoPi / 16.0f);            // offset by half a slice
        if (angle > twoPi)
        {
            angle -= twoPi;
        }
        var slice = (uint)(angle / (twoPi / 8.0f));   // find which division it's in
/*
    We have to convert slice values:

        3  2  1
        4     0
        5  6  7

    into CompassPoints value:

        6  7  8
        3  4  5
        0  1  2
*/
        CompassPoints[] directionConversion =
        {
            CompassPoints.East, CompassPoints.NorthEast, CompassPoints.North, CompassPoints.NorthWest,
            CompassPoints.West, CompassPoints.SouthWest, CompassPoints.South, CompassPoints.SouthEast
        };

        return Direction.Create(directionConversion[slice]);
    }

    // returns -1.0 (opposite directions) .. +1.0 (same direction)
    // returns 1.0 if self is (0,0) or d is CENTER
    public double RaySameness(Direction d) => RaySameness(d.AsNormalizedCoordinates());

    // returns -1.0 (opposite directions) .. +1.0 (same direction)
    // returns 1.0 if either vector is (0,0)
    private double RaySameness(Coordinates other)
    {
        var mag1 = Math.Sqrt(X * X + Y * Y);
        var mag2 = Math.Sqrt(other.X * other.X + other.Y * other.Y);

        if (mag1 == 0.0 || mag2 == 0.0) {
            return 1.0; // anything is "same" as zero vector
        }
        var dot = X * other.X + Y * other.Y;
        var cos = dot / (mag1 * mag2);
        
        return Math.MinMagnitude(Math.MaxMagnitude(cos, -1.0), 1.0); // clip
    }

    /**
 * operator overrides
 *********************************************************************************************/
    private bool Equals(Coordinates other) => X == other.X && Y == other.Y;
    public override bool Equals(object? obj) => obj is Coordinates other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(X, Y);

    public static bool operator ==(Coordinates lhs, Coordinates rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y;
    public static bool operator !=(Coordinates lhs, Coordinates rhs) => !(lhs == rhs);

    public static Coordinates operator +(Coordinates lhs, Coordinates rhs) => new(lhs.X + rhs.X, lhs.Y + rhs.Y);
    public static Coordinates operator -(Coordinates lhs, Coordinates rhs) => new(lhs.X - rhs.X, lhs.Y - rhs.Y);

    public static Coordinates operator *(Coordinates lhs, int multiplier) => new(lhs.X * multiplier, lhs.Y * multiplier);
    public static Coordinates operator +(Coordinates lhs, Direction rhs) => lhs + rhs.AsNormalizedCoordinates();
    public static Coordinates operator -(Coordinates lhs, Direction rhs) => lhs - rhs.AsNormalizedCoordinates();

}