using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellGrid : MonoBehaviour
{
    private int cellCountX = 6;
    private int cellCountZ = 6;
    public int chunkCountX = 4;
    public int chunkCountZ = 3;

    public Cell cellPrefab;
    public GridChunk chunkPrefab;

    public Material defaultMaterial;
    public Color defaultColor;

    GridChunk[] chunks;
    Cell[] cells;

    void Awake()
    {
        //gridCanvas = GetComponentInChildren<Canvas>();
        //hexMesh = GetComponentInChildren<HexMesh>();

        cellCountX = chunkCountX * CellMetrics.chunkSizeX;
        cellCountZ = chunkCountZ * CellMetrics.chunkSizeZ;

        CreateChunks();
        CreateCells();

    }

    void CreateChunks()
    {
        chunks = new GridChunk[chunkCountX * chunkCountZ];

        for (int z = 0, i = 0; z < chunkCountZ; z++)
        {
            for (int x = 0; x < chunkCountX; x++)
            {
                GridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
                chunk.transform.SetParent(transform);
            }
        }
    }

    void CreateCells()
    {
        cells = new Cell[cellCountZ * cellCountX];
        for (int z = 0, i = 0; z < cellCountZ; z++)
        {
            for (int x = 0; x < cellCountX; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    private void CreateCell(int x, int z, int i)
    {
        //Initialize the cell creation at the current location
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (CellMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (CellMetrics.outerRadius * 1.5f);

        //Instantiate a cell in the list we set up beforehand
        Cell cell = cells[i] = Instantiate<Cell>(cellPrefab);
        //cell.transform.SetParent(transform, false);
        //Move and offset the tile based on what number it is
        cell.transform.localPosition = position;
        cell.coordinates = CellCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;

        //Make text labels based on cell position
        //Text label = Instantiate<Text>(cellLabelPrefab);
        //label.rectTransform.SetParent(gridCanvas.transform, false);
        //label.rectTransform.anchoredPosition =
            //new Vector2(position.x, position.z);
        //label.text = cell.coordinates.ToStringOnSeparateLines();

        AddCellToChunk(x, z, cell);
    }

    void Start()
    {
        //hexMesh.Triangulate(cells);
    }

    private void AddCellToChunk(int x, int z, Cell cell)
    {
        int chunkX = x / CellMetrics.chunkSizeX;
        int chunkZ = z / CellMetrics.chunkSizeZ;
        GridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];

        int localX = x - chunkX * CellMetrics.chunkSizeX;
        int localZ = z - chunkZ * CellMetrics.chunkSizeZ;
        chunk.AddCell(localX + localZ * CellMetrics.chunkSizeX, cell);
    }
}
