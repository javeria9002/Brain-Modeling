using System;

public class Point
{
    private double x_;
    private double y_;

    public Point(double x, double y)
    {
        x_ = x;
        y_ = y;
    } 

    public double X { get { return x_; } }
    public double Y { get { return y_; } }

    public double Distance(Point other)
    {
        double dx = x_ - other.x_;
        double dy = y_ - other.y_;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    public override string ToString()
    {
        return "(" + x_ + ", " + y_ + ")";
    }
}