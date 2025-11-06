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
        Application.DocumentManager.DocumentActivated += OnDocumentActivated;
    });

    public void Terminate()
    {
    }

    private void OnDocumentActivated(object sender, DocumentCollectionEventArgs e)
    {
        // Подписываем ребра на изменения вершин.
        var loadProjectResult = LoadProjectResult.Load();
        loadProjectResult.GraphEdges.ForEach(edge => edge.RegisterWithVertices());
    }
}