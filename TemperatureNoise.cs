using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureNoise
{
    public static float[,] GenerateTemperatureNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves)
    {
        float[,] temperatureNoiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random((seed/2)^3);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-131071, 131071);
            float offsetY = prng.Next(-131071, 131071);
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
        {
            scale += -scale + 0.001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int y = 0; y < mapHeight; y++){
            for (int x = 0; x < mapWidth; x++)
            {
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = x / scale + octaveOffsets[i].x;
                    float sampleY = y / scale + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue;
                }
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                temperatureNoiseMap[x, y] = noiseHeight;
            }
        }
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                temperatureNoiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, temperatureNoiseMap[x, y]);
            }
        }
        return temperatureNoiseMap;
    }
}