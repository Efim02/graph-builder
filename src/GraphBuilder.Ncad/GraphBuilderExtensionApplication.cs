namespace GraphBuilder.Ncad;

using GraphBuilder.Ncad.Commands;
using GraphBuilder.Ncad.Extensions;
using GraphBuilder.Ncad.Utils;

using HostMgd.ApplicationServices;

using Teigha.Runtime;

public class GraphBuilderExtensionApplication : IExtensionApplication
{
    public void Initialize() => SafeUtils.Execute(() =>
    {
        Application.DocumentManager.DocumentCreated += OnDocumentOpened;
        Application.DocumentManager.DocumentToBeDestroyed += OnDocumentClosing;
    });

    private void OnDocumentClosing(object sender, DocumentCollectionEventArgs e)
    {
        // Отписываем ребра на изменения вершин.
        var loadProjectResult = LoadProjectResult.Load();
        loadProjectResult.GraphEdges.ForEach(edge => edge.UnregisterFromVertices());
    }

    public void Terminate()
    {
    }

    private void OnDocumentOpened(object sender, DocumentCollectionEventArgs e)
    {
        // Подписываем ребра на изменения вершин.
        var loadProjectResult = LoadProjectResult.Load();
        loadProjectResult.GraphEdges.ForEach(edge => edge.RegisterWithVertices());
    }
}