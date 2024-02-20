using InfiniteWorldLibrary.WorldGenerator.FeatureGenerator.Caves;
using Terraria;
using Terraria.ID;

namespace InfiniteWorldLibrary.WorldGenerator.ChunkGenerator
{
    class UnderworldChunkGenerator : ChunkGenerator
    {
        public UnderworldChunkGenerator(int seed = 1337) : base(1337)
        {
            FeatureGenerators.Add(new BasicPerlinCaveWorldFeatureGenerator(seed));
            FeatureGenerators.Add(new BasicCellularCaveWorldFeatureGenerator(seed));
        }

        public override Tile[] SetupTerrain(int x, int y)
        {
            return GetNewTileArray(TileID.Ash);
        }
    }
}
