namespace GraphBuilder.BL.Graph.Vertices;

/// <summary>
/// DTO для вершины графа.
/// </summary>
public class GraphVertex : Vertex
{
    public GraphVertex(long id, double x, double y) : base(id, x, y)
    {
    }
}