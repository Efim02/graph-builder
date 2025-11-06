namespace GraphBuilder.Ncad.Commands;

using System.Drawing;

using GraphBuilder.BL.Graph;
using GraphBuilder.BL.Graph.Finder;
using GraphBuilder.BL.Graph.Vertices;
using GraphBuilder.Ncad.CustomEntities;
using GraphBuilder.Ncad.Extensions;
using GraphBuilder.Ncad.Models;
using GraphBuilder.Ncad.Utils;

using HostMgd.ApplicationServices;

using Multicad;
using Multicad.AplicationServices;
using Multicad.Constants;
using Multicad.DatabaseServices;
using Multicad.Runtime;

using Teigha.DatabaseServices;

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
        SafeUtils.Execute(() =>
        {
            var findArgs = SelectVertices();
            if (!findArgs.Success)
                return;

            var loadProjectResult = LoadProjectResult.Load();
            var cadGraphVertices = loadProjectResult.GraphVertices;
            var cadGraphEdges = loadProjectResult.GraphEdges;

            var graphVertices = cadGraphVertices
                .Select(x => new GraphVertex(x.ID.Handle, x.CenterPoint.X, x.CenterPoint.Y))
                .ToList();
            var graphEdges = cadGraphEdges
                .Select(x => new GraphEdge(x.ID.Handle, x.StartVertex.ID.Handle, x.EndVertex.ID.Handle, x.Length))
                .ToList();

            var pathFinder = new SimpleGraphPathFinder();
            pathFinder.Initialize(graphVertices, graphEdges);

            var foundCadVertices = pathFinder.FindShortestPath(findArgs.StartVertexId, findArgs.EndVertexId)
                .SelectById(cadGraphVertices);
            if (foundCadVertices.Count <= 0)
                return;

            var foundCadEdges = pathFinder.GetRouteEdgeIds(foundCadVertices.ToHandles())
                .SelectById(cadGraphEdges);

            foundCadVertices.ForEach(vertex => vertex.IsSelected = true);
            foundCadEdges.ForEach(edge => edge.IsSelected = true);

            cadGraphVertices.Except(foundCadVertices).ForEach(vertex => vertex.IsSelected = false);
            cadGraphEdges.Except(foundCadEdges).ForEach(vertex => vertex.IsSelected = false);
        });
    }

    /// <summary>
    /// Получает начальную и конечную вершины от пользователя
    /// </summary>
    /// <returns> Результат выбора вершин </returns>
    private static ShortPathFindArgs SelectVertices()
    {
        var jig = new InputJig();
        var inputResultStart = jig.SelectObject("Выберите вершину старта.");
        if (inputResultStart.Result != InputResult.ResultCode.Normal ||
            inputResultStart.ObjectId.GetObject() is not CadGraphVertex)
            return ShortPathFindArgs.False();

        var inputResultEnd = jig.SelectObject("Выберите вершину финиша.");
        if (inputResultEnd.Result != InputResult.ResultCode.Normal ||
            inputResultEnd.ObjectId.GetObject() is not CadGraphVertex)
            return ShortPathFindArgs.False();

        return new ShortPathFindArgs(inputResultStart.ObjectId.Handle, inputResultEnd.ObjectId.Handle, true);
    }
}