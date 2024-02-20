using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using InfiniteWorldLibrary.Utils;
using InfiniteWorldLibrary.Utils.Math;
using InfiniteWorldLibrary.World.Region;
using InfiniteWorldLibrary.WorldGenerator.ChunkGenerator;
using log4net;
using log4net.Repository.Hierarchy;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace InfiniteWorldLibrary.World
{
    // TODO : Abstract it for easier support of multiple world type
    /// <summary>
    /// Class containing everything about the world
    /// </summary>
    [Serializable]
    public class World : ISerializable
    {
        private readonly int m_viewRange = 3;
        //private UndergroundChunkGenerator undergroundGenerator;

        /// <summary>
        /// string = GeneratorName
        /// Value = ChunkGenerator
        /// </summary>
        private readonly Dictionary<string, ChunkGenerator> _chunkGenerators;

        /// <summary>
        /// List of Chunks in the World
        /// </summary>
        private readonly ChunkMap _chunks = new ChunkMap();

        private readonly int _worldseed = 1337;
        public int Seed => _worldseed;

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public World()
        {
        }

        public World(int seed)
        {
            _chunks = new ChunkMap();
            this._worldseed = seed;
        }

        /// <summary>
        /// Gets a tile at the world position
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>The tile at the specified position</returns>
        public Tile this[int x, int y]
        {
            get
            {
                //var logger = LogManager.GetLogger("find chunk");

                //logger.Info("X: " + x);
                //logger.Info("Y: " + y);
                //logger.Info("Chunk Width: " + Chunk<Tile>.ChunkWidth);
                //logger.Info("Chunk Height: " + Chunk<Tile>.ChunkHeight);
                //logger.Info("Chunk Count: " + m_chunks.Count);
                return _chunks[x, y];
            }
            set => throw new Exception("why you do this");
        }

        ///// <summary>
        ///// Gets a chunk from a specified tile position
        ///// </summary>
        ///// <param name="x">X position</param>
        ///// <param name="y">Y position</param>
        ///// <returns>The chunk at the specified tile position</returns>
        //public Chunk GetChunkFromTilePos(int x, int y)
        //{
            
        //    return FindChunk(new Vector2(x, y));
        //}

        ///// <summary>
        ///// Gets a chunk from a specified chunk position
        ///// </summary>
        ///// <param name="x">X position</param>
        ///// <param name="y">Y position</param>
        ///// <returns>The chunk at the specified chunk position</returns>
        //public Chunk GetChunkFromChunkPos(int x, int y)
        //{
        //    return FindChunk(x, y);
        //}

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue("list", _chunks);
        }
    }
}
