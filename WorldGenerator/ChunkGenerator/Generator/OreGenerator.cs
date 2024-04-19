using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfiniteWorldLibrary.Utils;
using InfiniteWorldLibrary.Utils.Math;
using InfiniteWorldLibrary.World;
using InfiniteWorldLibrary.World.Region;
using Terraria;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace InfiniteWorldLibrary.WorldGenerator.ChunkGenerator.Generator
{
    internal class CopperTinOreGenerator : GeneratePass
    {
        public FastNoise noise;

        public CopperTinOreGenerator(uint ChunkId) : base(ChunkId)
        {
            if (noise == null)
            {
                noise = new FastNoise(Main.ActiveWorldFileData.Seed + 10);
                noise.SetFrequency(0.05f);
                noise.SetFractalOctaves(5);
                noise.SetFractalGain(0.5f);
                noise.SetFractalLacunarity(2f);
                noise.SetFractalWeightedStrength(0.0f);
                noise.SetCellularDistanceFunction(FastNoise.CellularDistanceFunction.EuclideanSq);
                noise.SetCellularReturnType(FastNoise.CellularReturnType.CellValue);
                noise.SetCellularJitter(1f);
                // caveNoise.SetFractalPingPongStrength(0.9f);

                noise.SetFractalType(FastNoise.FractalType.None);
                noise.SetNoiseType(FastNoise.NoiseType.Cellular);
            }
        }

        public override void Apply(long startingX)
        {
            for (int num896 = 0; num896 < (int)((double)(Chunk.ChunkWidth * 800) * 8E-05); num896++)
            {
                var mineralToGen = 7;

                if (WorldGen.genRand.Next(2) == 0)
                    mineralToGen = 7;
                else
                    mineralToGen = 167;

                WorldGenUtils.OreRunner(WorldGen.genRand.Next((int)startingX, (int)(startingX + Chunk.ChunkWidth)), WorldGen.genRand.Next(200, Main.maxTilesY - 200), (int)startingX, 0, (int)(startingX + ChunkMap.ChunkWidth), Main.maxTilesY + 200, WorldGen.genRand.Next(9, 15), WorldGen.genRand.Next(7, 10), mineralToGen);
            }
        }
    }

    internal class IronLeadOreGenerator : GeneratePass
    {
        public FastNoise noise;

        public IronLeadOreGenerator(uint ChunkId) : base(ChunkId)
        {
            if (noise == null)
            {
                noise = new FastNoise(Main.ActiveWorldFileData.Seed + 13);
                noise.SetFrequency(0.05f);
                noise.SetFractalOctaves(5);
                noise.SetFractalGain(0.5f);
                noise.SetFractalLacunarity(2f);
                noise.SetFractalWeightedStrength(0.0f);
                noise.SetCellularDistanceFunction(FastNoise.CellularDistanceFunction.EuclideanSq);
                noise.SetCellularReturnType(FastNoise.CellularReturnType.CellValue);
                noise.SetCellularJitter(1f);
                // caveNoise.SetFractalPingPongStrength(0.9f);

                noise.SetFractalType(FastNoise.FractalType.None);
                noise.SetNoiseType(FastNoise.NoiseType.Cellular);
            }
        }

        public override void Apply(long startingX)
        {
            for (int num896 = 0; num896 < (int)((double)(Chunk.ChunkWidth * Main.maxTilesY) * 8E-05); num896++)
            {
                var mineralToGen = 6;

                if (WorldGen.genRand.Next(2) == 0)
                    mineralToGen = 6;
                else
                    mineralToGen = 167;

                WorldGenUtils.OreRunner(WorldGen.genRand.Next((int)startingX, (int)(startingX + Chunk.ChunkWidth)), WorldGen.genRand.Next(200, Main.maxTilesY - 200), (int)startingX, 0, (int)(startingX + ChunkMap.ChunkWidth), Main.maxTilesY + 200, WorldGen.genRand.Next(4, 9), WorldGen.genRand.Next(4, 8), mineralToGen);
            }
            /*
            int startingPositionX = (int)(startingX);
            for (int i = 0; i < Chunk.ChunkWidth; i++)
            {
                for (int j = 200; j < Main.maxTilesY - 200; j++)
                {
                    float noiseValue = (float)(noise.GetNoise((startingPositionX + i), j * 1.6f));
                    if (j > 200 && j < 400 && noiseValue >= 0.95)
                    {
                        var tile = Main.tile[startingPositionX + i, j];
                        tile.TileType = TileID.Iron;
                    }
                    if (noiseValue >= 0.90 && j > 400 && j < 600)
                    {
                        var tile = Main.tile[startingPositionX + i, j];
                        tile.TileType = TileID.Iron;
                    }
                }
            }*/
        }
    }

    internal class HellstoneOreGenerator : GeneratePass
    {
        public HellstoneOreGenerator(uint ChunkId) : base(ChunkId)
        {
        }

        public override void Apply(long startingX)
        {
            for (int num896 = 0; num896 < (int)((double)(Chunk.ChunkWidth * Main.maxTilesY) * 8E-05); num896++)
            {
                var mineralToGen = TileID.Hellstone;
                WorldGenUtils.OreRunner(WorldGen.genRand.Next((int)startingX, (int)(startingX + Chunk.ChunkWidth)), WorldGen.genRand.Next(Main.maxTilesY - 200, Main.maxTilesY), (int)startingX, 0, (int)(startingX + ChunkMap.ChunkWidth), Main.maxTilesY + 200, WorldGen.genRand.Next(4, 9), WorldGen.genRand.Next(4, 8), mineralToGen);
            }
        }
    }
}
