namespace GraphBuilder.Ncad.Views.Vertex;

using System.IO;

using GraphBuilder.Ncad.CustomEntities;
using GraphBuilder.Ncad.Models;
using GraphBuilder.Ncad.Views.Common;

using Microsoft.Win32;

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

    private VertexFileInfo? _fileInfo;

    public ActionBaseCommand SelectFileCommand => new(() => SelectFile());

    public ActionBaseCommand ClearFileCommand => new(() => FileInfo = null);

    /// <summary>
    /// Форма вершины.
    /// </summary>
    public VertexFormKind VertexFormKind { get; set; }

    /// <summary>
    /// Прикрепленный файл к вершине.
    /// </summary>
    public VertexFileInfo? FileInfo
    {
        get => _fileInfo;
        set
        {
            if (Equals(value, _fileInfo))
                return;
            _fileInfo = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Выбирает файл с определенным расширением.
    /// </summary>
    private void SelectFile(string fileType = "Все файлы", string fileExtension = "*.*")
    {
        var openFileDialog = new OpenFileDialog();

        // Фильтр для конкретного типа файлов
        openFileDialog.Filter = $"{fileType} (*.{fileExtension})|*.{fileExtension}|Все файлы (*.*)|*.*";
        openFileDialog.FilterIndex = 1;
        openFileDialog.RestoreDirectory = true;

        if (openFileDialog.ShowDialog() != true)
            return;

        var bytes = File.ReadAllBytes(openFileDialog.FileName);
        var fileName = Path.GetFileName(openFileDialog.FileName);

        FileInfo = new VertexFileInfo
        {
            Data = bytes,
            FileName = fileName
        };
    }
}