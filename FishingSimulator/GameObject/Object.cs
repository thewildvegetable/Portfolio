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
    abstract class Object
    {
        //fields
        protected Rectangle rekt;
        protected ObjectType type;

        public Rectangle Rekt
        {
            get { return this.rekt; }
            set { this.rekt = value; }
        }
        
        public virtual ObjectType Type
        {
            get { return this.type; }
        }

        //constructors
        public Object(Rectangle rect)
        {
            rekt = rect;
        }

    }
}
