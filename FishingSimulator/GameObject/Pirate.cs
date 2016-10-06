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
    class Pirate : Object
    {
        #region//----------------------------------------------------------------------->>FIELDS<<
        private Texture2D image;
        private bool available;
        bool direct;
        #endregion

        #region//----------------------------------------------------------------------->>CONSTRUCTOR<<
        public Pirate(int x, int y, int width, int height, Texture2D image)
            : base(new Rectangle(x, y, width, height))
        {
            this.image = image;
            this.available = true;
            direct = true;

            //initialize enum
            type = ObjectType.pirate;
        }
        #endregion

        #region//----------------------------------------------------------------------->>PROPERTIES<<
        public bool Available
        {
            get { return available; }
            set { available = value; }
        }
        #endregion

        #region//----------------------------------------------------------------------->>METHODS<<
        public void ResolvePirate(Player play)
        {
            Random rand = new Random();
            if(available)
            {
                play.changeHealth(rand.Next((int)Math.Ceiling(play.Health / 5f), (int)Math.Ceiling((play.Health / 3f)))* -1);
                int num = play.changeCrew(rand.Next((int)Math.Ceiling(play.Crew / 6f), (int)Math.Ceiling((play.Crew / 2f)))* -1);
                if (num != -1)
                    play.Crew -= num;
                this.available = false;
            }
        }

        public void Move(Player play, Random rand)
        {
            if (available)
            {
                Vector2 start = new Vector2(this.rekt.X, this.rekt.Y);
                Vector2 end = new Vector2(play.XPos, play.YPos);

                float distance = Vector2.Distance(start, end);
                Vector2 direction = Vector2.Normalize(end - start);
                int movement = 0;
                if (distance < 90)
                {
                    movement = rand.Next(6, 10);
                    this.rekt.X += (int)(direction.X * movement);
                    this.rekt.Y += (int)(direction.Y * movement);
                }
                else if (distance < 150)
                {
                    movement = rand.Next(5, 8);
                    this.rekt.X += (int)(direction.X * movement);
                    this.rekt.Y += (int)(direction.Y * movement);
                }
                else
                {
                    movement = rand.Next(2, 5);
                    this.rekt.X += (int)(direction.X * movement);
                    this.rekt.Y += (int)(direction.Y * movement);
                }

                if(play.Rekt.X > rekt.X)
                {
                    direct = true;
                }
                else
                {
                    direct = false;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (available)
            {
                if (direct)
                {
                    spriteBatch.Draw(image, rekt, Color.White);
                }
                else
                {
                    SpriteEffects se = SpriteEffects.FlipHorizontally;
                    spriteBatch.Draw(image, rekt, new Rectangle(0, 0, image.Width, image.Height), Color.White, 0f, new Vector2(0, 0), se, 0f);
                }
            }
        }
        #endregion
    }
}
