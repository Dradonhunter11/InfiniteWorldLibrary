using System;
using System.Collections.Generic;
using InfiniteWorldLibrary.World.Region;
using InfiniteWorldLibrary.WorldGenerator.FeatureGenerator;
using Terraria;
using Terraria.ID;

namespace InfiniteWorldLibrary.WorldGenerator.ChunkGenerator
{
    [Obsolete("This is now obselete, refer to v2")]
    public abstract class ChunkGenerator
    {
        public List<WorldFeatureGenerator> FeatureGenerators = new List<WorldFeatureGenerator>();

        public int Seed;

        protected Tile[] GetNewTileArray(int tileType = -1)
        {
            Tile[] newTileArray = new Tile[Chunk.ChunkWidth * Chunk.ChunkHeight];
            for (int i = 0; i < Chunk.ChunkWidth; i++)
            {
                for (int j = 0; j < Chunk.ChunkHeight; j++)
                {
                    newTileArray[i + j * Chunk.ChunkWidth].TileType = (ushort)tileType;
                    newTileArray[i + j * Chunk.ChunkWidth].BlockType = BlockType.Solid;
                    newTileArray[i + j * Chunk.ChunkWidth].HasTile = false;
                    newTileArray[i + j * Chunk.ChunkWidth].LiquidType = 0;
                    newTileArray[i + j * Chunk.ChunkWidth].LiquidAmount = 0;
                    newTileArray[i + j * Chunk.ChunkWidth].TileFrameX = 0;
                    newTileArray[i + j * Chunk.ChunkWidth].TileFrameY = 0;
                }
            }

            return newTileArray;
        }

        public abstract Tile[] SetupTerrain(int x, int y);

        internal Tile[] Generate(int x, int y, bool disableTerrain = false)
        {
            Tile[] generate;
            if (!disableTerrain)
                generate = SetupTerrain(x, y);
            else
                generate = GetNewTileArray();

            foreach (var worldFeatureGenerator in FeatureGenerators)
            {
                worldFeatureGenerator.Apply(generate, x, y);
            }

            return generate;
        }

        public ChunkGenerator(int seed = 1337)
        {
            this.Seed = seed;
        }
    }
}
