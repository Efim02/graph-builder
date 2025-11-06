namespace GraphBuilder.Ncad.Commands;

using System.Drawing;
using System.Windows;

using GraphBuilder.Ncad.Extensions;
using GraphBuilder.Ncad.Utils;
using GraphBuilder.Ncad.Views.Edge;

using HostMgd.ApplicationServices;

using Multicad.AplicationServices;
using Multicad.DatabaseServices;
using Multicad.NativeLineTypes;
using Multicad.Runtime;

using Teigha.DatabaseServices;

using Application = HostMgd.ApplicationServices.Application;

/// <summary>
/// Команда для редактирования стиля ребер графа.
/// </summary>
public class EditGraphEdgeStyleCommand
{
    private static Document CurrentDocument => Application.DocumentManager.CurrentDocument;

    [CommandMethod("GB_EDIT_GRAPH_EDGE_STYLE", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
    public static void EditGraphEdgeStyle() => SafeUtils.Execute(() =>
    {
        var loadProjectResult = LoadProjectResult.Load();
        var edges = loadProjectResult.GraphEdges;

        if (!edges.Any())
        {
            MessageBox.Show("Отсутствуют ребра графа", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        using var transaction = CurrentDocument.Database.TransactionManager.StartTransaction();
        var linetypeTable = (LinetypeTable)CurrentDocument.Database.LinetypeTableId.GetObject(OpenMode.ForRead);
        var lineTypeNames = linetypeTable.Cast<ObjectId>()
            .Select(id => (LinetypeTableRecord)id.GetObject(OpenMode.ForRead))
            .Select(record => record.Name)
            .ToList();

        var firstEdge = edges.First();
        var editEdgeStyleVM = new EditEdgeStyleVM
        {
            LineTypeNames = lineTypeNames,
            LineTypeName = firstEdge.LineType,
            LineThickness = firstEdge.LineThickness,
            LineColor = System.Windows.Media.Color.FromArgb(
                firstEdge.LineColor.A, 
                firstEdge.LineColor.R,
                firstEdge.LineColor.G, 
                firstEdge.LineColor.B),
        };
        
        var editEdgeStyleWindow = new EditEdgeStyleWindow() {DataContext = editEdgeStyleVM};
        if (editEdgeStyleWindow.ShowDialog(McContext.MainWindowHandle) != true)
            return;
        
        edges.ForEach(edge =>
        {
            edge.LineType = editEdgeStyleVM.LineTypeName;
            edge.LineThickness = editEdgeStyleVM.LineThickness;
            edge.LineColor = Color.FromArgb(
                editEdgeStyleVM.LineColor.A, 
                editEdgeStyleVM.LineColor.R, 
                editEdgeStyleVM.LineColor.G,
                editEdgeStyleVM.LineColor.B);
        });
        
        transaction.Commit();
    });
}