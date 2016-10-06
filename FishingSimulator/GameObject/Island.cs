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
    class Island: Object
    {
        //----------------------------------------------------------------------->>FIELDS<<

        /* We should later implement a Dictionary<String, bool> options and their properties so
         * each island has a set of options to choose from;
         * if a bool is true then the string option is displayed;
         * if a bool is false then the string option will not be displayed
         */
        Texture2D image;
        public Inventory inventory;

        //----------------------------------------------------------------------->>CONSTRUCTOR<<
        public Island(int x, int y, int width, int height, Texture2D image): base(new Rectangle(x, y, width, height))
        {
            this.image = image;

            //initialize enum
            type = ObjectType.island;

            //initialize inventory
            this.inventory = new Inventory();
        }

        //----------------------------------------------------------------------->>PROPERTIES<<
        public Inventory Items 
        { 
            get { return this.inventory; }
            set { this.inventory = value; }
        }

        //island inherits object. object has the rectangle property already, DONT REIMPLEMENT IT!

        //----------------------------------------------------------------------->>METHODS<<

        //Adds the specified item to the island's inventory
        public void AddItem(InventoryItem item)
        {
            Items.AddItem(item, int.MaxValue);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, rekt, Color.White);
        }
    }
}
