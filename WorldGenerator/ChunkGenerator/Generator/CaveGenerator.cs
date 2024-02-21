using InfiniteWorldLibrary.Utils.Math;
using InfiniteWorldLibrary.World.Region;
using InfiniteWorldLibrary.WorldGenerator.FeatureGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace InfiniteWorldLibrary.WorldGenerator.ChunkGenerator.Generator
{
    class DirtPerlinPatchGenerator : GeneratePass
    {
        private static FastNoise caveNoise;

        public DirtPerlinPatchGenerator(uint chunkId) : base(chunkId)
        {
            if (caveNoise == null)
            {
                caveNoise = new FastNoise(Main.ActiveWorldFileData.Seed - 10);
                caveNoise.SetFrequency(0.023f);
                caveNoise.SetFractalOctaves(3);
                caveNoise.SetFractalGain(0.5f);
                caveNoise.SetFractalLacunarity(2f);
                caveNoise.SetFractalWeightedStrength(0.0f);
                caveNoise.SetCellularDistanceFunction(FastNoise.CellularDistanceFunction.EuclideanSq);
                caveNoise.SetCellularReturnType(FastNoise.CellularReturnType.Distance2Add);
                caveNoise.SetCellularJitter(1f);
                // caveNoise.SetFractalPingPongStrength(0.9f);

                caveNoise.SetFractalType(FastNoise.FractalType.None);
                caveNoise.SetNoiseType(FastNoise.NoiseType.Cellular);
            }
        }

        public override void Apply(long startingX)
        {
            int startingPositionX = (int)(startingX);
            int startingPositionY = 400;
            for (int i = 0; i < Chunk.ChunkWidth; i++)
            {
                for (int j = 400; j < Main.maxTilesY - 200; j++)
                {
                    if (j < Main.maxTilesY - 600)
                    {
                        caveNoise.SetFractalOctaves(3);
                    }
                    else
                    {
                        caveNoise.SetFractalOctaves(5);
                    }

                    float noiseValue = (float)(caveNoise.GetNoise((startingPositionX + i), j * 1.6f));
                    if (noiseValue == 1 && j < Main.maxTilesY - 600)
                    {
                        var tile = Main.tile[startingPositionX + i, j];
                        tile.TileType = TileID.Dirt;
                        tile.WallType = WallID.Dirt;
                    }
                    else if (noiseValue >= 0.0000000000005f)
                    {
                        var tile = Main.tile[startingPositionX + i, j];
                        tile.TileType = TileID.Dirt;
                        tile.WallType = WallID.Dirt;
                    }
                }
            }
        }
    }

    class BasicPerlinCaveWorldGenPass : GeneratePass
    {
        private static FastNoise caveNoise;

        public BasicPerlinCaveWorldGenPass(uint chunkId) : base(chunkId)
        {
            if (caveNoise == null)
            {
                caveNoise = new FastNoise(Main.ActiveWorldFileData.Seed);
                caveNoise.SetFrequency(0.02f);
                caveNoise.SetFractalOctaves(2);
                caveNoise.SetFractalGain(1.6f);
                caveNoise.SetFractalLacunarity(1.6f);
                caveNoise.SetFractalWeightedStrength(1.3f);
                //caveNoise.SetFractalPingPongStrength(4.9f);

                caveNoise.SetFractalType(FastNoise.FractalType.FBm);
                caveNoise.SetNoiseType(FastNoise.NoiseType.Perlin);
            }
        }

        public override void Apply(long startingX)
        {
            int startingPositionX = (int)(startingX);
            int startingPositionY = 400;
            for (int i = 0; i < Chunk.ChunkWidth; i++)
            {
                for (int j = 400; j < Main.maxTilesY - 200; j++)
                {
                    float noiseValue = (float)(caveNoise.GetNoise((startingPositionX + i), j * 1.6f));
                    if (noiseValue >= 0.0095f)
                    {
                        var tile = Main.tile[startingPositionX + i, j];
                        tile.HasTile = false;
                        if (tile.WallType == 0)
                        {
                            tile.WallType = WallID.Stone;
                        }
                    }
                }
            }
        }
    }
}
