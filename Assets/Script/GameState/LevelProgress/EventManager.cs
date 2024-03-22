using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class EventManager : MonoBehaviour
{
    [SerializeField] protected GameObject objectiveList;
    [SerializeField] AmbientSoundLibrary ambientSounds;
    public Enemy dyingEnemy;
    [SerializeField] protected Terrain terrain;
    [SerializeField] protected TerrainData terrainData;
    protected TerrainLayer[] layers;
    public enum E_TerrainLayer { POLLUTED, DIRT, GRASS};


    protected void FlipTiles(List<GameObject> tiles)
    {
        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<Cell>().terrainType = Cell.TerrainType.DIRT;
        }
    }

    protected void ChangeTexture(Vector3 worldPos, TerrainLayer toLayer)
    {
        int mapX = (int)(((worldPos.x - terrain.transform.position.x) / terrainData.size.x) * terrainData.alphamapWidth);
        int mapZ = (int)(((worldPos.z - terrain.transform.position.z) / terrainData.size.z) * terrainData.alphamapHeight);

        float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 15, 15);

        for (int z = 0; z < 15; z++)
        {
            for (int x = 0; x < 15; x++)
            {
                // Iterate through the enum and 
                for (int l = 0; l < layers.Length; l++)
                {
                    // set all layers to 0 except the toLayer
                    splatmapData[x, z, l] = layers[l] == toLayer ? 1 : 0;
                }
            }
        }

        terrainData.SetAlphamaps(mapX, mapZ, splatmapData);
        terrain.Flush();
    }

    protected void SetTerrainLayers()
    {
        layers = terrainData.terrainLayers;
    }

    protected void ResetTerrain()
    {
        for(int i = -100; i < 215; i += 8)
        {
            for (int j = -200; j < 180; i += 8)
            {
                int mapX = (int)(((i - terrain.transform.position.x) / terrainData.size.x) * terrainData.alphamapWidth);
                int mapZ = (int)(((j - terrain.transform.position.z) / terrainData.size.z) * terrainData.alphamapHeight);

                float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 15, 15);

                for (int z = 0; z < 15; z++)
                {
                    for (int x = 0; x < 15; x++)
                    {
                        // Iterate through the enum and 
                        for (int l = 0; l < layers.Length; l++)
                        {
                            // set all layers to 0 except the toLayer
                            splatmapData[x, z, l] = layers[l] == layers[0] ? 1 : 0;
                        }
                    }
                }
                terrainData.SetAlphamaps(mapX, mapZ, splatmapData);
                terrain.Flush();
            }
        }
        
    }
}
