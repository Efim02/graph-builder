namespace GraphBuilder.Ncad.Views.Vertex;

using System.Windows;

using Multicad.Wpf;

public partial class VertexWindow : McWindow
{
    public VertexWindow()
    {
        InitializeComponent();
    }

    private void OnSave(object sender, RoutedEventArgs e)
    {
        Close();
    }
}