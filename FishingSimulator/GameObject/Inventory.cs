using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace BoatGame
{
    //Charles Emmerich
    class Inventory
    {
        private Dictionary<string, InventoryItem> inventory;

        //item ids for fishing stuffz
        public static InventoryItem net = new InventoryItem("Net", "A net for fishing", 0, 100, 5, 2);
        public static InventoryItem harpoon = new InventoryItem("Harpoon", "A harpoon for fishing", 0, 100, 10, 5);
        public static InventoryItem cage = new InventoryItem("Cage", "A lobster cage for fishing", 0, 100, 8, 4);
        public static InventoryItem tuna = new InventoryItem("Tuna", "Smells bad", 0, 100, 0, 10);
        public static InventoryItem herring = new InventoryItem("Red Herring", "We've been smeckledorfed", 0, 100, 0, 4);
        public static InventoryItem swordfish = new InventoryItem("Swordfish", "Useful against pirates", 0, 100, 0,15);
        public static InventoryItem whale = new InventoryItem("Whale", "How is this fitting on the boat?", 0, 100, 0, 50);
        public static InventoryItem lobster = new InventoryItem("Lobster", "Watch out for claws", 0, 100, 0, 10);
        public static InventoryItem oilbarrel = new InventoryItem("Oil Barrel", "Barrels for storing your fuel", 100, 100, 70, 0);
        public static InventoryItem orange = new InventoryItem("Orange", "Yarr! Ye don't want scurvy do ye?", 100, 100, 20, 0);

        public int Size { get { return inventory.Count; } }

        //properties
        public Dictionary<string, InventoryItem> InventoryItems
        {
            get { return this.inventory; }
        }

        public Inventory()
        {
            inventory = new Dictionary<string, InventoryItem>();
        }

        public bool ContainsKey(string key)
        {
            if (inventory.ContainsKey(key))
                return true;
            return false;
        }

        //adds a number of an item to the inventory
        //if the item isn't in the inventory it adds it and the stack size
        //if add exceeds stackMax returns the differencew
        //if no issues returns -1;
        //dont try to add a negative
        public int AddItem(InventoryItem item, int num)
        {
            int difference;
            if (inventory.ContainsKey(item.Name))
            {
                InventoryItem temp;
                inventory.TryGetValue(item.Name, out temp);
                temp.StackNum += num;
                if (temp.StackNum > temp.StackMax)
                {
                    difference = temp.StackNum - temp.StackMax;
                    temp.StackNum = temp.StackMax;
                    return difference;
                }
                else return -1;
            }
            else
            {
                //add item change was done to make it so we have static items we can add and refer to and can create new items on the fly if we desire
                inventory.Add(item.Name, new InventoryItem(item.Name, item.Description, item.StackNum, item.StackMax, item.BuyPrice, item.SellPrice));
                InventoryItem temp;
                inventory.TryGetValue(item.Name, out temp);
                temp.StackNum = num;
                if (temp.StackNum > temp.StackMax)
                {
                    difference = temp.StackNum - temp.StackMax;
                    temp.StackNum = temp.StackMax;
                    return difference;
                }
                else return -1;
            }
        }

        //removes a number of a type of item from the inventory
        //if tries to remove more than available returns the difference
        //returns -1 if no problems
        public int DecrementItem(InventoryItem item, int num)
        {
            InventoryItem temp;
            inventory.TryGetValue(item.Name, out temp);

            if (temp != null)
            {
                if (temp.StackNum - num < 0)
                {
                    int diff = num - temp.StackNum;
                    temp.StackNum = 0;
                    return diff;
                }
                else
                {
                    temp.StackNum -= num;
                    return -1;
                }
            }
            return -1;
        }

        public InventoryItem GetItemOfType(string itemname)
        {
            InventoryItem temp = null;
            inventory.TryGetValue(itemname, out temp);
            return temp;
        }
    }
}
