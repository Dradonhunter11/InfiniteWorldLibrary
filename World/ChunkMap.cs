using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using InfiniteWorldLibrary.World.Region;
using log4net;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace InfiniteWorldLibrary.World
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public ref struct TileIdConverter
    {
        [FieldOffset(0)] public Tile Tile;
        [FieldOffset(0)] public uint X;
        [FieldOffset(0)] public byte SubChunkX;
        [FieldOffset(1)] public uint ChunkX;
        [FieldOffset(4)] private byte padding;
        // One unused byte
        [FieldOffset(6)] public ushort Y;

        public TileIdConverter(Tile tile)
        {
            this.Tile = tile;
        }
        
        public TileIdConverter(int x, int y)
        {
            this.padding = 0;
            this.X = (uint)x;
            this.Y = (ushort)y;
        }

        public TileIdConverter(Vector2 vector)
        {
            this.padding = 0;
            this.X = (uint)vector.X;
            this.Y = (ushort)vector.Y;
        }

        public TileIdConverter(Point point)
        {
            this.padding = 0;
            this.X = (uint)point.X;
            this.Y = (ushort)point.Y;
        }

        public TileIdConverter(uint x, ushort y)
        {
            this.padding = 0;
            this.X = x;
            this.Y = y;
        }
    }

    public readonly struct ChunkMap
    {
        public static readonly int ChunkWidth = 256;
        public static readonly int ChunkHeight = ushort.MaxValue + 1;
        private static readonly uint Center = uint.MaxValue / 2;

        public Tile this[int x, int y]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get
            {
                if (x < 0 || y < 0 || y > ChunkHeight)
                {
                    x = 0; y = 0;
                    // throw new ArgumentOutOfRangeException();
                }

                var tileIdConverter = new TileIdConverter((uint)x, (ushort)y);
                ChunkData.EnsureAllocate(tileIdConverter.ChunkX);
                return tileIdConverter.Tile;
            }
            set
            {
                throw new InvalidOperationException("Cannot set Chunk Map tiles. Only used to init null tiles in Vanilla (which don't exist anymore)");
            }
        }

        internal ChunkMap(uint chunkID)
        {
            if (Main.showSplash)
            {
                return;
            }

            ChunkData.EnsureAllocate(chunkID);
        }

        public Tile this[Point pos] => this[pos.X, pos.Y];

        public void ClearEverything() => ChunkData.ClearEverything();

        public T[] GetData<T>() where T : unmanaged, ITileData => ChunkData<T>.data[0];

        
    }

    public static unsafe class TileExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe ref T Get<T>(in Tile tile) where T : unmanaged, ITileData
        {
            TileIdConverter converter = new TileIdConverter(tile); // ldargs0
            ChunkData<T>.EnsureAllocate(converter.ChunkX);
            return ref ChunkData<T>.dataPtr[converter.ChunkX][converter.Y * ChunkMap.ChunkWidth + converter.SubChunkX];
        }
    }
}
