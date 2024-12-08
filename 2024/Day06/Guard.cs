using System.Drawing;


namespace Day06;

public class Guard
{
    private int StepCounter = 0;
    private int uniqueCoordinatCounter = 0;
    private GuardDirection _guDir;
    private Point _GuardPosition;
    private int xOffset = 0;
    private int yOffset = 0;

    public Guard(Point GuardPosition, GuardDirection guDir = GuardDirection.Up)
    {
        _GuardPosition = GuardPosition;
        _guDir = guDir;
        SetOffset(guDir);
        AddCoordinate(uniqueCoordinates, _GuardPosition.X, _GuardPosition.Y);
    }

    public int GetSteps()=> StepCounter;
    public int GetUniqueCoordinat() => uniqueCoordinatCounter;
    public GuardDirection GetDirection() => _guDir;
    public (int X, int Y) GetPosition() => (_GuardPosition.X, _GuardPosition.Y);

    public void MoveOneStep()
    {
        _GuardPosition.X = _GuardPosition.X + xOffset;
        _GuardPosition.Y = _GuardPosition.Y + yOffset;
        AddCoordinate(uniqueCoordinates, _GuardPosition.X, _GuardPosition.Y);
    }

    public (int X, int Y) CheckPathKord() =>
        (_GuardPosition.X + xOffset, _GuardPosition.Y + yOffset);

    public void TurnRight()
    {
        //TODO: Enum Casting verstehen
        _guDir = (GuardDirection)(((int)_guDir + 1) % 4);
        SetOffset(_guDir);
    }
    private void AddCoordinate(HashSet<(int x, int y)> coordinates, int x, int y)
    {
        if (coordinates.Add((x, y)))
        {
            uniqueCoordinatCounter++;
            GlobalLog.Log($"Koordinate ({x}, {y}) hinzugefügt.");
        }
        else
        {
            GlobalLog.Log($"Koordinate ({x}, {y}) ist ein Duplikat und wurde nicht hinzugefügt.");
        }
    }

    //TODO: Was kann man damit alles machen? Stadard UseCases?
    HashSet<(int x, int y)> uniqueCoordinates = new HashSet<(int, int)>();

    public enum GuardDirection
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
    }

    private void SetOffset(GuardDirection direction)
    {
        var dir = direction switch
        {
            GuardDirection.Up => (0, -1),
            GuardDirection.Right => (1, 0),
            GuardDirection.Down => (0, 1),
            GuardDirection.Left => (-1, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), $"Invalid direction: {direction}")
        };

        xOffset = dir.Item1;
        yOffset = dir.Item2;
    }

}
