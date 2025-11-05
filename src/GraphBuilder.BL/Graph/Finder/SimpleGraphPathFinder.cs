namespace GraphBuilder.BL.Graph.Finder;

using GraphBuilder.BL.Graph.Vertices;

/// <summary>
/// Простой менеджер для поиска кратчайшего пути в графе
/// </summary>
public class SimpleGraphPathFinder
{
    private Dictionary<long, List<GraphConnection>> _connections = null!;
    private Dictionary<EdgeKey, GraphEdge> _edgesDictionary = null!;
    private Dictionary<long, GraphVertex> _vertices = null!;

    /// <summary>
    /// Поиск кратчайшего пути.
    /// </summary>
    /// <param name="startId"> ID начальной вершины </param>
    /// <param name="endId"> ID конечной вершины </param>
    /// <returns> Список ID вершин, через которые должен проходить маршрут </returns>
    public List<long> FindShortestPath(long startId, long endId)
    {
        ValidateVerticesExist(startId, endId);

        var distances = new Dictionary<long, double>();
        var previous = new Dictionary<long, long>();
        var unvisited = new HashSet<long>();

        foreach (var vertexId in _vertices.Keys)
        {
            distances[vertexId] = double.MaxValue;
            previous[vertexId] = -1;
            unvisited.Add(vertexId);
        }

        distances[startId] = 0;

        while (unvisited.Count > 0)
        {
            var currentId = GetClosestUnvisitedVertex(unvisited, distances);
            if (currentId == -1)
                break;

            unvisited.Remove(currentId);
            if (currentId == endId)
                break;

            UpdateNeighborDistances(currentId, distances, previous, unvisited);
        }

        return BuildPath(previous, endId);
    }

    /// <summary>
    /// Получает рёбра маршрута по списку вершин
    /// </summary>
    /// <param name="routeVertices"> Список ID вершин маршрута </param>
    /// <returns> Список рёбер маршрута </returns>
    public List<GraphEdge> GetRouteEdges(List<long> routeVertices)
    {
        if (routeVertices.Count < 2)
            return new List<GraphEdge>();

        var routeEdges = new List<GraphEdge>();

        for (var i = 0; i < routeVertices.Count - 1; i++)
        {
            var startVertexId = routeVertices[i];
            var endVertexId = routeVertices[i + 1];

            var edgeKey = new EdgeKey(startVertexId, endVertexId);
            if (_edgesDictionary.TryGetValue(edgeKey, out var edge))
                routeEdges.Add(edge);
            else
            {
                var reverseEdgeKey = new EdgeKey(endVertexId, startVertexId);
                if (_edgesDictionary.TryGetValue(reverseEdgeKey, out edge))
                    routeEdges.Add(edge);
                else
                {
                    throw new InvalidOperationException(
                        $"Ребро между вершинами {startVertexId} и {endVertexId} не найдено");
                }
            }
        }

        return routeEdges;
    }

    /// <summary>
    /// Получает ID рёбер маршрута по списку вершин
    /// </summary>
    /// <param name="routeVertices"> Список ID вершин маршрута </param>
    /// <returns> Список ID рёбер маршрута </returns>
    public List<long> GetRouteEdgeIds(List<long> routeVertices)
    {
        return GetRouteEdges(routeVertices).Select(e => e.Id).ToList();
    }

    /// <summary>
    /// Вычисляет общую длину маршрута
    /// </summary>
    /// <param name="routeVertices"> Список ID вершин маршрута </param>
    /// <returns> Общая длина маршрута </returns>
    public double CalculateRouteLength(List<long> routeVertices)
    {
        return GetRouteEdges(routeVertices).Sum(e => e.Length);
    }

    /// <summary>
    /// Получает информацию о графе
    /// </summary>
    /// <returns> Информация о количестве вершин и рёбер в графе </returns>
    public string GetInfo()
    {
        return $"Вершин: {_vertices.Count}, Рёбер: {_edgesDictionary.Count / 2}";
    }

