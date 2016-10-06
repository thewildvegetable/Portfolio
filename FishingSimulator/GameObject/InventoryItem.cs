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
    class InventoryItem
    {
        private string name;
        private string description;
        private int stackNum;
        private int stackMax;
        public int buyPrice;
        public int sellPrice;

        public int StackNum { get { return stackNum; } set { stackNum = value;} }
        public int StackMax { get { return stackMax; } }
        public string Name { get { return name; } }
        public string Description { get { return description; } }
        public int BuyPrice { get { return buyPrice; } set { buyPrice = value; } }
        public int SellPrice { get { return sellPrice; } set { sellPrice = value; } }

        //later we may want to have classes that inherit from this
        //or we could have an enum for the different items
        public InventoryItem(string name, string description, int stackNum, int stackMax, int buyPrice, int sellPrice)
        {
            this.name = name;
            this.description = description;
            this.stackNum = stackNum;
            this.stackMax = stackMax;
            this.buyPrice = buyPrice;
            this.sellPrice = sellPrice;
        }
    }
}
