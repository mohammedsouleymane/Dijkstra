namespace Dijkstra;
/// <summary>
/// Class <c>Dijstra</c> implements dijkstra's algorithm on two-dimensional array.
/// </summary>
/// <typeparam name="T">Generic type of your matrix.</typeparam>
public class Dijkstra<T>
{
    private readonly SortedSet<(int p, int x, int y)> _costs = new();
    private readonly SortedSet<(int, int)> _visited = new();
    private  T[,] Matrix { get; }
    private Func<(int, int), List<(int x, int y)>> Neighbours { get; }
    private (int, int) Start { get; }
    private (int, int) End { get; }

    /// <param name="matrix">matrix/grid</param>
    /// <param name="start">start position -> matrix coordinate (x,y)</param>
    /// <param name="end">destination -> matrix coordinate (x,y) </param>
    /// <param name="func"> Function to get neighbouring elements based on your logic -> expects (int,int) as param and has to return list of (int,int).</param>
    public Dijkstra(T[,] matrix,(int, int) start, (int, int) end ,Func<(int, int), List<(int x, int y)>> func)
    {
        Matrix = matrix;
        Neighbours = func;
        Start = start;
        End = end;
    }

    /// <summary>
    /// Method <c>ShortestCost</c> calculates path with shortest cost.
    /// </summary>
    public int ShortestCost()
    {
        (int x, int y) node = (Start.Item1, Start.Item2);
        UpdateCosts(node);
        _visited.Add(node);
        while (node != End && _costs.Any())
        {
            node = (_costs.First().x, _costs.First().y);
            _visited.Add(node);
            UpdateCosts(node, _costs.First().p);
            if(node != End)
                _costs.Remove(_costs.First());
        }

        return _costs.Count > 0 ? _costs.First().p : 0;
    }

    /// <summary>
    /// Method <c>UpdateCosts</c> updates costs of returned neighbours.
    /// if T is an int -> Cost of neighbour is costOfCurrentNode + (number in position)
    /// else cost is costOfCurrentNode + 1
    /// </summary>
    private void UpdateCosts((int,int) node, int costOfCurrentNode = 0)
    {
        foreach (var c in Neighbours(node).Where(x => !_visited.Contains(x)))
        {
            _costs.Add(typeof(T).Name.Contains("Int")
                ? (costOfCurrentNode + (int) (object) Matrix[c.x, c.y]!, c.x, c.y)
                : (costOfCurrentNode + 1, c.x, c.y));
        }
    }
}