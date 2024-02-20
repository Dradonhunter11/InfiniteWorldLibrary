using Terraria;

namespace InfiniteWorldLibrary.WorldGenerator.ChunkGenerator
{
    class SkyChunkGenerator : ChunkGenerator
    {
        public SkyChunkGenerator(int seed = 1337) : base(1337)
        {
            
        }

        public override Tile[] SetupTerrain(int x, int y)
        {
            return GetNewTileArray();
        }
    }
}
