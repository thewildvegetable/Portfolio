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
    class MapNode : Object
    {

        #region//----------------------------------------------------------------------->>FIELDS<<
        private int islandNum;                                  //Number of islands in this map area + manager
        private int fishNum;                                    //Number of fishing spots in the map area + manager
        private int stormNum;                                   //Number of storms in the map area + manager
        private int pirateNum;                                  //Number of pirates in the map area + manager
        private int screenWidth;                                //Width of the screen
        private int screenHeight;                               //Height of the screen

        private List<Island> islandList;                        //List of islands in the MapNode
        private List<Texture2D> islandImages;                   //List of textures for the islands to use
        private List<FishingSpot> fishList;                     //List of fishing spots in the MapNode
        private List<Storm> stormList;                          //List of storms in the MapNode
        private List<Pirate> pirateList;                        //List of pirates in the MapNode
        private List<Object> objects;                           //Keeps track of all islands, vortexes, fishing spots on the map

        private Texture2D mapImage;                             //Texture to contain the specific section of map
        private Texture2D fishImage;                            //Texture to contain the fishing spot
        private Texture2D stormImage;                           //Texture to contain the storm
        private Texture2D pirateImage;                          //Texture to contain the pirate
        #endregion

        #region//----------------------------------------------------------------------->>CONSTRUCTOR<<
        public MapNode(int islands, int fish, int storms, int pirates, Texture2D map, List<Texture2D> islandImages,
                       Texture2D fSpot, Texture2D stormImage, Texture2D pirateImage, int screenWidth, int screenHeight):
                       base(new Rectangle(0, 0, screenWidth, screenHeight))
        {
            islandNum = islands;
            this.islandList = new List<Island>();

            mapImage = map;
            this.islandImages = islandImages;

            fishNum = fish;
            fishImage = fSpot;
            fishList = new List<FishingSpot>();

            stormNum = storms;
            this.stormImage = stormImage;
            stormList = new List<Storm>();

            pirateNum = pirates;
            this.pirateImage = pirateImage;
            pirateList = new List<Pirate>();

            objects = new List<Object>();

            this.screenHeight = screenHeight;

            this.screenWidth = screenWidth;

        }
        #endregion

        #region//----------------------------------------------------------------------->>PROPERTIES<<
        public int IslandNum
        {
            get { return islandNum; }
        }
        public List<Island> Islands
        {
            get { return islandList; }
        }
        public List<FishingSpot> FishingSpots
        {
            get { return fishList; }
        }
        public List<Storm> Vortexes
        {
            get { return stormList; }
        }
        public List<Pirate> Pirates 
        {
            get { return pirateList; }
        }
        public int FishNum
        {
            get { return fishNum; }
            set { fishNum = value; }
        }
        public int StormNum
        {
            get { return stormNum; }
            set { stormNum = value; }
        }
        public int PirateNum
        {
            get { return pirateNum; }
            set { pirateNum = value; }
        }
        #endregion

        #region//----------------------------------------------------------------------->>METHODS<<
        //Creates the island (if there is one) on the map
        //places in a random location with a random size
        public void makeIslands(Random rand, Player play)
        {
            List<int> removedindexes = new List<int>();
            
            Island tempIsland;
            Texture2D islImage;

            for (int i = 0; i < islandNum; i++)
            {
                //Makes sure no islands have the same image on screen
                int num0 = rand.Next(0, islandImages.Count);
                while(removedindexes.Contains(num0))
                {
                    num0 = rand.Next(0, islandImages.Count);
                }
                islImage = islandImages[num0]; 
                removedindexes.Add(num0);

                //Creates the parameters for the new islands
                int num1 = rand.Next(0, screenWidth - islImage.Width - 50);
                int num2 = rand.Next(0, screenHeight - islImage.Height - 50);
                int num3 = rand.Next(50, 150);
                int num4 = rand.Next(50, 150);

                //Creates the island / adds them to the island and object lists
                tempIsland = new Island(num1, num2, num3, num3, islImage);

                islandList.Add(tempIsland);
                objects.Add(tempIsland);
            }
            
            //This crazy bastard makes sure islands don't overlap
            for(int j = 0; j < islandList.Count; j++)
            {
                for(int k = 0; k < islandList.Count; k++)
                {
                    rand = new Random();
                    while(j!=k && (islandList[j].Rekt.Intersects(islandList[k].Rekt) || islandList[k].Rekt.Intersects(play.Rekt)))
                    {
                        islandList[k].Rekt = new Rectangle(rand.Next(0, screenWidth - islandList[k].Rekt.Width),
                                                        rand.Next(0, screenHeight - islandList[k].Rekt.Height),
                                                        rand.Next(50, islandList[k].Rekt.Width),
                                                        rand.Next(50, islandList[k].Rekt.Height));
                    }
                }
            }
                   
        }

        public void makeFishingSpots(Random rand)
        {
            int dimensions = rand.Next(25, 75);
            FishingSpot temp;

            for (int i = 0; i < fishNum; i++)
            {
                int num1 = rand.Next(0, screenWidth - fishImage.Width * 2);
                int num2 = rand.Next(0, screenHeight - fishImage.Height * 2);

                temp = new FishingSpot(new Rectangle(num1, num2, dimensions, dimensions), fishImage);

                fishList.Add(temp);
                
            }
            //---------------------------------------------------------------------------------------------------------------------------------->>CHECK THIS CODE LATER<<
            //This crazy bastard makes sure other objects onscreen don't overlap
            for (int j = 0; j < fishList.Count; j++)
            {
                for (int k = 0; k < objects.Count; k++)
                {
                    while (fishList[j].Rekt.Intersects(objects[k].Rekt))
                    {
                        fishList[j].Rekt = new Rectangle(rand.Next(0, screenWidth - fishList[j].Rekt.Width),
                                                        rand.Next(0, screenHeight - fishList[j].Rekt.Height),
                                                        dimensions,
                                                        dimensions);
                    }
                }
                objects.Add(fishList[j]);
            }
        }

        public void makeVortexes(Random rand)
        {
            int dimensions = rand.Next(50, 75);
            Storm temp;

            for (int i = 0; i < stormNum; i++)
            {
                int num1 = rand.Next(0, screenWidth - stormImage.Width);
                int num2 = rand.Next(0, screenHeight - stormImage.Height);

                temp = new Storm(num1, num2, dimensions, dimensions, stormImage);

                stormList.Add(temp);
                objects.Add(temp);
            }
        }

        public void makePirates(Random rand)
        {
            int dimensions = rand.Next(35, 65);
            Pirate temp;

            for(int i = 0; i < pirateNum; i++)
            {
                int num1 = rand.Next(0, screenWidth - pirateImage.Width);
                int num2 = rand.Next(0, screenHeight - pirateImage.Height);

                temp = new Pirate(num1, num2, dimensions, dimensions, pirateImage);

                pirateList.Add(temp);
                objects.Add(temp);
            }

        }

        public void RemoveFishingSpot(FishingSpot fish)
        {
            fishList.Remove(fish);
            objects.Remove(fish);
        }
        public void RemoveVortex(Storm vortex)
        {
            stormList.Remove(vortex);
            objects.Remove(vortex);
        }
        public void RemovePirate(Pirate pete)
        {
            pirateList.Remove(pete);
            objects.Remove(pete);
        }

        //Takes a bunch of random items from the item list and adds them to the island
        public void AddItem(List<InventoryItem> items, bool addItem, Random rand)
        {
            for (int i = 0; i < items.Count; i++)
            {
                foreach (Island isl in islandList)
                {
                    if (rand.Next(0, 2) > 0)
                        addItem = true;
                    else
                        addItem = false;

                    if (addItem)
                    {
                        isl.AddItem(items[i]);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, int width, int height)
        {
            spriteBatch.Draw(mapImage, new Rectangle(0, 0, width, height), Color.White);
            for(int i = 0; i < islandNum; i++)
            {
                islandList[i].Draw(spriteBatch);
            }
            for(int i = 0; i < fishNum; i++)
            {
                fishList[i].Draw(spriteBatch);
            }
            for(int i = 0; i < stormNum; i++)
            {
                if(stormList[i].Available)
                    stormList[i].Draw(spriteBatch);
            }
            for(int i = 0; i < pirateNum; i++)
            {
                if (pirateList[i].Available)
                    pirateList[i].Draw(spriteBatch);
            }
        }
        #endregion
    }
}