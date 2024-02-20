using InfiniteWorldLibrary.World;
using Terraria;

namespace InfiniteWorldLibrary.WorldGenerator.FeatureGenerator
{
    public abstract class WorldFeatureGenerator
    {
        public static readonly World.World World = StaticInstance.WorldInstance;

        public abstract bool Apply(Tile[] tileArray, int x, int y);

    }
}
