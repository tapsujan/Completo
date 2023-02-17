using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationNoise
{
    public static float[,] GenerateVegetationNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity)
    {
        float[,] vegetationNoiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
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

        float maxNoiseValue = float.MinValue;
        float minNoiseValue = float.MaxValue;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseValue = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = x / scale * frequency + octaveOffsets[i].x;
                    float sampleY = y / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseValue += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                if (noiseValue > maxNoiseValue)
                {
                    maxNoiseValue = noiseValue;
                }
                else if (noiseValue < minNoiseValue)
                {
                    minNoiseValue = noiseValue;
                }

                vegetationNoiseMap[x, y] = noiseValue;
            }
        }
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                vegetationNoiseMap[x, y] = Mathf.InverseLerp(minNoiseValue, maxNoiseValue, vegetationNoiseMap[x, y]);
            }
        }
        return vegetationNoiseMap;
    }
}