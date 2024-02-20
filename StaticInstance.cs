using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfiniteWorldLibrary.World;

namespace InfiniteWorldLibrary
{
    public static class StaticInstance
    {
        public static World.World WorldInstance = new World.World(1337);
    }
}
