using InfiniteWorldLibrary.Utils.Math;
using InfiniteWorldLibrary.World.Region;
using Terraria;
using Terraria.ID;

namespace InfiniteWorldLibrary.WorldGenerator.FeatureGenerator.Caves
{
    class SurfaceCarverWorldFeatureGenerator : WorldFeatureGenerator
    {
        private static FastNoise caveNoise;

        public SurfaceCarverWorldFeatureGenerator(int seed = 1337)
        {
            caveNoise = new FastNoise(seed);
            caveNoise.SetFrequency(0.02f);
            caveNoise.SetFractalOctaves(2);
            caveNoise.SetFractalGain(1.6f);
            caveNoise.SetFractalLacunarity(1.6f);
            caveNoise.SetFractalWeightedStrength(1.3f);
            //caveNoise.SetFractalPingPongStrength(4.9f);

            caveNoise.SetFractalType(FastNoise.FractalType.FBm);
            caveNoise.SetNoiseType(FastNoise.NoiseType.Perlin);
        }

        public override bool Apply(Tile[] tileArray, int x, int y)
        {
            int startingPositionX = x * Chunk.ChunkWidth;
            int startingPositionY = y * Chunk.ChunkHeight;
            for (int i = 0; i < Chunk.ChunkWidth; i++)
            {
                for (int j = 0; j < Chunk.ChunkHeight; j++)
                {
                    float noiseValue = (float)(caveNoise.GetNoise((startingPositionX + i), (startingPositionY + j)));
                    if (noiseValue >= 0.1f && tileArray[i * Chunk.ChunkWidth + j].HasTile)
                    {
                        tileArray[i * Chunk.ChunkWidth + j].HasTile = false;
                        tileArray[i * Chunk.ChunkWidth + j].WallType = WallID.Dirt;
                    }
                }
            }
            return true;
        }
    }
}
