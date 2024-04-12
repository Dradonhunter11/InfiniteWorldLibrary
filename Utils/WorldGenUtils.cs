using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.WorldBuilding;
using Terraria;

namespace InfiniteWorldLibrary.Utils
{
    internal class WorldGenUtils
    {
        internal static List<(Vector2D, int)> OreList = new List<(Vector2D, int)>();


        public static void OreRunner(int i, int j, int minX, int minY, int maxX, int maxY, double strength, int steps, int type, bool addTile = false, double speedX = 0.0, double speedY = 0.0, bool noYChange = false, bool overRide = true, int ignoreTileType = -1)
        {
            double num = strength;
            double num2 = steps;
            Vector2D vector2D = default(Vector2D);
            vector2D.X = i;
            vector2D.Y = j;
            Vector2D vector2D2 = default(Vector2D);
            vector2D2.X = (double)WorldGen.genRand.Next(-10, 11) * 0.1;
            vector2D2.Y = (double)WorldGen.genRand.Next(-10, 11) * 0.1;
            if (speedX != 0.0 || speedY != 0.0)
            {
                vector2D2.X = speedX;
                vector2D2.Y = speedY;
            }

            bool flag = type == 368;
            bool flag2 = type == 367;
            bool lava = false;

            while (num > 0.0 && num2 > 0.0)
            {
                if (vector2D.Y < 0.0 && num2 > 0.0 && type == 59)
                    num2 = 0.0;

                num = strength * (num2 / (double)steps);
                num2 -= 1.0;
                int num3 = (int)(vector2D.X - num * 0.5);
                int num4 = (int)(vector2D.X + num * 0.5);
                int num5 = (int)(vector2D.Y - num * 0.5);
                int num6 = (int)(vector2D.Y + num * 0.5);
                if (num3 < minX)
                    num3 = minX;

                if (num4 > maxX - 1)
                    num4 = maxX - 1;

                if (num5 < minY)
                    num5 = minY;

                if (num6 > maxY - 1)
                    num6 = maxY - 1;

                for (int k = num3; k < num4; k++)
                {

                    for (int l = num5; l < num6; l++)
                    {
                        if (GenVars.mudWall && (double)l > Main.worldSurface && Main.tile[k, l - 1].WallType != 2 && l < Main.maxTilesY - 210 - WorldGen.genRand.Next(3) && System.Math.Abs(k - vector2D.X) + System.Math.Abs(l - vector2D.Y) < strength * 0.45 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.01))
                        {
                            if (l > GenVars.lavaLine - WorldGen.genRand.Next(0, 4) - 50)
                            {
                                if (Main.tile[k, l - 1].WallType != 64 && Main.tile[k, l + 1].WallType != 64 && Main.tile[k - 1, l].WallType != 64 && Main.tile[k + 1, l].WallType != 64)
                                    WorldGen.PlaceWall(k, l, 15, mute: true);
                            }
                            else if (Main.tile[k, l - 1].WallType != 15 && Main.tile[k, l + 1].WallType != 15 && Main.tile[k - 1, l].WallType != 15 && Main.tile[k + 1, l].WallType != 15)
                            {
                                WorldGen.PlaceWall(k, l, 64, mute: true);
                            }
                        }

                        var tile1 = Main.tile[k, l];
                        if (type < 0)
                        {
                            if (tile1.TileType == 53)
                                continue;

                            if (type == -2 && tile1.HasTile && (l < GenVars.waterLine || l > GenVars.lavaLine))
                            {
                                tile1.LiquidAmount = byte.MaxValue;
                                tile1.LiquidType = 1;
                            }

                            tile1.HasTile = false;
                            continue;
                        }

                        if (flag && System.Math.Abs((double)k - vector2D.X) + System.Math.Abs((double)l - vector2D.Y) < strength * 0.3 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.01))
                            WorldGen.PlaceWall(k, l, 180, mute: true);

                        if (flag2 && System.Math.Abs((double)k - vector2D.X) + System.Math.Abs((double)l - vector2D.Y) < strength * 0.3 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.01))
                            WorldGen.PlaceWall(k, l, 178, mute: true);

                        if (overRide || !tile1.HasTile)
                        {
                            Tile tile = tile1;
                            bool flag3 = false;
                            flag3 = Main.tileStone[type] && tile.TileType != 1;
                            if (!TileID.Sets.CanBeClearedDuringGeneration[tile.TileType])
                                flag3 = true;

                            switch (tile.TileType)
                            {
                                case 53:
                                    if (type == 59 && GenVars.UndergroundDesertLocation.Contains(k, l))
                                        flag3 = true;
                                    if (type == 40)
                                        flag3 = true;
                                    if ((double)l < Main.worldSurface && type != 59)
                                        flag3 = true;
                                    break;
                                case 45:
                                case 147:
                                case 189:
                                case 190:
                                case 196:
                                case 460:
                                    flag3 = true;
                                    break;
                                case 396:
                                case 397:
                                    flag3 = !TileID.Sets.Ore[type];
                                    break;
                                case 1:
                                    if (type == 59 && (double)l < Main.worldSurface + (double)WorldGen.genRand.Next(-50, 50))
                                        flag3 = true;
                                    break;
                                case 367:
                                case 368:
                                    if (type == 59)
                                        flag3 = true;
                                    break;
                            }

                            if (!flag3)
                            {
                                tile.TileType = (ushort)type;
                                // OreList.Add((new Vector2D(k, l), type));
                            }
                        }

                        if (addTile)
                        {
                            tile1.HasTile = true;
                            tile1.LiquidAmount = 0;
                            tile1.LiquidType = 0;
                        }

                        if (noYChange && (double)l < Main.worldSurface && type != 59)
                            tile1.WallType = 2;

                        if (type == 59 && l > GenVars.waterLine && tile1.LiquidAmount > 0)
                        {
                            tile1.LiquidType = 0;
                            tile1.LiquidAmount = 0;
                        }
                    }
                }

