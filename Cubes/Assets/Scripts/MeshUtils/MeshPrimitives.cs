using UnityEngine;
using UnityEngine.Assertions;

public class MeshPrimitives
{
    private static int[] QuadsToTriangles(int[] quads)
    {
        Assert.AreEqual(quads.Length % 4, 0);
        var numQuads = quads.Length / 4;

        int[] tris = new int[numQuads * 6]; // creating 2 triangles per quad -> 2 * 3 = 6

        for (int i = 0; i < numQuads; i++)
        {
            tris[i * 6 + 0] = quads[i * 4 + 0];
            tris[i * 6 + 1] = quads[i * 4 + 1];
            tris[i * 6 + 2] = quads[i * 4 + 2];
            tris[i * 6 + 3] = quads[i * 4 + 0];
            tris[i * 6 + 4] = quads[i * 4 + 2];
            tris[i * 6 + 5] = quads[i * 4 + 3];
        }

        return tris;
    }

    public static Mesh CreateCube(Vector3? scale, Vector3? translation)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices =
        {
            new(-0.5f, -0.5f, -0.5f), // left bottom front
            new(0.5f, -0.5f, -0.5f), // right bottom front
            new(-0.5f, 0.5f, -0.5f), // left top front
            new(0.5f, 0.5f, -0.5f), // right top front
            new(-0.5f, -0.5f, 0.5f), // left bottom back
            new(0.5f, -0.5f, 0.5f), // right bottom back
            new(-0.5f, 0.5f, 0.5f), // left top back
            new(0.5f, 0.5f, 0.5f), // right top back
        };


        if (scale.HasValue)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Scale(scale.Value);
            }
        }

        if (translation.HasValue)
        {
            if (scale.HasValue)
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] += translation.Value;
                }
            }
        }

        int[] quads = new int[]
        {
            0, 2, 3, 1, // front
            2, 6, 7, 3, // top
            5, 7, 6, 4, // back
            0, 1, 5, 4, // bottom
            0, 4, 6, 2, // left
            1, 3, 7, 5 // right
        };

        var triangles = QuadsToTriangles(quads);

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh = MeshOps.ToFlatShaded(mesh);
        mesh.RecalculateNormals();

        return mesh;
    }
}