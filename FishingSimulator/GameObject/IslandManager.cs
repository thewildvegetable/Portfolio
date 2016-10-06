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
    class IslandManager:Object
    {

        //----------------------------------------------------------------------->>FIELDS<<

        /* We should later implement a Dictionary<String, bool> options and their properties so
         * each island has a set of options to choose from;
         * if a bool is true then the string option is displayed;
         * if a bool is false then the string option will not be displayed
         */
        private Dictionary<String, bool> options;
        Texture2D image;

        //----------------------------------------------------------------------->>CONSTRUCTOR<<
        //LATER ADD
        public IslandManager(int x, int y, int width, int height, Texture2D image): base(new Rectangle(x, y, width, height))
        {
            options = new Dictionary<String, bool>();
            this.image = image;
        }

        //----------------------------------------------------------------------->>PROPERTIES<<
        public Rectangle Pos
        {
            get { return rekt; }
        }

        //----------------------------------------------------------------------->>METHODS<<
        //Should add a series of strings and their set booleans
        //options.Add(a single option string here, whether or not it appears)
        public void setDictionary(bool refuel, bool repair, bool fishSell, bool buyItems)
        {
            //Just an option for now...can be changed later
            options.Add("Refuel your boat", refuel);
            options.Add("Repair your boat", repair);
            options.Add("Sell fish", fishSell);
            options.Add("Buy an item", buyItems);
        }

        public void DrawIslands(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw();
        }
    }
}