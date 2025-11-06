namespace GraphBuilder.Ncad.Commands;

using GraphBuilder.Ncad.CustomEntities;
using GraphBuilder.Ncad.Services;

using Multicad;
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
        if (edge.PlaceObject() != hresult.s_Ok)
            return;

        var edgeStyleService = new GraphEdgeStyleService();
        edge.SetStyle(edgeStyleService.Load());

        McObjectManager.UpdateAll();
    }
}