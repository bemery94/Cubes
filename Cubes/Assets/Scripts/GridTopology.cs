using System.Collections.Generic;
using UnityEngine;

public enum Direction : int
{
    Left = 0,
    Right = 1,
    Down = 2,
    Up = 3
}


public class GridTopology
{
    public static readonly Dictionary<Direction, Vector2Int> DirectionVectors = new()
    {
        { Direction.Left, new Vector2Int(-1, 0) },
        { Direction.Right, new Vector2Int(1, 0) },
        { Direction.Down, new Vector2Int(0, -1) },
        { Direction.Up, new Vector2Int(0, 1) },
    };

    private bool[,,] edges_;

    public GridTopology(Vector2Int size, bool fillValue)
    {
        edges_ = new bool[size.x, size.y, 2];
        ArrayUtils.FillConstant(edges_, fillValue);
    }

    public GridTopology(bool[,,] edges)
    {
        edges_ = edges;
    }

    public Vector2Int Size()
    {
        return new Vector2Int(edges_.GetLength(0), edges_.GetLength(1));
    }

    public IEnumerable<(Direction, Vector2Int)> IterNeighbors(Vector2Int position)
    {
        foreach (var direction in DirectionVectors)
        {
            var neighbor = position + direction.Value;
            if (ArrayUtils.SizeContains(Size(), neighbor))
            {
                yield return (direction.Key, neighbor);
            }
        }
    }

    private static (int, int, int) MakeEdgeIndex(Vector2Int position, Direction direction)
    {
        return (
            position.x + ((direction == Direction.Left) ? -1 : 0),
            position.y + ((direction == Direction.Down) ? -1 : 0),
            (int)direction / 2
        );
    }

    public bool GetEdge(Vector2Int position, Direction direction)
    {
        var (i, j, k) = MakeEdgeIndex(position, direction);
        return edges_[i, j, k];
    }

    public void SetEdge(Vector2Int position, Direction direction, bool value)
    {
        var (i, j, k) = MakeEdgeIndex(position, direction);
        edges_[i, j, k] = value;
    }

    public GridTopology ToWallTopology()
    {
        var wallSize = Size() + new Vector2Int(1, 1);
        var wallTopology = new GridTopology(wallSize, true);

        for (int x = 1; x < wallSize.x; x++)
        {
            for (int y = 1; y < wallSize.y; y++)
            {
                var p = new Vector2Int(x, y);
                wallTopology.SetEdge(p, Direction.Left, !GetEdge(new Vector2Int(x - 1, y - 1), Direction.Up));
                wallTopology.SetEdge(p, Direction.Down, !GetEdge(new Vector2Int(x - 1, y - 1), Direction.Right));
            }
        }

        return wallTopology;
    }
}