using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoatGame
{
    //death enum
    public enum Doom
    {
        alive,
        noHP,
        noFuel,
        noCrew
    }

    //singleton to handle how the player dies
    public sealed class Death
    {
        //fields
        public Doom die;
        public static readonly Death instance = new Death();

        //constructor
        private Death()
        {
            die = Doom.alive;

        }
    }
}
