using System;
using System.Collections;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Threading.Tasks;
using InfiniteWorldLibrary.WorldGenerator.ChunkGenerator;
using log4net;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Tile = Terraria.Tile;

namespace InfiniteWorldLibrary.World.Region
{
    
    public class Chunk
    {
        public static readonly int ChunkWidth = 256;
        public static readonly int ChunkHeight = ushort.MaxValue + 1;

        /// <summary>
        /// If this is set to true, it will be fully loaded in memory
        /// </summary>
        public bool Loaded = false;

        /// <summary>
        /// Indicate that a chunk as been received world gen action in the past, but without being fully generated
        /// This is to prevent infinite world gen loop 
        /// </summary>

        public long chunkSeed;
        public Vector2 Position;

        // private GCHandle handle;

        // Initial tile id
        public int firstTileId;
        protected readonly Tile[] _tiles;

        
        public bool PartiallyGenerated = false;
        private Vector2 vector2;
        private Tile[] tile;

        public Chunk(Vector2 vector2, Tile[] tile)
        {
            this.vector2 = vector2;
            this.tile = tile;
        }

        internal Tile[] GetTileArrayCopy()
        {
            return _tiles;
        }

        /// <summary>
        /// Get the tile at the specified position
        /// </summary>
        /// <param name="x">X position of the tile</param>
        /// <param name="y">Y position of the tile</param>
        /// <returns>The tile at the specified position</returns>
        public Tile this[int x, int y]
        {
            get
            {
                if (y > ChunkHeight)
                {
                    y = ChunkHeight;
                }

                if (x > ChunkWidth)
                {
                    x = ChunkWidth;
                }

                if (y < 0)
                {
                    y = 0;
                }

                if (x < 0)
                {
                    x = 0;
                }

                if (y + x * ChunkHeight >= _tiles.Length)
                {
                    LogManager.GetLogger("Hopes and dreams").Info("Overflow!!! " + (y + x * ChunkHeight));
                }

                return _tiles[y + x * ChunkHeight];
            }
            set => _tiles[y + x * ChunkHeight] = value;
        }
    }

    public static class ChunkData
    {
        internal static Action OnClearEverything;
        internal static Action<uint, bool> OnEnsureAllocate;
        internal static Action<Tile> OnClearSingle;
        internal static Action<Tile, Tile> OnCopySingle;

        public static void EnsureAllocate(uint chunkID, bool queue = true) => OnEnsureAllocate?.Invoke(chunkID, queue);
        public static void ClearEverything() => OnClearEverything?.Invoke();
        public static void ClearSingle(Tile tileID) => OnClearSingle?.Invoke(tileID);
        public static void CopySingle(Tile sourceIndex, ref Tile destinationIndex) => OnCopySingle?.Invoke(sourceIndex, destinationIndex);
    }

    /// <summary>
	/// A chunk of tiles in the world
	/// </summary>
    public static unsafe class ChunkData<T> where T : unmanaged
    {
        private const int ChunkCount = ushort.MaxValue;
        public static T[][] data { get; private set; }
        public static GCHandle[] handle { get; private set; }
        public static T*[] dataPtr { get; private set; }

        static ChunkData()
        {
            data = new T[ChunkCount][];
            handle = new GCHandle[ChunkCount];
            dataPtr = new T*[ChunkCount];

            ChunkData.OnEnsureAllocate += EnsureAllocate;
            ChunkData.OnClearSingle += ClearSingle;
            ChunkData.OnCopySingle += CopySingle;
            ChunkData.OnClearEverything += ClearEverything;

            AssemblyLoadContext.GetLoadContext(typeof(T).Assembly).Unloading += _ => Unload();
        }

        private static void Unload()
        {
            ChunkData.OnEnsureAllocate -= EnsureAllocate;
            ChunkData.OnClearSingle -= ClearSingle;
            ChunkData.OnCopySingle -= CopySingle;
            ChunkData.OnClearEverything -= ClearEverything;
            ClearEverything();
        }

        /// <summary>
        /// Make sure chunk is setup
        /// </summary>
        /// <param name="len"></param>
        public static unsafe void EnsureAllocate(uint chunkID, bool queue = true)
        {
            if (data[chunkID] != null)
            {
                return;
            }

            if (!ChunkGeneratorV2.PendingChunkList.Contains(chunkID) && !ChunkGeneratorV2.generatedChunkList.Contains(chunkID))
            {
                ChunkGeneratorV2.PendingChunkList.Enqueue(chunkID);
            }

            var newData = new T[Chunk.ChunkWidth * Chunk.ChunkHeight];
            var newHandle = GCHandle.Alloc((object)newData, GCHandleType.Pinned);
            var newPtr = (T*)newHandle.AddrOfPinnedObject().ToPointer();
            data[chunkID] = newData;
            handle[chunkID] = newHandle;
            dataPtr[chunkID] = newPtr;
        }

        private static unsafe void ClearSingle(Tile tileID)
        {
            TileIdConverter tileIdConverter = new TileIdConverter(tileID);
            var ptr = dataPtr[tileIdConverter.ChunkX];
            if (ptr == null)
            {
                return;
            }
            
            ptr[tileIdConverter.Y * ChunkMap.ChunkWidth + tileIdConverter.SubChunkX] = default;
        }

        private static unsafe void CopySingle(Tile sourceIndex, Tile destinationIndex)
        {
            TileIdConverter sourceIndexConverter = new TileIdConverter(sourceIndex);

            var sourcePtr = dataPtr[sourceIndexConverter.ChunkX];
            if (sourcePtr == null)
            {
                ClearSingle(destinationIndex);
                return;
            }
            TileIdConverter destinationIndexConverter = new TileIdConverter(destinationIndex);

            EnsureAllocate(destinationIndexConverter.ChunkX);

            var destPtr = dataPtr[destinationIndexConverter.ChunkX];

            int subChunkSourceId = sourceIndexConverter.Y * ChunkMap.ChunkWidth + sourceIndexConverter.SubChunkX;
            int subChunkDestId = destinationIndexConverter.Y * ChunkMap.ChunkWidth + destinationIndexConverter.SubChunkX;

            destPtr[subChunkDestId] = sourcePtr[subChunkSourceId];
        }

        private static void ClearEverything()
        {
            for (int i = 0; i < ChunkCount; i++)
            {
                if (data[i] == null)
                {
                    continue;
                }
                handle[i].Free();
                data[i] = null;
                dataPtr[i] = null;
            }
        }

    }
}
