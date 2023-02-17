using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplexNoise
{
    public static int[,] GenerateSimplexNoiseMap(int mapWidth, int mapHeight, int seed)
    {
        int[,] simplexNoiseMap = new int[mapWidth, mapHeight];
        System.Random prng = new System.Random(seed);

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                int noiseValue = prng.Next(0, 2);

                simplexNoiseMap[x, y] = noiseValue;
            }
        }
        return simplexNoiseMap;
    }
}