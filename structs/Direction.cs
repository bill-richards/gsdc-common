using gsdc.common.enums;

namespace gsdc.common.structs;

public readonly struct Direction
{
    private readonly CompassPoints _compassPointsPoint;
    private readonly List<byte> _rotateRight = new() { 3, 0, 1, 6, 4, 2, 7, 8, 5 };
    private readonly List<byte> _rotateLeft = new() { 1, 2, 5, 0, 4, 8, 3, 6, 7 };

    private static readonly Random RandomGenerator = new(Environment.TickCount);
    public static Direction Create(CompassPoints compassPointsPoint) 
        => new(compassPointsPoint);
    public static Direction Random() => new((CompassPoints)RandomGenerator.Next((int)CompassPoints.NorthEast + 1));

    private Direction(CompassPoints c = CompassPoints.Center) => _compassPointsPoint = c;

    public int ToInt32() => (int)_compassPointsPoint;

    private Direction Rotate(int numberOfStops, RotationAspects rotationAspect = RotationAspects.Clockwise)
    {
        var directionList = (rotationAspect == RotationAspects.Clockwise) ? _rotateRight : _rotateLeft;
        var origin = ToInt32();
        
        while (numberOfStops-- > 0)
        {
            origin = directionList[origin];
        }

        return new Direction((CompassPoints)origin);
    }

    public Direction Rotate90DegreesClockwise() => Rotate(2);
    public Direction Rotate90DegreesCounterClockwise() => Rotate(2, RotationAspects.CounterClockwise);
    public Direction Rotate180Degrees() => Rotate(4);
    public Polar AsNormalizedPolar() => new(1, _compassPointsPoint);
    
    public Coordinates AsNormalizedCoordinates()
    {
        var d = ToInt32();
        return new Coordinates(d % 3 - 1, d / 3 - 1);
    }

    /**
     * operator overrides
     *********************************************************************************************/

    private bool Equals(Direction other) => _compassPointsPoint == other._compassPointsPoint;

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return obj.GetType() == this.GetType() && Equals((Direction)obj);
    }

    public override int GetHashCode() => ToInt32();

    public static bool operator ==(Direction d, CompassPoints c) => d._compassPointsPoint == c;
    public static bool operator !=(Direction d, CompassPoints c) => !(d == c);

    public static bool operator ==(Direction lhs, Direction rhs) => lhs._compassPointsPoint == rhs._compassPointsPoint;
    public static bool operator !=(Direction lhs, Direction rhs) => !(lhs == rhs);
}