using UnityEngine;
using Random = System.Random;

public class Maze : MonoBehaviour
{
    void Start()
    {
        var rng = new Random(42);

        var maze = MazeGenerator.GenerateMazeDFS(rng, new Vector2Int(20, 16));

        Mesh mesh = MazeToMesh.MazeToMeshCubes(maze, 0.5f, 0.1f, 0.2f);

        MeshFilter mf = gameObject.AddComponent<MeshFilter>();
        mf.mesh = mesh;

        var renderer = gameObject.AddComponent<MeshRenderer>();

        Material grayMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        grayMat.color = Color.gray;
        renderer.materials = new Material[] { grayMat };
    }

    void Update()
    {
    }
}