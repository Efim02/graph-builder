namespace GraphBuilder.BL.Graph.Vertices;

/// <summary>
/// DTO для вершины графа.
/// </summary>
public class Vertex
{
    public Vertex(long id, double x, double y)
    {
        Id = id;
        X = x;
        Y = y;
    }

    public long Id { get; set; }

    public double X { get; set; }

    public double Y { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Vertex dto && Id == dto.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}