namespace GraphBuilder.Ncad.Models;

using Multicad;

/// <summary>
/// Данные файла вершины.
/// </summary>
public class VertexFileInfo : IMcSerializable
{
    private byte[]? _data;
    private string? _fileName;

    /// <summary>
    /// Данные файла.
    /// </summary>
    public byte[] Data
    {
        get => _data;
        set => _data = value;
    }

    /// <summary>
    /// Название файла.
    /// </summary>
    public string FileName
    {
        get => _fileName;
        set => _fileName = value;
    }

    public bool IsFilled => _data != null && !string.IsNullOrWhiteSpace(_fileName);
    
    public hresult OnMcDeserialization(McSerializationInfo info)
    {
        info.GetValue(nameof(Data), out _data);
        info.GetValue(nameof(FileName), out _fileName);

        return hresult.s_Ok;
    }

    public hresult OnMcSerialization(McSerializationInfo info)
    {
        info.Add(nameof(Data), _data);
        info.Add(nameof(FileName), _fileName);
        return hresult.s_Ok;
    }
}