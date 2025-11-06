namespace GraphBuilder.Ncad.Views.Edge;

using System.Windows.Media;

/// <summary>
/// Вью-модель редактирования стиля линий.
/// </summary>
public class EditEdgeStyleVM
{
    /// <summary>
    /// Названия типов линий.
    /// </summary>
    public IReadOnlyList<string> LineTypeNames { get; init; } = null!;

    /// <summary>
    /// Название типа линий.
    /// </summary>
    public string LineTypeName { get; set; } = null!;
    
    /// <summary>
    /// Толщина линий.
    /// </summary>
    public int LineThickness { get; set; }
    
    /// <summary>
    /// Цвет линий.
    /// </summary>
    public Color LineColor { get; set; }
}