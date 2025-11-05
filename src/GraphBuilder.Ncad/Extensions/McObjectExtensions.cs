namespace GraphBuilder.Ncad.Extensions;

using Multicad;

public static class McObjectExtensions
{
    public static List<long> ToHandles(this IEnumerable<McObject> mcObjects)
    {
        return mcObjects.Select(mcObject => mcObject.ID.Handle).ToList();
    }
}