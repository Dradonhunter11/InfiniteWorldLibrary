using InfiniteWorldLibrary.Utils.Math;
using InfiniteWorldLibrary.World.Region;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfiniteWorldLibrary.World;
using log4net;
using Terraria.ID;
using Terraria;

namespace InfiniteWorldLibrary.WorldGenerator.ChunkGenerator.Generator
{
    public class ClearEverything : GeneratePass
    {
        public ClearEverything(uint ChunkId) : base(ChunkId)
        {
        }

        public override void Apply(long startingX)
        {
            for (int i = (int)startingX; i < startingX + Chunk.ChunkWidth; i++)
            {
                for (int j = 0; j < Chunk.ChunkHeight; j++)
                {
                    var tile = Main.tile[i, j];
                    tile.ResetToType(0);
                    tile.HasTile = false;
                }
            }
        }
    }

    public class SurfaceGenerator : GeneratePass
    {
        public SurfaceGenerator(uint ChunkId) : base(ChunkId)
        {
        }

        public override void Apply(long startingX)
        {

            float[] frequency = new float[] { 0.0077f, 0.0011f, 0.08f, 0.04f };
            float[] limit = new float[] { 0.07f, 0.5f, 0.02f, 0.001f };
            int[][] displacements = new int[frequency.Length][];

            int[] totalDisplacement = new int[Chunk.ChunkWidth];
            for (int i = 0; i < displacements.Length; i++)
            {
                displacements[i] = GetPerlinDisplacements(256, frequency[i], 400, limit[i], Main.ActiveWorldFileData.Seed, (int)startingX);
            }

            for (int i = 0; i < displacements.Length; i++)
            {
                for (int j = 0; j < Chunk.ChunkWidth; j++)
                {
                    totalDisplacement[j] += displacements[i][j];
                }
            }
            
            for (int x = (int)startingX; x < startingX + Chunk.ChunkWidth; x++)
            {
                for (int y = 0; y < 400; y++)
                {
                    var tile = StaticInstance.WorldInstance[x, y];
                    tile.HasTile = false;
                    tile.WallType = 0;
                    tile.LiquidAmount = 0;
                }
            }
            Main.NewText("Generating surface...");
            for (int x = 0; x < Chunk.ChunkWidth; x++)
            {
                totalDisplacement[x] = (int)(totalDisplacement[x] / displacements.Length + 75);
                Fill(startingX + x, totalDisplacement[x] + 175, 1, 200, 0);
            }
        }

        public static int[] GetPerlinDisplacements(int displacementCount, float frequency, int maxLimit, float multiplier, int seed, int startingPosition = 0)
        {
            FastNoise noise = new FastNoise(seed);
            noise.SetNoiseType(FastNoise.NoiseType.Perlin);
            noise.SetFrequency(frequency);
            noise.SetFractalType(FastNoise.FractalType.Ridged);

            int[] displacements = new int[displacementCount];
            int startPosition = startingPosition;
            for (int x = 0; x < displacementCount; x++)
            {
                float noiseValue = noise.GetNoise(x + startPosition, x + startPosition);

                displacements[x] = (int)Math.Floor(noiseValue * maxLimit * multiplier);
            }


            return displacements;
        }

        public static void Fill(long x, int startingY, int width = 1, int depth = 1, ushort tileType = 0)
        {
            for (int i = startingY; i < startingY + depth || i < Chunk.ChunkHeight; i++)
            {
                if (i < 0 || x < 0 || i >= startingY + depth) return;
                var tile = Main.tile[(int)x, i];
                tile.HasTile = true;
                tile.TileType = tileType;
                tile.BlockType = BlockType.Solid;
                tile.LiquidType = 0;
                tile.LiquidAmount = 0;
            }
        }
    }

    public class UndergroundGenerator : GeneratePass
    {
        public UndergroundGenerator(uint ChunkId) : base(ChunkId)
        {
        }

        public override void Apply(long startingX)
        {
            for (int i = (int)startingX; i < startingX + ChunkMap.ChunkWidth; i++)
            {
                for (int j = 400; j < Main.maxTilesY - 200; j++)
                {
                    var tile = Main.tile[i, j];
                    tile.TileType = TileID.Stone;
                    tile.HasTile = true;
                    tile.BlockType = BlockType.Solid;
                    tile.LiquidType = 0;
                    tile.LiquidAmount = 0;
                    tile.ResetToType(1);
                }
            }
        }
    }

    public class HellGenerator : GeneratePass
    {
        public HellGenerator(uint ChunkId) : base(ChunkId)
        {
        }

        public override void Apply(long startingX)
        {
            for (int i = (int)startingX; i < startingX + ChunkMap.ChunkWidth; i++)
            {
                for (int j = Main.maxTilesY - 200; j < Main.maxTilesY; j++)
                {
                    var tile = Main.tile[i, j];
                    tile.HasTile = true;
                    tile.TileType = TileID.Ash;
                    tile.BlockType = BlockType.Solid;
                    tile.LiquidType = 0;
                    tile.LiquidAmount = 0;
                }
            }
        }
    }

    public class DeepHellGenerator : GeneratePass
    {
        public DeepHellGenerator(uint ChunkId) : base(ChunkId)
        {
        }

        public override void Apply(long startingX)
        {
            for (int i = (int)startingX; i < startingX + ChunkMap.ChunkWidth; i++)
            {
                for (int j = Main.maxTilesY; j < Main.maxTilesY + 200; j++)
                {
                    var tile = Main.tile[i, j];
                    tile.HasTile = true;
                    tile.TileType = TileID.LihzahrdBrick;
                    tile.BlockType = BlockType.Solid;
                    tile.TileColor = 7;
                    tile.LiquidType = 0;
                    tile.LiquidAmount = 0;
                }
            }
        }
    }

    public class FixEverything : GeneratePass
    {
        public FixEverything(uint ChunkId) : base(ChunkId)
        {
        }

        public override void Apply(long startingX)
        {
            for (int i = (int)startingX; i < startingX + ChunkMap.ChunkWidth; i++)
            {
                for (int j = 0; j < Main.maxTilesY + 200; j++)
                {
                    WorldGen.TileFrame(i, j);
                }
            }
        }
    }
}
