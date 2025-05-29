using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class MazeGenerator {
    private static Vector2Int SamplePosition(System.Random rng, Vector2Int size, int margin = 0)
    {
        return new Vector2Int(rng.Next(margin, size.x - margin), rng.Next(margin, size.y - margin));
    }
    
    private static T SampleArray<T>(System.Random rng, T[] array)
    {
        var index = rng.Next(0, array.Length);
        return array[index];
    }
    
    public static GridTopology GenerateMazeDFS(System.Random rng, Vector2Int size) 
    {
        var mazeTopology = new GridTopology(size, false);
        
        var visited = new bool[size.x, size.y];
        var stack = new Stack<Vector2Int>();
        
        void Visit(Vector2Int p)
        {
            visited[p.x, p.y] = true;
            stack.Push(p);
        }
        
        var startPosition = SamplePosition(rng, size);
        Visit(startPosition);
        
        while (stack.Count > 0)
        {
            var current = stack.First();
            
            var neighbors = mazeTopology.IterNeighbors(current)
                .Where(((Direction d, Vector2Int p) n) => !visited[n.p.x, n.p.y])
                .ToArray();
            
            
            if (neighbors.Length == 0)
            {
                stack.Pop();
                continue;
            }
            
            var (direction, neighbor) = SampleArray(rng, neighbors);
            mazeTopology.SetEdge(current, direction, true);
            Visit(neighbor);
        }
        
        // todo: entries
        
        return mazeTopology;
    }
}
