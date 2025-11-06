namespace GraphBuilder.Ncad.Commands;

using GraphBuilder.Ncad.CustomEntities;

using Multicad.CustomObjectBase;
using Multicad.DatabaseServices;

/// <summary>
/// Класс для хранения результатов загрузки проекта: вершины и рёбра графа
/// </summary>
public class LoadProjectResult
{
    public IReadOnlyList<CadGraphVertex> GraphVertices { get; set; } = null!;
    public IReadOnlyList<CadGraphEdge> GraphEdges { get; set; } = null!;

    public IReadOnlyList<McCustomBase> AllObjects => GraphVertices.Concat<McCustomBase>(GraphEdges).ToList();
    
    /// <summary>
    /// Загружаем данные проекта.
    /// </summary>
    public static LoadProjectResult Load()
    {
        // Создаем фильтр объектов, чтобы выбрать все объекты
        var objectFilter = new ObjectFilter(true)
        {
            AllObjects = true,
        };

        // Получаем идентификаторы всех объектов
        var allObjectsIds = objectFilter.GetObjects();

        // Загружаем объекты по их идентификаторам
        var allObjects = allObjectsIds.Select(McObjectManager.GetObject).ToList();

        // Фильтруем объекты по типу и сохраняем в соответствующие списки
        var cadGraphVertices = allObjects.OfType<CadGraphVertex>().ToList();
        var cadGraphEdges = allObjects.OfType<CadGraphEdge>().ToList();

        // Возвращаем результат в виде экземпляра LoadProjectResult
        return new LoadProjectResult
        {
            GraphVertices = cadGraphVertices,
            GraphEdges = cadGraphEdges
        };
    }
}