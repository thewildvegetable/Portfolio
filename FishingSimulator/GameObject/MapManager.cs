using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace BoatGame
{
    class MapManager
    {
        //----------------------------------------------------------------------->>FIELDS<<
        private List<List<MapNode>> maps;
        Random rand;

        //----------------------------------------------------------------------->>CONSTRUCTOR<<
        public MapManager()
        {
            maps = new List<List<MapNode>>();
            rand = new Random();
        }

        //----------------------------------------------------------------------->>PROPERTIES<<
        public List<List<MapNode>> Maps
        {
            get { return maps; }
            set { maps = value; }
        }

        //----------------------------------------------------------------------->>METHODS<<
        //adds a node
        //nodes have a number of islands between 0 and 2
        public void AddNode(bool newRow, Texture2D mapImage, List<Texture2D> islandImages, Texture2D fishImage, Texture2D stormImage,
                            Texture2D pirateImage, int screenWidth, int screenHeight, List<InventoryItem> items, Player play)
        {
            MapNode node = new MapNode(rand.Next(0,4),
                                       rand.Next(0,3),
                                       rand.Next(0,2),
                                       rand.Next(0, 3),
                                       mapImage,
                                       islandImages,
                                       fishImage,
                                       stormImage,
                                       pirateImage,
                                       screenWidth,
                                       screenHeight);
            bool addItem = false;
            node.makeIslands(rand, play);
            node.makeFishingSpots(rand);
            node.makeVortexes(rand);
            node.makePirates(rand);
            node.AddItem(items, addItem, rand);
            if (newRow)
            {
                maps.Add(new List<MapNode>());
            }

            maps[Rows() - 1].Add(node);
        }
        #region Returning values
        //Returns the number of nodes in a row
        public int Rows()
        {
            return maps.Count;
        }

        //Returns the number of columns in said row
        public int Columns()
        {
            return maps[Rows() - 1].Count;
        }

        //Returns a list of islands in the specified row/column
        public List<Island> Islands(int row, int column)
        {
            return maps[row][column].Islands;
        }

        //Returns a list of fishing spots in the specified row/column
        public List<FishingSpot> FishingSpots(int row, int column)
        {
            return maps[row][column].FishingSpots;
        }

        //Returns a list of vortexes in the specified row/column
        public List<Storm> Vortexes(int row, int column)
        {
            return maps[row][column].Vortexes;
        }

        //Returns a list of pirates in the specified row/column
        public List<Pirate> Pirates(int row, int column)
        {
            return maps[row][column].Pirates;
        }

        //Returns the number of islands in the specified row/column
        public int IslandNum(int row, int column)
        {
            return maps[row][column].IslandNum;
        }

        #endregion

        public void Remove(int row, int column, ObjectType objType, Object remove)
        {
            if (objType == ObjectType.fishingSpot)
            {
                maps[row][column].RemoveFishingSpot((FishingSpot)remove);
                maps[row][column].FishNum--;
            }
            else if (objType == ObjectType.storm)
            {
                maps[row][column].RemoveVortex((Storm)remove);
                maps[row][column].StormNum--;
            }
            else
                return;
        }
        //Draws the MapNode on the screen
        public void Draw(SpriteBatch spriteBatch, int row, int column, int width, int height)
        {
            maps[row][column].Draw(spriteBatch, width, height);
        }
    }
}