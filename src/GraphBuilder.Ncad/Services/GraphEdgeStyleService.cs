namespace GraphBuilder.Ncad.Services;

using System.Drawing;
using System.Windows;

using GraphBuilder.Ncad.Commands;
using GraphBuilder.Ncad.Models;
using GraphBuilder.Ncad.Utils;

using Multicad.DatabaseServices;

/// <summary>
/// Сервис для работы с настройками стиля ребер графа.
/// </summary>
public class GraphEdgeStyleService
{
    private const string GRAPH_EDGE_STYLE = "GraphEdgeStyle";

    public GraphEdgeStyle Load()
    {
        var mcDocument = McDocumentsManager.GetActiveDoc();
        var value = mcDocument.CustomProperties[GRAPH_EDGE_STYLE];
        if (value == null)
        {
            var loadProjectResult = LoadProjectResult.Load();
            var edges = loadProjectResult.GraphEdges;

            if (edges.Any())
            {
                var graphEdge = edges.First();
                return new GraphEdgeStyle
                {
                    LineType = graphEdge.LineType,
                    LineThickness = graphEdge.LineThickness,
                    LineColor = graphEdge.LineColor
                };
            }

            MessageBox.Show("Отсутствуют ребра графа, взятые настройки по-умолчанию", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            return new GraphEdgeStyle();
        }

        return XmlUtils.DeserializeXml<GraphEdgeStyle>(value.ToString() ?? string.Empty)!;
    }

    public void Save(GraphEdgeStyle graphEdgeStyle)
    {
        var mcDocument = McDocumentsManager.GetActiveDoc();
        mcDocument.CustomProperties[GRAPH_EDGE_STYLE] = XmlUtils.SerializeXml(graphEdgeStyle);
    }
}