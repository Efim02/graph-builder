namespace GraphBuilder.Ncad.CustomEntities;

using System.Drawing;

using GraphBuilder.Ncad.Abstractions;
using GraphBuilder.Ncad.Utils;
using GraphBuilder.Ncad.Views.Vertex;

using Multicad;
using Multicad.AplicationServices;
using Multicad.Constants;
using Multicad.CustomObjectBase;
using Multicad.DatabaseServices;
using Multicad.Geometry;
using Multicad.Runtime;

/// <summary>
/// Cad-объект вершины графа
/// </summary>
[CustomEntity("B31FD339-831A-4D4E-951F-A62EC5E23917", "GB_GraphVertex", "Вершина графа")]
public class CadGraphVertex : McCustomBase, IVertexObservable, ISelectable
{
    private const int RADIUS = 200;

    private readonly List<IVertexObserver> _observers = new();

    public Point3d CenterPoint = new(0, 0, 0);
    private VertexFormKind _vertexFormKind;
    private bool _isSelected;

    /// <summary>
    /// Выбранная форма вершина.
    /// </summary>
    public VertexFormKind VertexFormKind
    {
        get => _vertexFormKind;
        set
        {
            TryModify(0);
            _vertexFormKind = value;
        }
    }

    /// <summary>
    /// Выделена ли точка.
    /// </summary>
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            TryModify(0);
            _isSelected = value;
        }
    }

    /// <inheritdoc />
    public void AddObserver(IVertexObserver observer)
    {
        if (!_observers.Contains(observer))
            _observers.Add(observer);
    }

    /// <inheritdoc />
    public void NotifyErased()
    {
        foreach (var observer in _observers.ToList())
        {
            observer.OnVertexErased(this);
        }
    }

    /// <inheritdoc />
    public void NotifyMoved()
    {
        foreach (var observer in _observers.ToList())
        {
            observer.OnVertexMoved(this);
        }
    }

    /// <inheritdoc />
    public void RemoveObserver(IVertexObserver observer)
    {
        _observers.Remove(observer);
    }

    /// <inheritdoc />
    public override bool GetGripPoints(GripPointsInfo info)
    {
        info.AppendGrip(new McSmartGrip<CadGraphVertex>(CenterPoint, (obj, _, offset) =>
        {
            obj.TryModify(0);
            obj.CenterPoint += offset;
            obj.NotifyMoved();
        }));

        return true;
    }

    /// <inheritdoc />
    public override void OnDraw(GeometryBuilder builder)
    {
        builder.Clear();

        builder.LineType = LineTypes.ByObject;
        builder.LineWidth = LineWeights.ByObject;

        if (_vertexFormKind == VertexFormKind.Circle)
        {
            builder.Color = Color.Blue;
            builder.DrawCircle(CenterPoint, RADIUS);
        }

        if (_vertexFormKind == VertexFormKind.Triangle)
        {
            builder.Color = Color.Red;
            var trianglePoints = TrianglePoints.CreateTrianglePointsByRadius(CenterPoint, RADIUS);
            builder.DrawPolyline(trianglePoints.ToClosestArrayPoints());
        }

        if (IsSelected)
        {
            builder.Color = Color.Yellow;
            builder.DrawCircle(CenterPoint, RADIUS * 1.1d);
        }
    }

    /// <inheritdoc />
    public override void OnErase()
    {
        NotifyErased();
        base.OnErase();
    }

    /// <inheritdoc />
    public override hresult OnMcDeserialization(McSerializationInfo info)
    {
        if (!info.GetValue(nameof(CenterPoint), out CenterPoint))
            return hresult.e_Fail;
        if (!info.GetValue(nameof(_vertexFormKind), out int vertexFormType))
            return hresult.e_Fail;

        _vertexFormKind = GetGraphVertexForm(vertexFormType);
        return hresult.s_Ok;
    }

    /// <inheritdoc />
    public override hresult OnMcSerialization(McSerializationInfo info)
    {
        info.Add(nameof(CenterPoint), CenterPoint);
        info.Add(nameof(_vertexFormKind), (int)_vertexFormKind);

        return hresult.s_Ok;
    }

    /// <inheritdoc />
    public override void OnTransform(Matrix3d tfm)
    {
        TryModify(0);
        CenterPoint = CenterPoint.TransformBy(tfm);
        NotifyMoved(); // Уведомляем наблюдателей о трансформации
    }

    /// <inheritdoc />
    public override hresult PlaceObject(PlaceFlags lInsertType)
    {
        var jig = new InputJig();

        // Выбор типа вершины
        var res = jig.GetIntNumber("Выберите тип объекта(1 - треугольник, 2 - круг):",
            out var graphVertexFormType);
        if (!res)
            return hresult.e_Fail;

        // Выбор позиции
        var pointInputResult = jig.GetPoint("Выберите точку вставки:");
        if (pointInputResult.Result != InputResult.ResultCode.Normal)
            return hresult.e_Fail;

        CenterPoint = pointInputResult.Point;
        _vertexFormKind = GetGraphVertexForm(graphVertexFormType);

        DbEntity.AddToCurrentDocument();
        jig.ExcludeObject(ID);

        // Интерактивное перемещение
        jig.MouseMove = (_, a) =>
        {
            TryModify(0);
            CenterPoint = a.Point;
            DbEntity.Update();
            InputJig.PropertyInpector.UpdateProperties();
        };

        return hresult.s_Ok;
    }

    /// <inheritdoc />
    public override hresult OnEdit(Point3d pnt, EditFlags lFlag)
    {
        var vertexVM = new VertexVM { VertexFormKind = _vertexFormKind };
        var vertexWindow = new VertexWindow { DataContext = vertexVM };
        if (vertexWindow.ShowDialog(McContext.MainWindowHandle) != true)
            return hresult.e_Abort;

        VertexFormKind = vertexVM.VertexFormKind;

        return hresult.s_Ok;
    }

    /// <summary>
    /// Возвращает enum-представление формы вершины по её номеру.
    /// </summary>
    private VertexFormKind GetGraphVertexForm(int type)
    {
        return type switch
        {
            1 => VertexFormKind.Triangle,
            _ => VertexFormKind.Circle,
        };
    }
}