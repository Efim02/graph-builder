namespace GraphBuilder.Ncad.Commands;

using GraphBuilder.Ncad.CustomEntities;

using Multicad;
using Multicad.DatabaseServices;
using Multicad.Runtime;

/// <summary>
/// Команда для построения графа.
/// </summary>
public class CreateGraphVertexCommand
{
    [CommandMethod("GB_CREATE_GRAPH_VERTEX", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
    public static void CreateGraphVertex()
    {
        CadGraphVertex? lastVertex = null;
        while (true)
        {
            var vertex = new CadGraphVertex();
            var result = vertex.PlaceObject();

            if (result != hresult.s_Ok)
                break;

            if (lastVertex != null)
            {
                var edge = new CadGraphEdge(lastVertex.ID, vertex.ID);
                edge.DbEntity.AddToCurrentDocument();
            }

            lastVertex = vertex;
            McObjectManager.UpdateAll();
        }
    }
}