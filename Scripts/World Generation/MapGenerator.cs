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
    public RuleTile dirtTile;
    public RuleTile mountainTile;
    public RuleTile mountain2Tile;
    public RuleTile mountainSnowyTile;
    public RuleTile OaktreeTile, PinetreeTile, MangroovetreeTile, CactusTreeTile;
    //deep water tilemap
    public Tilemap tilemapLayer;
    //water tilemap
    public Tilemap tilemapLayer0;
    //Sand, Grass, Mountain tilemap
    public Tilemap tilemapLayer1;
    //Mountain2, SnowyMountain tilemap
    public Tilemap tilemapLayer2;
    private Vector3Int tilePosition;
    private Vector2 treePosition;

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

    //biome Map
    string[,] biome;

    //tile height
    [Range(0, 1)]
    public float waterThreshold, sandThreshold, grassThreshold, dirThreshold, grass2Threshold, mountainThreshold, mountainSnowyThreshold, highBiome, lowBiome;

    //tile temperature
    [Range(0, 1)]
    public float temperatureHotThreshold, temperatureTemperateThreshold, temperatureColdThreshold;

    //vegetation height
    [Range(0, 1)]
    public float treeThreshold, shrubThreshold;

    //Vegetation Boolean Map
    bool[,] canPlant;
    bool[,] vegetationBoolMap;

    //Noise.cs, TemperatureNoise.cs, vegetationNoise.cs
    public int mapWidth;
    public int mapHeight;
    public int seed, temperatureSeed, vegetationSeed, simplexSeed;
    public float noiseScale, vegetationNoiseScale;
    public int octaves, vegetationOctaves;
    public float persistance, vegetationPersistance;
    public float lacunarity, vegetationLacunarity;

    public bool autoUpdate;
    private void Start()
    {
        if (temperatureSeed == 0)
        {
            temperatureSeed = seed / 2;
        }
        if (vegetationSeed == 0)
        {
            vegetationSeed = -seed / 2;
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
        canPlant = new bool[mapWidth + 1, mapHeight + 1];
        vegetationBoolMap = new bool[mapWidth + 1, mapHeight + 1];
        biome = new string[mapWidth, mapHeight];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = heightNoiseMap[x, y];
                float currentTemperature = temperatureNoiseMap[x, y];
                float currentVegetation = Mathf.Lerp(vegetationNoiseMap[x, y], simplexNoiseMap[x, y], 0.5f);
                tilePosition = new Vector3Int(x, y, 0);
                treePosition = new Vector2(x, y);

                if ((x == 0 || y == 0) || (x == mapWidth - 1 || y == mapHeight - 1))
                {
                    currentVegetation = 0;
                }

                //Set boolean Map
                if (currentVegetation > treeThreshold)
                {
                    if (//check x neighbours
                        /*
                        x | x | x
                        x | 0 | x
                        x | x | x
                        */
                        canPlant[x - 1, y - 1] != true &&
                        canPlant[x, y - 1] != true &&
                        canPlant[x + 1, y - 1] != true &&
                        canPlant[x - 1, y] != true &&
                        canPlant[x + 1, y] != true &&
                        canPlant[x - 1, y + 1] != true &&
                        canPlant[x, y + 1] != true &&
                        canPlant[x + 1, y + 1] != true)
                    {
                        //define x neighbours & Enable 0
                        /*
                        x | x | x
                        x | 0 | x
                        x | x | x
                        */
                        canPlant[x, y] = true;

                        canPlant[x - 1, y - 1] = false;
                        canPlant[x, y - 1] = false;
                        canPlant[x + 1, y - 1] = false;
                        canPlant[x - 1, y] = false;
                        canPlant[x + 1, y] = false;
                        canPlant[x - 1, y + 1] = false;
                        canPlant[x, y + 1] = false;
                        canPlant[x + 1, y + 1] = false;

                        //define map

                        vegetationBoolMap[x, y] = false;
                        vegetationBoolMap[x - 1, y - 1] = false;
                        vegetationBoolMap[x, y - 1] = false;
                        vegetationBoolMap[x + 1, y - 1] = false;
                        vegetationBoolMap[x - 1, y] = false;
                        vegetationBoolMap[x + 1, y] = false;
                        vegetationBoolMap[x - 1, y + 1] = false;
                        vegetationBoolMap[x, y + 1] = false;
                        vegetationBoolMap[x + 1, y + 1] = false;
                    }
                    else
                    {
                        canPlant[x, y] = false;
                    }
                }
                else
                {
                    canPlant[x, y] = false;
                    vegetationBoolMap[x, y] = true;
                }

                //biome setter
                if (currentHeight <= lowBiome)
                {
                    if (currentTemperature <= temperatureColdThreshold)
                    {
                        biome[x, y] = tundraBiome;
                    }
                    else if (currentTemperature <= temperatureTemperateThreshold)
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
                    else if (currentTemperature <= temperatureTemperateThreshold)
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
                    if (temperatureNoiseMap[x, y] <= temperatureTemperateThreshold)
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
                if (tilemapLayer1.GetTile(tilePosition) == null)
                {
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
                        else if (canPlant[x, y] && currentHeight > grassThreshold && currentHeight <= mountainThreshold)
                        {
                            InstantiateTree(CactusTreeTile);
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
                        else if (canPlant[x, y] && currentHeight > grassThreshold && currentHeight <= mountainThreshold)
                        {
                            InstantiateTree(CactusTreeTile);
                        }
                    }
                    else if (biome[x, y] == mangrooveBiome)
                    {
                        tilemapLayer1.SetTile(tilePosition, sandTile);
                        if (currentHeight < sandThreshold)
                        {
                            tilemapLayer0.SetTile(tilePosition, waterTile);
                            if (canPlant[x, y])
                            {
                                InstantiateTree(MangroovetreeTile);

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
                        else if (currentHeight > grassThreshold && currentHeight <= dirThreshold)
                        {
                            tilemapLayer1.SetTile(tilePosition, grassTile);
                        }
                        else if (currentHeight > dirThreshold && currentHeight <= grass2Threshold)
                        {
                            if (biome[x, y] != montaneGrasslandsBiome ||
                                biome[x, y] != coniferousForestBiome ||
                                biome[x, y] != mediterraneanBiome)
                            {
                                tilemapLayer1.SetTile(tilePosition, dirtTile);
                            }
                            else
                            {
                                tilemapLayer1.SetTile(tilePosition, grassTile);
                            }
                        }
                        else if (currentHeight > grass2Threshold && currentHeight <= mountainThreshold)
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
                            if (canPlant[x, y] && currentHeight > grassThreshold && currentHeight <= mountainThreshold)
                            {
                                InstantiateTree(PinetreeTile);
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
                            if (canPlant[x, y] && currentHeight > grassThreshold && currentHeight <= mountainThreshold)
                            {
                                InstantiateTree(OaktreeTile);
                            }
                        }
                    }
                }
            }
        }
        //re-define boolean map
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                TileBase currentTile0, currentTile1;
                currentTile0 = tilemapLayer0.GetTile(new Vector3Int(x, y, 0));
                currentTile1 = tilemapLayer1.GetTile(new Vector3Int(x, y, 0));
                if (!(currentTile1 == grassTile || (currentTile0 == waterTile && currentTile1 == null)))
                {
                    vegetationBoolMap[x, y] = false;
                }
            }
        }
    }
    public bool getVegetationBoolMap(int x, int y)
    {
        return vegetationBoolMap[x, y];
    }

    private void InstantiateTree(RuleTile currentTreeTileType)
    {
        tilemapLayer2.SetTile(tilePosition, currentTreeTileType);
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