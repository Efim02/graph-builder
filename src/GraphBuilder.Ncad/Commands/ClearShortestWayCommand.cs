namespace GraphBuilder.Ncad.Commands;

using GraphBuilder.Ncad.Abstractions;
using GraphBuilder.Ncad.Extensions;
using GraphBuilder.Ncad.Utils;

using Multicad.Runtime;

/// <summary>
/// Команда убирает выделение короткого пути.
/// </summary>
public class ClearShortestWayCommand
{
    [CommandMethod("GB_CLEAR_SHORTEST_WAY", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
    public static void ClearShortestWay() => SafeUtils.Execute(() =>
    {
        var loadProjectResult = LoadProjectResult.Load();
        loadProjectResult.AllObjects.Cast<ISelectable>().ForEach(vertex => vertex.IsSelected = false);
    });
}