namespace GraphBuilder.BL.Graph.Finder;

/// <summary>
/// Вспомогательный класс для представления соединения между вершинами
/// </summary>
public class GraphConnection
{
    /// <summary>
    /// Конструктор соединения
    /// </summary>
    /// <param name="neighborId"> ID соседней вершины </param>
    /// <param name="weight"> Вес соединения </param>
    public GraphConnection(long neighborId, double weight)
    {
        NeighborId = neighborId;
        Weight = weight;
    }

    /// <summary>
    /// ID соседней вершины
    /// </summary>
    public long NeighborId { get; }

    /// <summary>
    /// Вес соединения
    /// </summary>
    public double Weight { get; }
}