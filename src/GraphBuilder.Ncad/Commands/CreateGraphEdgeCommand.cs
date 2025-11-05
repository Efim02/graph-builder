namespace GraphBuilder.Ncad.Commands;

using GraphBuilder.Ncad.CustomEntities;

using Multicad.DatabaseServices;
using Multicad.Runtime;

/// <summary>
/// Команда создания дополнительных ребёр графа для соединения вершин.
/// </summary>
public class CreateGraphEdgeCommand
{
    [CommandMethod("GB_CREATE_GRAPH_EDGE", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
    public static void CreateGraphEdge()
    {
        var edge = new CadGraphEdge();
        edge.PlaceObject();
        McObjectManager.UpdateAll();
    }
}