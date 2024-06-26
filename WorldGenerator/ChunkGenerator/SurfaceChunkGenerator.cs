﻿using System;
using InfiniteWorldLibrary.Utils.Math;
using InfiniteWorldLibrary.World.Region;
using InfiniteWorldLibrary.WorldGenerator.FeatureGenerator.Caves;
using Terraria;
using Terraria.ID;

namespace InfiniteWorldLibrary.WorldGenerator.ChunkGenerator
{
    class SurfaceChunkGenerator : ChunkGenerator
    { 
        private static FastNoise _noise;

        public SurfaceChunkGenerator(int seed = 1337) : base(1337)
        {
            _noise = new FastNoise(seed);
            FeatureGenerators.Add(new SurfaceCarverWorldFeatureGenerator(seed));
        }

        public static int[] GetPerlinDisplacements(int displacementCount, float frequency, int maxLimit, float multiplier, int seed, int startingPosition = 0)
        {
            FastNoise noise = new FastNoise(Main.ActiveWorldFileData.Seed);
            noise.SetNoiseType(FastNoise.NoiseType.Perlin);
            noise.SetFrequency(frequency);
            noise.SetFractalType(FastNoise.FractalType.Ridged);

            int[] displacements = new int[displacementCount];
            int startPosition = startingPosition * Chunk.ChunkWidth;
            for (int x = 0; x < displacementCount; x++)
            {
                float noiseValue = noise.GetNoise(x + startPosition, x + startPosition);

                displacements[x] = (int)Math.Floor(noiseValue * maxLimit * multiplier);
            }
                

            return displacements;
        }

        public override Tile[] SetupTerrain(int x1, int y1)
        {
            //LogManager.GetLogger("NO AGAIN").Debug(x1 + ", " + y1);

            Tile[] chunkBase = new Tile[Chunk.ChunkWidth * Chunk.ChunkHeight];

            
            float[] frequency = new float[] { 0.0077f, 0.0011f, 0.08f, 0.04f };
            float[] limit = new float[] { 0.07f, 0.5f, 0.02f, 0.001f };
            int[][] displacements = new int[frequency.Length][];

            int[] totalDisplacement = new int[Chunk.ChunkWidth];
            for (int i = 0; i < displacements.Length; i++)
            {
                displacements[i] = GetPerlinDisplacements(200, frequency[i], Chunk.ChunkHeight, limit[i], WorldGen._lastSeed, x1);
            }

            for (int i = 0; i < displacements.Length; i++)
            {
                for (int j = 0; j < Chunk.ChunkWidth; j++)
                {
                    totalDisplacement[j] += displacements[i][j];
                }
            }

            for (int x = 0; x < Chunk.ChunkWidth; x++)
            {
                for (int y = 0; y < Chunk.ChunkHeight; y++)
                {
                    chunkBase[x * y + Chunk.ChunkWidth].HasTile = false;
                }
            }

            for (int x = 0; x < Chunk.ChunkWidth; x++)
            {
                totalDisplacement[x] = (int) (totalDisplacement[x] / displacements.Length + 75);
                Fill(chunkBase, x, totalDisplacement[x], 1, 1, 0);
            }

            return chunkBase;
        }

        public static void Fill(Tile[] tileArray, int x, int startingY, int width = 1, int depth = 1, ushort tile = 0)
        {
            for (int i = startingY; i < startingY + depth || i < Chunk.ChunkHeight; i++)
            {
                if(i < 0 || x < 0 || i >= 200 || x >= Chunk.ChunkWidth) continue;
                tileArray[x * i + Chunk.ChunkWidth].HasTile = true;
                tileArray[x * i + Chunk.ChunkWidth].TileType = TileID.Dirt;
                tileArray[x * i + Chunk.ChunkWidth].BlockType = BlockType.Solid;
                tileArray[x * i * Chunk.ChunkWidth].LiquidType = 0;
                tileArray[x * i * Chunk.ChunkWidth].LiquidAmount = 0;
            }
        }
    }
}
