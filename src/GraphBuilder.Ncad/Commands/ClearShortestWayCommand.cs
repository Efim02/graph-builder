namespace GraphBuilder.Ncad.Commands;

using GraphBuilder.Ncad.CustomEntities;
using GraphBuilder.Ncad.Utils;

using Multicad.DatabaseServices;
using Multicad.Runtime;

/// <summary>
/// Команда убирает выделение короткого пути.
/// </summary>
public class ClearShortestWayCommand
{
    [CommandMethod("GB_CLEAR_SHORTEST_WAY", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
    public static void ClearShortestWay() => SafeUtils.Execute(() =>
    {
        var objectFilter = new ObjectFilter(true)
        {
            AllObjects = true,
        };
        var allObjectsIds = objectFilter.GetObjects();
        var allObjects = allObjectsIds.Select(McObjectManager.GetObject).ToList();

        var cadGraphVertices = allObjects.OfType<CadGraphVertex>().ToList();
        var cadGraphEdges = allObjects.OfType<CadGraphEdge>().ToList();

        cadGraphVertices.ForEach(vertex => vertex.IsSelected = false);
        cadGraphEdges.ForEach(edge => edge.IsSelected = false);
    });
}