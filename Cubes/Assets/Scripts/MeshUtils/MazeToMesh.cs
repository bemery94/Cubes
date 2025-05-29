using System.Collections.Generic;
using UnityEngine;

public class MazeToMesh
{
    public static Mesh MazeToMeshCubes(GridTopology mazeTopology, float cellSize, float wallWidth, float wallHeight)
    {
        var wallTopology = mazeTopology.ToWallTopology();

        var meshes = new List<Mesh>();
        
        var nodeScale = new Vector3(wallWidth, wallHeight, wallWidth);
        var edgeScaleX = new Vector3(cellSize - wallWidth, wallHeight, wallWidth);
        var edgeScaleY = new Vector3(wallWidth, wallHeight, cellSize - wallWidth);

        for (int x = 0; x < wallTopology.Size().x; x++)
        {
            for (int y = 0; y < wallTopology.Size().y; y++)
            {
                
                var offset = new Vector3(x * cellSize, 0, y * cellSize);
                var mesh = MeshPrimitives.CreateCube(nodeScale, offset);
                meshes.Add(mesh);
                
                if (x > 0 && wallTopology.GetEdge(new Vector2Int(x, y), Direction.Left))
                {
                    offset = new Vector3((x - 0.5f) * cellSize, 0, y * cellSize);
                    mesh = MeshPrimitives.CreateCube(edgeScaleX, offset);
                    meshes.Add(mesh);
                }
                
                if (y > 0 && wallTopology.GetEdge(new Vector2Int(x, y), Direction.Down))
                {
                    offset = new Vector3(x * cellSize, 0, (y - 0.5f) * cellSize);
                    mesh = MeshPrimitives.CreateCube(edgeScaleY, offset);
                    meshes.Add(mesh);
                }
                
            }
        }
        
        return MeshOps.MergeMeshes(meshes);
    }
}