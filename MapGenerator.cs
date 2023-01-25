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
    public Tilemap tilemapLayer;
    public Tilemap tilemapLayer0;
    public Tilemap tilemapLayer1;
    public Tilemap tilemapLayer2;
    private Vector3Int tilePosition;
    private Vector2 treePosition;

    //trees and srhubs
    public GameObject pineTree, oakTree, mangrooveTree, desertCactus;
    public float tileOffsetX, tileOffsetY;

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

    //vegetation height
    [Range(0, 1)]
    public float treeThreshold, shrubThreshold;

    //Noise.cs, TemperatureNoise.cs, vegetationNoise.cs
    public int mapWidth;
    public int mapHeight;
    public int seed, temperatureSeed, vegetationSeed, simplexSeed;
    public float noiseScale, vegetationNoiseScale;
    public int octaves, vegetationOctaves;
    public float persistance, vegetationPersistance;
    public float lacunarity, vegetationLacunarity;

    public bool autoUpdate, printBiomes;
    private void Start()
    {
        if (temperatureSeed == 0)
        {
            temperatureSeed = seed / 2;
        }
        if (vegetationSeed == 0)
        {
            vegetationSeed = seed / 4;
        }
        if (simplexSeed == 0)
        {
            simplexSeed = -seed;
        }
    }
    public void GenerateMap()
    {
        float[,] heightNoiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity);
        float[,] temperatureNoiseMap = TemperatureNoise.GenerateTemperatureNoiseMap(mapWidth, mapHeight, temperatureSeed, noiseScale, octaves);
        float[,] vegetationNoiseMap = VegetationNoise.GenerateVegetationNoiseMap(mapWidth, mapHeight, seed, vegetationNoiseScale, octaves, vegetationPersistance, vegetationLacunarity);
        int[,] simplexNoiseMap = SimplexNoise.GenerateSimplexNoiseMap(mapWidth, mapHeight, vegetationSeed);
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = heightNoiseMap[x, y];
                float currentTemperature = temperatureNoiseMap[x, y];
                float currentVegetation = Mathf.Lerp(vegetationNoiseMap[x, y], simplexNoiseMap[x, y], 0.5f);
                tilePosition = new Vector3Int(x, y, 0);
                treePosition = new Vector2(x, y);
                //biome setter
                string[,] biome = new string[mapWidth, mapHeight];
                if (currentHeight <= lowBiome)
                {
                    if (currentTemperature <= temperatureColdThreshold)
                    {
                        biome[x, y] = tundraBiome;
                    }
                    else if (currentTemperature <= temperatureWarmThreshold)
                    {
                        biome[x, y] = savannaBiome;
                    }
                    else if (currentTemperature <= temperatureHotThreshold)
                    {
                        biome[x, y] = mediterraneanBiome;
                    }
                    else
                    {
                        biome[x, y] = mangrooveBiome;
                    }
                }
                else if (currentHeight <= highBiome)
                {
                    if (currentTemperature <= temperatureColdThreshold)
                    {
                        biome[x, y] = taigaBiome;
                    }
                    else if (currentTemperature <= temperatureWarmThreshold)
                    {
                        biome[x, y] = coniferousForestBiome;
                    }
                    else if (currentTemperature <= temperatureHotThreshold)
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
                //tile and vegetation setter
                if (biome[x, y] == desertBiome)
                {
                    //tile
                    tilemapLayer1.SetTile(tilePosition, sandTile);
                    if (currentHeight <= waterThreshold)
                    {
                        tilemapLayer0.SetTile(tilePosition, waterTile);
                        biome[x, y] = "Oasis";
                    }
                    //vegetation
                    else if (currentVegetation > treeThreshold && currentHeight > grassThreshold && currentHeight <= mountainThreshold)
                    {
                        //Instantiate(desertCactus, new Vector3(treePosition.x + tileOffsetX, treePosition.y + tileOffsetY, -1), Quaternion.identity);
                    }
                }
                else if (biome[x, y] == savannaBiome)
                {
                    //natural tiles
                    if (currentHeight <= sandThreshold)
                    {
                        tilemapLayer0.SetTile(tilePosition, waterTile);
                    }
                    //tile
                    else if (currentHeight > sandThreshold && currentHeight <= grassThreshold)
                    {
                        tilemapLayer1.SetTile(tilePosition, sandTile);
                    }
                    else if (currentHeight <= mountainThreshold)
                    {
                        tilemapLayer1.SetTile(tilePosition, grassTile);
                    }
                    else if (currentHeight > mountainThreshold && currentHeight <= mountainSnowyThreshold)
                    {
                        tilemapLayer1.SetTile(tilePosition, mountainTile);
                    }
                    else if (currentHeight > mountainSnowyThreshold)
                    {
                        tilemapLayer2.SetTile(tilePosition, mountain2Tile);
                    }
                    //vegetation
                    else if (currentVegetation > treeThreshold && currentHeight > grassThreshold && currentHeight <= mountainThreshold)
                    {
                        //Instantiate(desertCactus, new Vector3(treePosition.x + tileOffsetX, treePosition.y + tileOffsetY, -1), Quaternion.identity);
                    }
                }
                else if (biome[x, y] == mangrooveBiome)
                {
                    tilemapLayer1.SetTile(tilePosition, sandTile);
                    if (currentHeight < sandThreshold)
                    {
                        tilemapLayer0.SetTile(tilePosition, waterTile);
                        if (currentVegetation > treeThreshold)
                        {
                            Instantiate(mangrooveTree, new Vector3(treePosition.x + tileOffsetX, treePosition.y + tileOffsetY, -1), Quaternion.identity);
                        }
                    }
                }
                else
                {
                    //natural tiles
                    if (currentHeight > waterThreshold && currentHeight <= sandThreshold)
                    {
                        tilemapLayer0.SetTile(tilePosition, waterTile);
                        tilemapLayer1.SetTile(tilePosition, sandTile);
                    }
                    else if (currentHeight <= waterThreshold)
                    {
                        tilemapLayer0.SetTile(tilePosition, waterTile);
                        tilemapLayer.SetTile(tilePosition, deepWaterTile);
                    }
                    else if (currentHeight > sandThreshold && currentHeight <= grassThreshold)
                    {
                        tilemapLayer1.SetTile(tilePosition, sandTile);
                    }
                    else if (currentHeight > grassThreshold && currentHeight <= mountainThreshold)
                    {
                        tilemapLayer1.SetTile(tilePosition, grassTile);
                    }
                    else if (currentHeight > mountainThreshold)
                    {
                        tilemapLayer1.SetTile(tilePosition, mountainTile);
                    }
                    if (biome[x, y] == montaneGrasslandsBiome || biome[x, y] == taigaBiome || biome[x, y] == tundraBiome)
                    {
                        //snowy tiles
                        if (currentHeight > mountainSnowyThreshold)
                        {
                            tilemapLayer2.SetTile(tilePosition, mountainSnowyTile);
                        }
                        //vegetation
                        if (currentVegetation > treeThreshold && currentHeight > grassThreshold && currentHeight <= mountainThreshold)
                        {
                            Instantiate(pineTree, new Vector3(treePosition.x + tileOffsetX, treePosition.y + tileOffsetY, -1), Quaternion.identity);
                        }
                    }
                    else
                    {
                        //regular tiles
                        if (currentHeight > mountainSnowyThreshold)
                        {
                            tilemapLayer2.SetTile(tilePosition, mountain2Tile);
                        }
                        //vegetation
                        if (currentVegetation > treeThreshold && currentHeight > grassThreshold && currentHeight <= mountainThreshold)
                        {
                            Instantiate(oakTree, new Vector3(treePosition.x + tileOffsetX, treePosition.y + tileOffsetY, -1), Quaternion.identity);
                        }
                    }
                }
                if (printBiomes)
                {
                    Debug.Log("(" + x + ", " + y + ") " + biome[x, y]);
                }
            }
        }
    }
    public void ClearMap()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                tilePosition = new Vector3Int(x, y, 0);
                tilemapLayer.SetTile(tilePosition, null);
                tilemapLayer0.SetTile(tilePosition, null);
                tilemapLayer1.SetTile(tilePosition, null);
                tilemapLayer2.SetTile(tilePosition, null);
            }
        }
    }
}