namespace GraphBuilder.Ncad.Commands;

using GraphBuilder.BL.Graph;
using GraphBuilder.BL.Graph.Finder;
using GraphBuilder.BL.Graph.Vertices;
using GraphBuilder.Ncad.CustomEntities;
using GraphBuilder.Ncad.Models;
using GraphBuilder.Ncad.Utils;

using HostMgd.ApplicationServices;

using Multicad.DatabaseServices;
using Multicad.Runtime;

/// <summary>
/// Команда для поиска ближайшего пути по графу.
/// </summary>
public class FindShortestWayCommand
{
    /// <summary>
    /// Обрабатывает команду поиска кратчайшего пути в графе
    /// </summary>
    [CommandMethod("GB_FIND_SHORTEST_WAY", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
    public static void FindShortestWayCmd()
    {
        var vertices = GetVertices();
        if (!vertices.Success)
            return;

        var document = Application.DocumentManager.MdiActiveDocument;
        var transaction = document.Database.TransactionManager.StartTransaction();

        var allObjectsIds = CadUtils.GetAllDocumentObjectIds(document, transaction);
        var allObjects = allObjectsIds.Select(McObjectManager.GetObject).ToList();
        var graphVertexes = allObjects.OfType<CadGraphVertex>().ToList();
        var graphEdges = allObjects.OfType<CadGraphEdge>().ToList();

        var vertexDtoList =
            graphVertexes.Select(x => new GraphVertex(x.ID.Handle, x.CenterPoint.X, x.CenterPoint.Y)).ToList();
        var edgeDtoList =
            graphEdges.Select(x => new GraphEdge(x.ID.Handle, x.StartVertex.ID.Handle, x.EndVertex.ID.Handle, x.Length))
                .ToList();

        var pathFinder = new SimpleGraphPathFinder();
        pathFinder.Initialize(vertexDtoList, edgeDtoList);

        var routeVertices = pathFinder.FindShortestPath(vertices.StartVertexId, vertices.EndVertexId);
        if (routeVertices.Count <= 0)
            return;

        var routeEdgeIds = pathFinder.GetRouteEdgeIds(routeVertices);
        foreach (var vertexId in routeVertices)
        {
            var vertex = graphVertexes.FirstOrDefault(v => v.ID.Handle == vertexId);
            vertex?.DbEntity.Highlight(true);
        }

        foreach (var edgeId in routeEdgeIds)
        {
            var edge = graphEdges.FirstOrDefault(e => e.ID.Handle == edgeId);
            edge?.DbEntity.Highlight(true);
        }
    }

    /// <summary>
    /// Получает начальную и конечную вершины от пользователя
    /// </summary>
    /// <returns> Результат выбора вершин </returns>
    private static VertexSelectionResult GetVertices()
    {
        var jig = new InputJig();
        var inputResultStart = jig.SelectObject("Выберите вершину старта.");
        if (inputResultStart.Result != InputResult.ResultCode.Normal ||
            inputResultStart.ObjectId.GetObject() is not CadGraphVertex)
            return VertexSelectionResult.False();

        var inputResultEnd = jig.SelectObject("Выберите вершину финиша.");
        if (inputResultEnd.Result != InputResult.ResultCode.Normal ||
            inputResultEnd.ObjectId.GetObject() is not CadGraphVertex)
            return VertexSelectionResult.False();

        return new VertexSelectionResult(inputResultStart.ObjectId.Handle, inputResultEnd.ObjectId.Handle, true);
    }
}