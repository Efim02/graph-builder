namespace GraphBuilder.BL.Graph;

using GraphBuilder.BL.Graph.Vertices;

/// <summary>
/// DTO класс для представления ребра графа
/// </summary>
public class GraphEdge
{
    /// <summary>
    /// Конструктор класса GraphEdgeDto
    /// </summary>
    /// <param name="id"> Уникальный идентификатор ребра </param>
    /// <param name="startVertexId"> ID начальной вершины ребра </param>
    /// <param name="endVertexId"> ID конечной вершины ребра </param>
    /// <param name="length"> Длина ребра </param>
    public GraphEdge(long id, long startVertexId, long endVertexId, double length)
    {
        Id = id;
        StartVertexId = startVertexId;
        EndVertexId = endVertexId;
        Length = length;
    }

    /// <summary>
    /// Уникальный идентификатор ребра
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// ID начальной вершины ребра
    /// </summary>
    public long StartVertexId { get; set; }

    /// <summary>
    /// ID конечной вершины ребра
    /// </summary>
    public long EndVertexId { get; set; }

    /// <summary>
    /// Список промежуточных точек ребра (для визуализации изогнутых рёбер)
    /// </summary>
    public List<Vertex> IntermediatePoints { get; set; } = new();

    /// <summary>
    /// Длина ребра
    /// </summary>
    public double Length { get; set; }
}