using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoatGame
{
    class ShopItem: InventoryItem
    {
        private int cost;

        public int Cost { get { return cost; } }

        public ShopItem(string name, string description, int cost, int stackNum, int stackMax): base(name, description, stackNum, stackMax)
        {
            this.cost = cost;
        }

    }
}
