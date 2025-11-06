namespace GraphBuilder.Ncad.Models;

using System.Drawing;
using System.Xml.Serialization;

/// <summary>
/// Стиль ребер графа.
/// </summary>
public class GraphEdgeStyle
{
    /// <summary>
    /// Тип линии.
    /// </summary>
    public string LineType { get; set; } = null!;

    /// <summary>
    /// Толщина линии.
    /// </summary>
    public int LineThickness { get; set; }

    /// <summary>
    /// Цвет линии, Argb.
    /// </summary>
    public int LineColorArgb { get; set; }

    /// <summary>
    /// Цвет линии.
    /// </summary>
    [XmlIgnore]
    public Color LineColor
    {
        get => Color.FromArgb(LineColorArgb);
        set => LineColorArgb = value.ToArgb();
    }
}