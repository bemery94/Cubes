using System.Collections.Generic;
using UnityEngine;

public class MeshOps
{
    public static Mesh MergeMeshes(IEnumerable<Mesh> meshes)
    {
        Mesh merged = new Mesh();

        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();

        int vertOffset = 0;

        foreach (var mesh in meshes)
        {
            verts.AddRange(mesh.vertices);

            foreach (var index in mesh.triangles)
                tris.Add(index + vertOffset);

            vertOffset += mesh.vertexCount;
        }

        merged.SetVertices(verts);
        merged.SetTriangles(tris, 0);
        merged.RecalculateNormals();

        return merged;
    }

    public static Mesh ToFlatShaded(Mesh original)
    {
        Vector3[] oldVerts = original.vertices;
        int[] oldTris = original.triangles;

        Vector3[] newVerts = new Vector3[oldTris.Length];
        int[] newTris = new int[oldTris.Length];

        for (int i = 0; i < oldTris.Length; i++)
        {
            newVerts[i] = oldVerts[oldTris[i]];
            newTris[i] = i;
        }

        Mesh flatMesh = new Mesh();
        flatMesh.vertices = newVerts;
        flatMesh.triangles = newTris;
        flatMesh.RecalculateNormals();
        return flatMesh;
    }
}