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
        double area = Math.PI* _radius * _radius ;
        return area;
    }
}