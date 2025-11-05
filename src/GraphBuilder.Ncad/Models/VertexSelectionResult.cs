namespace GraphBuilder.Ncad.Models;

/// <summary>
/// Результат выбора вершин пользователем
/// </summary>
public class VertexSelectionResult
{
    /// <summary>
    /// Конструктор результата выбора вершин
    /// </summary>
    /// <param name="startVertexId"> ID начальной вершины </param>
    /// <param name="endVertexId"> ID конечной вершины </param>
    /// <param name="success"> Флаг успешного выбора </param>
    public VertexSelectionResult(long startVertexId, long endVertexId, bool success)
    {
        StartVertexId = startVertexId;
        EndVertexId = endVertexId;
        Success = success;
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
    /// Флаг успешного выбора вершин
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// Отрицательный результат
    /// </summary>
    public static VertexSelectionResult False()
    {
        return new VertexSelectionResult(-1, -1, false);
    }
}