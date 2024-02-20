using InfiniteWorldLibrary.WorldGenerator.FeatureGenerator.Caves;
using InfiniteWorldLibrary.WorldGenerator.FeatureGenerator.WorldGen;
using Terraria;
using Terraria.ID;

namespace InfiniteWorldLibrary.WorldGenerator.ChunkGenerator
{
    class UndergroundChunkGenerator : ChunkGenerator
    {
        public UndergroundChunkGenerator(int seed = 1337) : base(1337)
        {
            FeatureGenerators.Add(new BasicPerlinCaveWorldFeatureGenerator(seed));
            FeatureGenerators.Add(new BasicPerlinCaveWorldFeatureGenerator(seed+1));
            FeatureGenerators.Add(new BasicCellularCaveWorldFeatureGenerator(seed));
            FeatureGenerators.Add(new OreWorldGen(seed));
        }

        public override Tile[] SetupTerrain(int x, int y)
        {
            return GetNewTileArray(TileID.Stone);
        }
    }
}
