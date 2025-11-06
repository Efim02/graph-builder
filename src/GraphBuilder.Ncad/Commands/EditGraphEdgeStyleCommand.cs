namespace GraphBuilder.Ncad.Commands;

using System.Drawing;
using System.Windows;

using GraphBuilder.Ncad.Extensions;
using GraphBuilder.Ncad.Models;
using GraphBuilder.Ncad.Services;
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
        using var transaction = CurrentDocument.Database.TransactionManager.StartTransaction();
        var linetypeTable = (LinetypeTable)CurrentDocument.Database.LinetypeTableId.GetObject(OpenMode.ForRead);
        var lineTypeNames = linetypeTable.Cast<ObjectId>()
            .Select(id => (LinetypeTableRecord)id.GetObject(OpenMode.ForRead))
            .Select(record => record.Name)
            .ToList();

        var edgeStyleService = new GraphEdgeStyleService();
        var style = edgeStyleService.Load();
        var editEdgeStyleVM = new EditEdgeStyleVM
        {
            LineTypeNames = lineTypeNames,
            LineTypeName = style.LineType,
            LineThickness = style.LineThickness,
            LineColor = System.Windows.Media.Color.FromArgb(
                style.LineColor.A, 
                style.LineColor.R,
                style.LineColor.G, 
                style.LineColor.B),
        };
        
        var editEdgeStyleWindow = new EditEdgeStyleWindow() {DataContext = editEdgeStyleVM};
        if (editEdgeStyleWindow.ShowDialog(McContext.MainWindowHandle) != true)
            return;

        var newStyle = new GraphEdgeStyle
        {
            LineType = editEdgeStyleVM.LineTypeName,
            LineThickness = editEdgeStyleVM.LineThickness,
            LineColor = Color.FromArgb(
                editEdgeStyleVM.LineColor.A, 
                editEdgeStyleVM.LineColor.R, 
                editEdgeStyleVM.LineColor.G,
                editEdgeStyleVM.LineColor.B)
        };
        edgeStyleService.Save(newStyle);
        
        LoadProjectResult.Load().GraphEdges.ForEach(edge =>
        {
            edge.LineType = newStyle.LineType;
            edge.LineThickness = newStyle.LineThickness;
            edge.LineColor = newStyle.LineColor;
        });
        
        transaction.Commit();
    });
}