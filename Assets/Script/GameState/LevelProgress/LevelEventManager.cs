using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class LevelEventManager : EventManager
{
    [SerializeField] protected GameObjectRuntimeSet playerSet;
    protected CelestialPlayer celestialPlayer;
    protected EarthPlayer earthPlayer;
    [SerializeField] protected GameObject objectiveList;
    [SerializeField] AmbientSoundLibrary ambientSounds;
    public Enemy dyingEnemy;
    [SerializeField] protected Terrain terrain;
    [SerializeField] protected TerrainData terrainData1;
    [SerializeField] protected TerrainData terrainData2;
    protected TerrainLayer[] layers;
    public enum E_TerrainLayer { POLLUTED, DIRT, GRASS};

    protected bool hasFlipped;


    protected void FlipTiles(List<GameObject> tiles)
    {
        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<Cell>().terrainType = Cell.TerrainType.DIRT;
        }
    }

    //Function to call for changing texture over a particular area
    protected void ApplyTextureChangeOverArea(int xStart, int xEnd, int zStart, int zEnd, int steps, int layerToChange, float layerWeight)
    {
        //Iterate across x axis
        for(int i = xStart; i < xEnd; i += steps)
        {
            //Iterate across z axis
            for(int j = zStart; j < zEnd; j += steps)
            {
                Vector3 pos = new Vector3(i, 0, j);
                ChangeTexture(pos, layers[layerToChange], layerWeight);
            }
        }
    }

    protected void ChangeTexture(Vector3 worldPos, TerrainLayer toLayer, float weight)
    {
        int mapX = (int)(((worldPos.x - terrain.transform.position.x) / terrainData2.size.x) * terrainData2.alphamapWidth);
        int mapZ = (int)(((worldPos.z - terrain.transform.position.z) / terrainData2.size.z) * terrainData2.alphamapHeight);

        float[,,] splatmapData = terrainData2.GetAlphamaps(mapX, mapZ, 15, 15);

        for (int z = 0; z < 15; z++)
        {
            for (int x = 0; x < 15; x++)
            {
                // Iterate through the enum and 
                for (int l = 0; l < layers.Length; l++)
                {
                    // set all layers to 0 except the toLayer
                    splatmapData[x, z, l] = layers[l] == toLayer ? weight : 0;
                }
            }
        }

        terrainData2.SetAlphamaps(mapX, mapZ, splatmapData);
        terrain.Flush();
    }

    protected void InitializeTerrain()
    {
        terrainData1 = terrain.terrainData;
        layers = terrainData1.terrainLayers;
        hasFlipped = false;
    }

    protected void DuplicateTerrain()
    {
        terrainData2 = Instantiate(terrainData1);
        //terrainData2 = terrainData1;
        terrain.terrainData = terrainData2;
        
        hasFlipped = true;
    }

    protected void ResetTerrain()
    {
        terrain.terrainData = terrainData1;
        Destroy(terrainData2);
    }
}
