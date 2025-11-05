namespace GraphBuilder.Ncad.Utils;

using Multicad.AplicationServices;
using Multicad.DatabaseServices;

public static class SafeUtils
{
    public static void Execute(Action action)
    {
        try
        {
            action();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            McContext.ShowNotification($"Произошла ошибка:\n{exception.GetType().Name}: {exception.Message}");
        }
    }
}