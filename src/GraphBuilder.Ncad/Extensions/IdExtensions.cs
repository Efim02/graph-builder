namespace GraphBuilder.Ncad.Extensions;

using Multicad.CustomObjectBase;

public static class IdExtensions
{
    /// <summary>
    /// Получает по ИД объекты.
    /// </summary>
    public static List<T> SelectById<T>(this IEnumerable<long> ids, 
        IReadOnlyCollection<T> collection,
        Func<T, long> selector)
    {
        var objectsByIds = collection.ToDictionary(selector, obj => obj);
        return ids.Select(id => objectsByIds[id]).ToList();
    } 
    
    /// <summary>
    /// Получает по ИД объекты.
    /// </summary>
    public static List<T> SelectById<T>(this IEnumerable<long> ids, 
        IReadOnlyCollection<T> collection) 
        where T : McCustomBase
    {
        return SelectById(ids, collection, customBase => customBase.ID.Handle);
    } 
}