    /// <summary>
    /// Инициализация графа данными
    /// </summary>
    /// <param name="vertices"> Список вершин графа </param>
    /// <param name="edges"> Список рёбер графа </param>
    public void Initialize(List<GraphVertex> vertices, List<GraphEdge> edges)
    {
        _vertices = vertices.ToDictionary(v => v.Id);
        _connections = new Dictionary<long, List<GraphConnection>>();
        _edgesDictionary = new Dictionary<EdgeKey, GraphEdge>();

        foreach (var edge in edges)
        {
            if (!_vertices.ContainsKey(edge.StartVertexId) || !_vertices.ContainsKey(edge.EndVertexId))
                continue;

            AddConnection(edge.StartVertexId, edge.EndVertexId, edge.Length);
            AddConnection(edge.EndVertexId, edge.StartVertexId, edge.Length);

            var edgeKey = new EdgeKey(edge.StartVertexId, edge.EndVertexId);
            var reverseEdgeKey = new EdgeKey(edge.EndVertexId, edge.StartVertexId);

            _edgesDictionary[edgeKey] = edge;
            _edgesDictionary[reverseEdgeKey] = edge;
        }
    }

    /// <summary>
    /// Проверяет существование вершин в графе
    /// </summary>
    /// <param name="vertexId"> ID вершины для проверки </param>
    /// <returns> True если вершина существует, иначе False </returns>
    public bool VertexExists(long vertexId)
    {
        return _vertices.ContainsKey(vertexId);
    }

    /// <summary>
    /// Добавляет соединение между вершинами
    /// </summary>
    /// <param name="fromId"> ID исходной вершины </param>
    /// <param name="toId"> ID целевой вершины </param>
    /// <param name="weight"> Вес соединения </param>
    private void AddConnection(long fromId, long toId, double weight)
    {
        if (!_connections.ContainsKey(fromId))
            _connections[fromId] = new List<GraphConnection>();

        _connections[fromId].Add(new GraphConnection(toId, weight));
    }

    /// <summary>
    /// Проверяет существование вершин в графе
    /// </summary>
    /// <param name="startId"> ID начальной вершины </param>
    /// <param name="endId"> ID конечной вершины </param>
    private void ValidateVerticesExist(long startId, long endId)
    {
        if (!VertexExists(startId))
            throw new ArgumentException($"Вершина {startId} не найдена");
        if (!VertexExists(endId))
            throw new ArgumentException($"Вершина {endId} не найдена");
        if (startId == endId)
            throw new ArgumentException("Начальная и конечная вершины совпадают");
    }

    /// <summary>
    /// Находит ближайшую непосещённую вершину
    /// </summary>
    /// <param name="unvisited"> Множество непосещённых вершин </param>
    /// <param name="distances"> Словарь расстояний до вершин </param>
    /// <returns> ID ближайшей вершины или -1 если не найдено </returns>
    private long GetClosestUnvisitedVertex(HashSet<long> unvisited, Dictionary<long, double> distances)
    {
        long closestId = -1;
        var minDistance = double.MaxValue;

        foreach (var vertexId in unvisited)
        {
            if (distances[vertexId] < minDistance)
            {
                minDistance = distances[vertexId];
                closestId = vertexId;
            }
        }

        return closestId;
    }

    /// <summary>
    /// Обновляет расстояния до соседних вершин
    /// </summary>
    /// <param name="currentId"> ID текущей вершины </param>
    /// <param name="distances"> Словарь расстояний до вершин </param>
    /// <param name="previous"> Словарь предыдущих вершин </param>
    /// <param name="unvisited"> Множество непосещённых вершин </param>
    private void UpdateNeighborDistances(long currentId, Dictionary<long, double> distances,
        Dictionary<long, long> previous, HashSet<long> unvisited)
    {
        if (!_connections.ContainsKey(currentId))
            return;

        foreach (var connection in _connections[currentId])
        {
            if (!unvisited.Contains(connection.NeighborId))
                continue;

            var newDistance = distances[currentId] + connection.Weight;
            if (newDistance < distances[connection.NeighborId])
            {
                distances[connection.NeighborId] = newDistance;
                previous[connection.NeighborId] = currentId;
            }
        }
    }

    /// <summary>
    /// Строит путь на основе словаря предыдущих вершин
    /// </summary>
    /// <param name="previous"> Словарь предыдущих вершин </param>
    /// <param name="endId"> ID конечной вершины </param>
    /// <returns> Список ID вершин пути </returns>
    private List<long> BuildPath(Dictionary<long, long> previous, long endId)
    {
        var path = new List<long>();
        var current = endId;

        while (current != -1 && previous.ContainsKey(current))
        {
            path.Insert(0, current);
            current = previous[current];
        }

        return path.Count > 0 && path[0] != -1 ? path : new List<long>();
    }
}