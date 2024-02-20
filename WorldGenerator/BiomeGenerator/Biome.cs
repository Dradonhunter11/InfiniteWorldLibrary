using System.Collections.Generic;
using Terraria.ID;

namespace InfiniteWorldLibrary.WorldGenerator.BiomeGenerator
{
    public abstract class Biome
    {
        internal abstract Dictionary<int, float> NpcList { get; set; }

        public int Dirt = TileID.Dirt;
        public int Grass = TileID.Grass;
        public int Stone = TileID.Stone;
        public int Sand = TileID.Sand;
        public int Sandstone = TileID.Sandstone;
        public int Ice = TileID.IceBlock;
        public int Snow = TileID.SnowBlock;

        /// <summary>
        /// Key   : Mob ID
        /// Value : weight
        /// </summary>
        /// <param name="mobs"></param>
        public abstract void SpawnMob(Dictionary<int, float> mobs);
    }
}
