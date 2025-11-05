namespace GraphBuilder.Ncad.Extensions;

using Multicad;

public static class McObjectIdExtensions
{
    public static List<McObjectId> ToMcObjectIds(this IEnumerable<long> handles)
    {
        return handles.Select(McObjectId.FromHandle).ToList();
    }

    public static List<long> ToHandles(this IEnumerable<McObjectId> mcObjectIds)
    {
        return mcObjectIds.Select(id => id.Handle).ToList();
    }
}