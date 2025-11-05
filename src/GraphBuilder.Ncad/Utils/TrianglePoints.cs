namespace GraphBuilder.Ncad.Utils;

using Multicad.Geometry;

/// <summary>
/// Класс для хранения трёх точек треугольника
/// </summary>
public class TrianglePoints
{
    /// <summary>
    /// Конструктор класса TrianglePoints
    /// </summary>
    /// <param name="a"> Точка A треугольника </param>
    /// <param name="b"> Точка B треугольника </param>
    /// <param name="c"> Точка C треугольника </param>
    public TrianglePoints(Point3d a, Point3d b, Point3d c)
    {
        A = a;
        B = b;
        C = c;
    }

    /// <summary>
    /// Точка A треугольника
    /// </summary>
    public Point3d A { get; }

    /// <summary>
    /// Точка B треугольника
    /// </summary>
    public Point3d B { get; }

    /// <summary>
    /// Точка C треугольника
    /// </summary>
    public Point3d C { get; }

    /// <summary>
    /// В точки
    /// </summary>
    public Point3d[] ToClosestArrayPoints()
    {
        return new[] { A, B, C, A };
    }

    /// <summary>
    /// Возвращает точки для построения треугольника, вписанного в окружность с заданным радиусом.
    /// </summary>
    /// <param name="center"> Координаты центра окружности </param>
    /// <param name="radius"> Радиус окружности </param>
    /// <returns> Объект TrianglePoints с координатами трёх точек для построения треугольника </returns>
    public static TrianglePoints CreateTrianglePointsByRadius(Point3d center, double radius)
    {
        const double angleA = Math.PI / 2;
        const double angleB = 7 * Math.PI / 6;
        const double angleC = 11 * Math.PI / 6;

        var a = new Point3d(
            center.X + radius * Math.Cos(angleA),
            center.Y + radius * Math.Sin(angleA),
            center.Z
        );

        var b = new Point3d(
            center.X + radius * Math.Cos(angleB),
            center.Y + radius * Math.Sin(angleB),
            center.Z
        );

        var c = new Point3d(
            center.X + radius * Math.Cos(angleC),
            center.Y + radius * Math.Sin(angleC),
            center.Z
        );

        return new TrianglePoints(a, b, c);
    }
}