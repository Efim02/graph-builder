namespace GraphBuilder.Ncad.Commands;

using GraphBuilder.Ncad.CustomEntities;
using GraphBuilder.Ncad.Services;

using Multicad;
using Multicad.AplicationServices;
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
            var vertexOrNull = AddOrSelectVertex();
            if (vertexOrNull == null)
                break;

            if (lastVertex != null)
            {
                var edge = new CadGraphEdge(lastVertex.ID, vertexOrNull.ID);
                
                var edgeStyleService = new GraphEdgeStyleService();
                edge.SetStyle(edgeStyleService.Load());

                edge.DbEntity.AddToCurrentDocument();
            }

            lastVertex = vertexOrNull;

            McObjectManager.UpdateAll();
        }
    }

    /// <summary>
    /// Добавляет вершину на чертеж или выбирает вершину на чертеже, null если отмена операции.
    /// </summary>
    private static CadGraphVertex? AddOrSelectVertex()
    {
        // BuildCommand(Id текущей команды, Id команды назначенной на enter)
        var inputJig = new InputJig();
        inputJig.BuildCommand(0, 0)
            .Add(1, "Выбрать узел")
            .Add(2, "Добавить узел")
            .Complete();
        
        if (!inputJig.SelectKeyword("Как будете строить граф?"))
            return null;

        var commandEventArgs = inputJig.GetCommand();
        
        if (commandEventArgs.UserInput == "Выбрать узел")
        {
            while (true)
            {
                var mcObjectId = McObjectManager.SelectObject("Выберите узел");
                if (!mcObjectId.IdentifiesObjectOfType<CadGraphVertex>())
                {
                    McContext.ShowNotification("Выбранный объект не является точкой графа");
                    return null;
                }

                return mcObjectId.GetObjectOfType<CadGraphVertex>();
            }
        }

        if (commandEventArgs.UserInput == "Добавить узел")
        {
            var vertex = new CadGraphVertex();
            var result = vertex.PlaceObject();

            if (result != hresult.s_Ok)
                return null;
            
            return vertex;
        }

        return null;
    }
}