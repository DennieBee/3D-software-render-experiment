using System.Timers;

namespace _3drender;

public class Vec3 {
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}

public class LineIdx {
    public int A { get; set; }
    public int B { get; set; }
}

public partial class RenderCanvas : Form
{
    private readonly float _focalLength = 5;

    private readonly List<Vec3> _cube = new() 
    {
        new Vec3() { X = -1, Y = -1, Z = -1 },
        new Vec3() { X = 1, Y = -1, Z = -1 },
        new Vec3() { X = 1, Y = 1, Z = -1 },
        new Vec3() { X = -1, Y = 1, Z = -1 },
        new Vec3() { X = -1, Y = -1, Z = 1 },
        new Vec3() { X = 1, Y = -1, Z = 1 },
        new Vec3() { X = 1, Y = 1, Z = 1 },
        new Vec3() { X = -1, Y = 1, Z = 1 },
    };

    private readonly List<LineIdx> _lines = new() 
    {
        new LineIdx() { A = 0, B = 1 },
        new LineIdx() { A = 1, B = 2 },
        new LineIdx() { A = 2, B = 3 },
        new LineIdx() { A = 3, B = 0 },
        new LineIdx() { A = 0, B = 4 },
        new LineIdx() { A = 1, B = 5 },
        new LineIdx() { A = 2, B = 6 },
        new LineIdx() { A = 3, B = 7 },
        new LineIdx() { A = 4, B = 5 },
        new LineIdx() { A = 5, B = 6 },
        new LineIdx() { A = 6, B = 7 },
        new LineIdx() { A = 7, B = 4 },
    };

    public RenderCanvas()
    {   
        Width = 1280;
        Height = 720;
        DoubleBuffered = true;
        Text = "3D Software Render Experiment";

        var timer = new System.Timers.Timer();
        timer.Elapsed += new ElapsedEventHandler(TimerEvent);
        timer.Interval = 16;
        timer.Start();
    }

    private void TimerEvent(object source, ElapsedEventArgs e) 
    {
        foreach (var vertex in _cube)
        {
            RotateY(vertex, 0.01);
        }
        
        Refresh();
    }

    private static void RotateY(Vec3 vertex, double rotation)
    {
        double a = Math.Atan(vertex.Z / vertex.X);
        double s = vertex.Z / Math.Sin(a);

        a += rotation;

        double x = Math.Cos(a) * s;
        double z = Math.Sin(a) * s;

        vertex.X = (float)x;
        vertex.Z = (float)z;
    }

    private float ProjectX(Vec3 position) 
    {
        return _focalLength * position.X / (_focalLength + position.Z) * 200 + (Width / 2);
    }

    private float ProjectY(Vec3 position) 
    {
        return _focalLength * position.Y / (_focalLength + position.Z) * 200 + (Height / 2);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        e.Graphics.Clear(Color.White);

        foreach (var vertex in _cube)
        {
            float projectedX = ProjectX(vertex);
            float projectedY = ProjectY(vertex);

            e.Graphics.FillRectangle(Brushes.Black, projectedX - 4.0f, projectedY - 4.0f, 8.0f, 8.0f);    
        }

        foreach (var line in _lines)
        {
            float projectedXA = ProjectX(_cube[line.A]);
            float projectedYA = ProjectY(_cube[line.A]);
            float projectedXB = ProjectX(_cube[line.B]);
            float projectedYB = ProjectY(_cube[line.B]);

            e.Graphics.DrawLine(Pens.Blue, projectedXA, projectedYA, projectedXB, projectedYB);
        }
    }
}
