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
    class FishingSpot: Object
    {
        //----------------------------------------------------------------------->>FIELDS<<
        Texture2D image;

        //----------------------------------------------------------------------->>CONSTRUCTOR<<
        public FishingSpot(Rectangle rectangle, Texture2D image) : base(rectangle)
        {
            this.image = image;

            //initialize enum
            type = ObjectType.fishingSpot;
        }


        //----------------------------------------------------------------------->>METHODS<<

        public void ResolveFishing(Player play, Random rand, InventoryItem item, MouseState mouse)
        {
            //get random number
            int percent = rand.Next(0, 10);

            if(item == Inventory.net)
            {
                if (play.Inventory.GetItemOfType(item.Name) != null && play.Inventory.GetItemOfType(item.Name).StackNum > 0)
                {
                    if (percent < 6)
                       play.Inventory.AddItem(Inventory.herring, 2);
                    else
                       play.Inventory.AddItem(Inventory.tuna, 1);

                    play.Inventory.DecrementItem(Inventory.net, 1);   
                }
            }

            if (item == Inventory.cage)
            {
                if (play.Inventory.GetItemOfType(item.Name) != null && play.Inventory.GetItemOfType(item.Name).StackNum > 0)
                {
                    if (percent < 1)
                    {
                        play.Inventory.AddItem(Inventory.lobster, 3);
                    }
                    else if (percent < 5)
                    {
                        play.Inventory.AddItem(Inventory.lobster, 2);
                    }
                    else if (percent < 9)
                    {
                        play.Inventory.AddItem(Inventory.lobster, 1);
                    }
                    play.Inventory.DecrementItem(Inventory.cage, 1);
                }
            }

            if (item == Inventory.harpoon)
            {
                if (play.Inventory.GetItemOfType(item.Name) != null && play.Inventory.GetItemOfType(item.Name).StackNum > 0)
                {
                    if (percent < 9)
                        play.Inventory.AddItem(Inventory.swordfish, 1);
                    else
                        play.Inventory.AddItem(Inventory.whale, 1);

                    play.Inventory.DecrementItem(Inventory.harpoon, 1);
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, rekt, Color.White);
        }


    }
}
