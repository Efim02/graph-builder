namespace GraphBuilder.Ncad.Views.Common;

using System.Windows;

using Multicad.Wpf;

public abstract class BaseMcWindow : McWindow
{
    protected void OnSave(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }
}