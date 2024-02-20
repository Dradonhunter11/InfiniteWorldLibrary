using Microsoft.Xna.Framework;
using Terraria;

namespace InfiniteWorldLibrary.WorldGenerator.FeatureGenerator
{
    abstract class StructureWorldFeatureGenerator : WorldFeatureGenerator
    {
        public abstract bool CanPlace(int x, int y, out Rectangle bound);

        public sealed override bool Apply(Tile[] tileArray, int x, int y)
        {
            Rectangle structureBound;
            if (CanPlace(x, y, out structureBound))
            {
                return true;
            }

            return false;
        }

        public abstract void InternalGenerate(Tile[,] tileArray, int x, int y);
    }
}