                vector2D += vector2D2;
                if ((!WorldGen.drunkWorldGen || WorldGen.genRand.Next(3) != 0) && num > 50.0)
                {
                    vector2D += vector2D2;
                    num2 -= 1.0;
                    vector2D2.Y += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                    vector2D2.X += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                    if (num > 100.0)
                    {
                        vector2D += vector2D2;
                        num2 -= 1.0;
                        vector2D2.Y += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                        vector2D2.X += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                        if (num > 150.0)
                        {
                            vector2D += vector2D2;
                            num2 -= 1.0;
                            vector2D2.Y += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                            vector2D2.X += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                            if (num > 200.0)
                            {
                                vector2D += vector2D2;
                                num2 -= 1.0;
                                vector2D2.Y += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                                vector2D2.X += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                                if (num > 250.0)
                                {
                                    vector2D += vector2D2;
                                    num2 -= 1.0;
                                    vector2D2.Y += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                                    vector2D2.X += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                                    if (num > 300.0)
                                    {
                                        vector2D += vector2D2;
                                        num2 -= 1.0;
                                        vector2D2.Y += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                                        vector2D2.X += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                                        if (num > 400.0)
                                        {
                                            vector2D += vector2D2;
                                            num2 -= 1.0;
                                            vector2D2.Y += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                                            vector2D2.X += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                                            if (num > 500.0)
                                            {
                                                vector2D += vector2D2;
                                                num2 -= 1.0;
                                                vector2D2.Y += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                                                vector2D2.X += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                                                if (num > 600.0)
                                                {
                                                    vector2D += vector2D2;
                                                    num2 -= 1.0;
                                                    vector2D2.Y += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                                                    vector2D2.X += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                                                    if (num > 700.0)
                                                    {
                                                        vector2D += vector2D2;
                                                        num2 -= 1.0;
                                                        vector2D2.Y += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                                                        vector2D2.X += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                                                        if (num > 800.0)
                                                        {
                                                            vector2D += vector2D2;
                                                            num2 -= 1.0;
                                                            vector2D2.Y += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                                                            vector2D2.X += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                                                            if (num > 900.0)
                                                            {
                                                                vector2D += vector2D2;
                                                                num2 -= 1.0;
                                                                vector2D2.Y += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                                                                vector2D2.X += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                vector2D2.X += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                if (vector2D2.X > 1.0)
                    vector2D2.X = 1.0;

                if (vector2D2.X < -1.0)
                    vector2D2.X = -1.0;

                if (!noYChange)
                {
                    vector2D2.Y += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
                    if (vector2D2.Y > 1.0)
                        vector2D2.Y = 1.0;

                    if (vector2D2.Y < -1.0)
                        vector2D2.Y = -1.0;
                }
                else if (type != 59 && num < 3.0)
                {
                    if (vector2D2.Y > 1.0)
                        vector2D2.Y = 1.0;

                    if (vector2D2.Y < -1.0)
                        vector2D2.Y = -1.0;
                }

                if (type == 59 && !noYChange)
                {
                    if (vector2D2.Y > 0.5)
                        vector2D2.Y = 0.5;

                    if (vector2D2.Y < -0.5)
                        vector2D2.Y = -0.5;

                    if (vector2D.Y < Main.rockLayer + 100.0)
                        vector2D2.Y = 1.0;

                    if (vector2D.Y > (double)(Main.maxTilesY - 300))
                        vector2D2.Y = -1.0;
                }
            }
        }

    }
}
