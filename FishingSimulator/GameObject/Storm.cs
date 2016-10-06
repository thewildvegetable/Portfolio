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
    class Storm : Object
    {
        #region//----------------------------------------------------------------------->>FIELDS<<
        private Texture2D image;
        private bool available;
        Random rand;
        int xMove;
        int yMove;
        int xMod;
        int yMod;
        int change;
        #endregion

        #region//----------------------------------------------------------------------->>CONSTRUCTOR<<
        public Storm(int x, int y, int width, int height, Texture2D image)
            : base(new Rectangle(x, y, width, height))
        {
            this.image = image;
            this.available = true;
            rand = new Random();


            //initialize enum
            type = ObjectType.storm;
        }
        #endregion

        #region//----------------------------------------------------------------------->>PROPERTIES<<

        public bool Available { get { return available; } set { available = value; } }
        #endregion

        #region//----------------------------------------------------------------------->>METHODS<<

        /// <summary>
        /// occurs when player collides with a storm
        /// decreases health and fuel
        /// </summary>
        /// <param name="play"></param>
        public void ResolveStorm(Player play, Random rand)
        {
            if (available == true)
            {
                play.changeFuel(-5f);
                play.changeHealth(rand.Next(-30, -10));
                this.available = false;
            }
        }

        public void Move(int screenWidth, int screenHeight, Player play)
        {
            
            xMove = rand.Next(-4, 5);
            yMove = rand.Next(-4, 5);
            change = rand.Next(-15, 15);
            if (change == -10)
            {
                xMod = -2;
            }
            else if (change == 10)
            {
                xMod = 2;
            }
            else if (change == 0)
            {
                xMod = 0;
            }
            change = rand.Next(-15, 15);
            if (change == -10)
            {
                yMod = -2;
            }
            else if (change == 10)
            {
                yMod = 2;
            }
            else if (change == 0)
            {
                yMod = 0;
            }
            xMove += xMod;
            yMove += yMod;

            rekt.X += xMove;
            rekt.Y += yMove;
            
            if(rekt.X < 0)
            {
                rekt.X += Math.Abs(xMove);
            }
            else if(rekt.X + image.Width >= screenWidth)
            {
                rekt.X -= Math.Abs(xMove);
            }
            if(rekt.Y < 0)
            {
                rekt.Y += Math.Abs(yMove);
            }
            else if(rekt.Y + image.Height >= screenHeight)
            {
                rekt.Y -= Math.Abs(yMove);
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(available)
                spriteBatch.Draw(image, rekt, Color.White);
        }
        #endregion
    }
}
