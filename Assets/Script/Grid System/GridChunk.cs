using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridChunk : MonoBehaviour
{
    Cell[] cells;

    CellMesh cellMesh;
    Canvas gridCanvas;

    private void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        cellMesh = GetComponentInChildren<CellMesh>();

        cells = new Cell[CellMetrics.chunkSizeX * CellMetrics.chunkSizeZ];
        //Debug.Log(cells.Length);
    }

    // Start is called before the first frame update
    void Start()
    {
        //cellMesh.Triangulate(cells);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddCell(int index, Cell cell)
    {
        cells[index] = cell;
        cell.chunk = this;
        cell.transform.SetParent(transform, false);
    }
}
