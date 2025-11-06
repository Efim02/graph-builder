namespace GraphBuilder.Ncad.Views.Vertex;

using GraphBuilder.Ncad.CustomEntities;

/// <summary>
/// Вью-модель вершины.
/// </summary>
public class VertexVM : BaseViewModel
{
    /// <summary>
    /// Все виды форм вершины.
    /// </summary>
    public static readonly IReadOnlyCollection<VertexFormKind> VertexFormKinds =
        Enum.GetValues(typeof(VertexFormKind)).Cast<VertexFormKind>().ToList();

    /// <summary>
    /// Форма вершины.
    /// </summary>
    public VertexFormKind VertexFormKind { get; set; }
}