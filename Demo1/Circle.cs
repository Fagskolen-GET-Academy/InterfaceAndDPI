namespace Demo1;

public class Circle:IShape
{
    private int _radius;

    public Circle(int radius)
    {
        _radius = radius;
    }

    public double Area()
    {
        return Math.PI * _radius * _radius ;
    }
}