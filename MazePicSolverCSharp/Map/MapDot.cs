using System.Drawing;

public class MapDot
{
    public void Reset()
    {
        PreviousLocation = Point.Empty;
        ShortestFromStart = 0;
        DeadEnd = false;
        Used = false;
    }
    public Point PreviousLocation { get; set; }
    public Point Location { get; }
    public uint ShortestFromStart { get; set; } = 0;
    public bool DeadEnd { get; set; } = false;
    public bool EverBeenUsed { get; set; }
    public bool Wall { get; }
    private bool Used { get; set; }
    public bool StartPoint { get; }
    public bool EndPoint { get; }

    public MapDot(bool wall, Point location, bool startPoint = false, bool endPoint = false)
    {
        this.Wall = wall;
        this.Location = location;
        this.EndPoint = endPoint;
        this.StartPoint = startPoint;
    }

    public bool PathUsed
    {
        get => Used;
        set
        {
            if (Wall) return;
            Used = value;
            if (value) EverBeenUsed = true;
        }
    }
}