using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    //tilemap
    public RuleTile deepWaterTile;
    public RuleTile waterTile;
    public RuleTile sandTile;
    public RuleTile grassTile;
    public RuleTile mountainTile;
    public RuleTile mountain2Tile;
    public RuleTile mountainSnowyTile;
    public Tilemap tilemapLayer0;
    public Tilemap tilemapLayer1;
    public Tilemap tilemapLayer2;
    private Vector3Int tilePosition;

    //biomes
    string montaneGrasslandsBiome = "Montane Grasslands";
    string taigaBiome = "Taiga";
    string tundraBiome = "Tundra";
    string coniferousForestBiome = "Coniferous Forest";
    string dryForestBiome = "Dry Forest";
    string savannaBiome = "Savanna";
    string desertBiome = "Desert";
    string mediterraneanBiome = "Mediterranean";
    string mangrooveBiome = "Mangroove";

    //tile height
    [Range(0, 1)]
    public float waterThreshold, sandThreshold, grassThreshold, mountainThreshold, mountainSnowyThreshold, highBiome, lowBiome;

    //tile temperature
    [Range(0, 1)]
    public float temperatureHotThreshold, temperatureWarmThreshold, temperatureColdThreshold;

    //Noise.cs, TemperatureNoise.cs
    public int mapWidth;
    public int mapHeight;
    public int seed;
    public float noiseScale;
    public int octaves;
    public float persistance;
    public float lacunarity;

    public bool autoUpdate, printBiomes;

    public void GenerateMap()
    {
        float[,] heightNoiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity);
        float[,] temperatureNoiseMap = TemperatureNoise.GenerateTemperatureNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves);

        for (int y = 0; y < mapHeight; y++)
        {
            for(int x = 0; x < mapWidth; x++)
            {
                tilePosition = new Vector3Int(x, y, 0);
                //biome setter
                string[,] biome = new string[mapWidth, mapHeight];
                if (heightNoiseMap[x, y] <= lowBiome)
                {
                    if (temperatureNoiseMap[x, y] <= temperatureColdThreshold)
                    {
                        biome[x, y] = tundraBiome;
                    }
                    else if (temperatureNoiseMap[x, y] <= temperatureWarmThreshold)
                    {
                        biome[x, y] = savannaBiome;
                    }
                    else if (temperatureNoiseMap[x, y] <= temperatureHotThreshold)
                    {
                        biome[x, y] = mediterraneanBiome;
                    }
                    else
                    {
                        biome[x, y] = mangrooveBiome;
                    }
                }
                else if (heightNoiseMap[x, y] <= highBiome)
                {
                    if (temperatureNoiseMap[x, y] <= temperatureColdThreshold)
                    {
                        biome[x, y] = taigaBiome;
                    }
                    else if (temperatureNoiseMap[x, y] <= temperatureWarmThreshold)
                    {
                        biome[x, y] = coniferousForestBiome;
                    }
                    else if (temperatureNoiseMap[x, y] <= temperatureHotThreshold)
                    {
                        biome[x, y] = dryForestBiome;
                    }
                    else
                    {
                        biome[x, y] = mediterraneanBiome;
                    }
                }
                else
                {
                    if (temperatureNoiseMap[x, y] <= temperatureWarmThreshold)
                    {
                        biome[x, y] = montaneGrasslandsBiome;
                    }
                    else if (temperatureNoiseMap[x, y] <= temperatureHotThreshold)
                    {
                        biome[x, y] = coniferousForestBiome;
                    }
                    else
                    {
                        biome[x, y] = desertBiome;
                    }
                }
                //tile resetter
                tilemapLayer0.SetTile(tilePosition, null);
                tilemapLayer2.SetTile(tilePosition, null);
                //tile setter
                if (heightNoiseMap[x, y] <= waterThreshold)
                {
                    tilemapLayer1.SetTile(tilePosition, waterTile);
                    if (biome[x, y] != desertBiome)
                    {
                        tilemapLayer0.SetTile(tilePosition, deepWaterTile);
                    }
                    else
                    {
                        biome[x, y] = "Oasis";
                    }
                }
                else if (heightNoiseMap[x, y] > waterThreshold && heightNoiseMap[x, y] <= sandThreshold)
                {
                    if (biome[x, y] != desertBiome)
                    {
                        tilemapLayer1.SetTile(tilePosition, waterTile);
                    }
                    else
                    {
                        tilemapLayer1.SetTile(tilePosition, sandTile);
                    }
                }
                else if (heightNoiseMap[x, y] > sandThreshold && heightNoiseMap[x, y] <= grassThreshold)
                {
                    tilemapLayer1.SetTile(tilePosition, sandTile);
                }
                else if (heightNoiseMap[x, y] > grassThreshold && heightNoiseMap[x, y] <= mountainThreshold)
                {
                    if (biome[x, y] != desertBiome)
                    {
                        tilemapLayer1.SetTile(tilePosition, grassTile);
                    }
                    else
                    {
                        tilemapLayer1.SetTile(tilePosition, sandTile);
                    }
                }
                else if (heightNoiseMap[x, y] > mountainThreshold && heightNoiseMap[x, y] <= mountainSnowyThreshold)
                {
                    if (biome[x, y] != desertBiome)
                    {
                        tilemapLayer1.SetTile(tilePosition, mountainTile);
                    }
                    else
                    {
                        tilemapLayer1.SetTile(tilePosition, sandTile);
                    }
                }
                else
                {
                    tilemapLayer1.SetTile(tilePosition, mountainTile);
                    if (biome[x, y] == montaneGrasslandsBiome || biome[x, y] == taigaBiome)
                    {
                        tilemapLayer2.SetTile(tilePosition, mountainSnowyTile);
                    }
                    else if (biome[x, y] != desertBiome)
                    {
                        tilemapLayer2.SetTile(tilePosition, mountain2Tile);
                    }
                }
                if (printBiomes)
                {
                    Debug.Log("(" + x + ", " + y + ") " + biome[x, y]);
                }
            }
        }
    }
    /*
    public string[,] biomeGenerator(int x, int y, float heightNoiseMap, float temperatureNoiseMap)
    {
        string[,] biome = new string[mapWidth, mapHeight];
        if (heightNoiseMap <= lowBiome)
        {
            if (temperatureNoiseMap <= temperatureColdThreshold)
            {
                biome[x, y] = tundraBiome;
            }
            else if (temperatureNoiseMap <= temperatureWarmThreshold)
            {
                biome[x, y] = savannaBiome;
            }
            else if (temperatureNoiseMap <= temperatureHotThreshold)
            {
                biome[x, y] = mediterraneanBiome;
            }
            else
            {
                biome[x, y] = mangrooveBiome;
            }
        }
        else if (heightNoiseMap <= highBiome)
        {
            if (temperatureNoiseMap <= temperatureColdThreshold)
            {
                biome[x, y] = taigaBiome;
            }
            else if (temperatureNoiseMap <= temperatureWarmThreshold)
            {
                biome[x, y] = coniferousForestBiome;
            }
            else if (temperatureNoiseMap <= temperatureHotThreshold)
            {
                biome[x, y] = dryForestBiome;
            }
            else
            {
                biome[x, y] = mediterraneanBiome;
            }
        }
        else
        {
            if (temperatureNoiseMap <= temperatureWarmThreshold)
            {
                biome[x, y] = montaneGrasslandsBiome;
            }
            else if (temperatureNoiseMap <= temperatureHotThreshold)
            {
                biome[x, y] = coniferousForestBiome;
            }
            else
            {
                biome[x, y] = desertBiome;
            }
        }
        return biome[x, y];
    }
*/
}