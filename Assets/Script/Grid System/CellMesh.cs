using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CellMesh : MonoBehaviour
{
    Mesh cellMesh;
    List<Vector3> vertices;
    List<int> triangles;
    List<Color> colors;

    MeshCollider meshCollider;

    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = cellMesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        cellMesh.name = "Cell Mesh";
        vertices = new List<Vector3>();
        //materials = new List<Material>();
        colors = new List<Color>();
        triangles = new List<int>();
    }

    public void Triangulate(Cell[] cells)
    {
        cellMesh.Clear();
        vertices.Clear();
        colors.Clear();
        triangles.Clear();
        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }
        cellMesh.vertices = vertices.ToArray();
        cellMesh.colors = colors.ToArray();
        cellMesh.triangles = triangles.ToArray();
        cellMesh.RecalculateNormals();
        meshCollider.sharedMesh = cellMesh;
    }

    void Triangulate(Cell cell)
    {
        Debug.Log(cell);
        Vector3 center = cell.transform.localPosition;
        for (int i = 0; i < 6; i++)
        {
            AddTriangle(
                center,
                center + CellMetrics.corners[i],
                center + CellMetrics.corners[i + 1]
                );
            AddTriangleMaterial(cell.color);
        }
    }

    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }

    
    void AddTriangleMaterial(Color color)
    {
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
    }
    
}
