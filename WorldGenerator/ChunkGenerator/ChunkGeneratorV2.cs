using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InfiniteWorldLibrary.World;
using InfiniteWorldLibrary.World.Region;
using InfiniteWorldLibrary.WorldGenerator.ChunkGenerator.Generator;
using log4net;
using Mono.Cecil;
using MonoMod.Cil;
using Steamworks;
using Terraria;
using Terraria.WorldBuilding;

namespace InfiniteWorldLibrary.WorldGenerator.ChunkGenerator
{
    public static class ChunkGeneratorV2
    {
        public static ConcurrentQueue<uint> PendingChunkList = new ConcurrentQueue<uint>();

        private static Task _watcher;
        private static CancellationTokenSource source = new CancellationTokenSource();
        private static CancellationToken token => source.Token;

        public static void Create()
        {
            _watcher = new Task(() =>
            {
                while (true)
                {
                    if (PendingChunkList.Count > 0)
                    {
                        Task.Run(() =>
                        {
                            uint chunkId;
                            if (PendingChunkList.TryDequeue(out chunkId))
                            {
                                LogManager.GetLogger("Generating").Info(chunkId);
                                var generate = GenerateDefaultChunkGen(chunkId);
                                generate.GenerateChunk();
                            }
                        });
                    }
                }
            }, token,TaskCreationOptions.LongRunning);
            _watcher.Start();
        }

        internal static ChunkGen GenerateDefaultChunkGen(uint chunkId)
        {
            var chunkGen = new ChunkGen(chunkId);
            chunkGen.AddPass("Surface", new SurfaceGenerator(chunkId));
            chunkGen.AddPass("Underground", new UndergroundGenerator(chunkId));
            chunkGen.AddPass("Hell", new HellGenerator(chunkId));
            chunkGen.AddPass("Deep hell", new DeepHellGenerator(chunkId));
            return chunkGen;
        }
    }

    public abstract class GeneratePass
    {
        private readonly long _startingX;
        private readonly uint _chunkId;
        protected int Seed => Main.ActiveWorldFileData.Seed;

        public abstract void Apply(long startingX);

        public GeneratePass(uint ChunkId)
        {
            this._startingX = ChunkId * ChunkMap.ChunkWidth;
            this._chunkId = ChunkId;
        }

        public void Generate()
        {
            Apply(_startingX);
        }
    }



    public class ChunkGen
    {
        private static IReadOnlyList<string> passName = new List<string>();
        private Dictionary<string, GeneratePass> genPass = new Dictionary<string, GeneratePass>();
        private readonly uint ChunkID;
        private long startingX;

        public ChunkGen(uint ChunkId)
        {
            this.ChunkID= ChunkId;
            this.startingX = ChunkID * ChunkMap.ChunkWidth;
        }

        public void GenerateChunk()
        {
            foreach (var genPassValue in genPass.Values)
            {
                genPassValue.Apply(startingX);
            }
        }

        public void AddPass(string name, GeneratePass pass)
        {
            if (genPass.ContainsKey(name))
            {
                throw new ArgumentException("Element already exist");
            }

            genPass.Add(name, pass);
        }

        public void ReplacePass(string name, GeneratePass pass)
        {
            if (!genPass.ContainsKey(name))
            {
                throw new KeyNotFoundException();
            }
            genPass[name] = pass;
        }

        public bool PassExist(string name)
        {
            return genPass[name] != null;
        }

        public GeneratePass GetPass(string name)
        {
            if (!genPass.ContainsKey(name))
            {
                throw new KeyNotFoundException();
            }
            return genPass[name];
        }
    }
}
