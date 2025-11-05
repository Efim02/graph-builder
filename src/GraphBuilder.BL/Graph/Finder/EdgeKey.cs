namespace GraphBuilder.BL.Graph.Finder;

/// <summary>
/// Вспомогательный класс для ключа рёбер в словаре
/// </summary>
public class EdgeKey
{
    /// <summary>
    /// Конструктор ключа ребра
    /// </summary>
    /// <param name="startVertexId"> ID начальной вершины </param>
    /// <param name="endVertexId"> ID конечной вершины </param>
    public EdgeKey(long startVertexId, long endVertexId)
    {
        StartVertexId = startVertexId;
        EndVertexId = endVertexId;
    }

    /// <summary>
    /// ID начальной вершины
    /// </summary>
    public long StartVertexId { get; }

    /// <summary>
    /// ID конечной вершины
    /// </summary>
    public long EndVertexId { get; }

    /// <summary>
    /// Определяет равенство объектов EdgeKey
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is EdgeKey other)
            return StartVertexId == other.StartVertexId && EndVertexId == other.EndVertexId;
        return false;
    }

    /// <summary>
    /// Возвращает хэш-код для текущего объекта
    /// </summary>
    public override int GetHashCode()
    {
        unchecked
        {
            return (StartVertexId.GetHashCode() * 397) ^ EndVertexId.GetHashCode();
        }
    }
}