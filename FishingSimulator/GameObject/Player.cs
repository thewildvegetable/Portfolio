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
    //Charles Emmerich
    class Player : Object
    {
        #region//----------------------------------------------------------------------->>FIELDS<<
        private string name;
        private float health;
        private int currency;
        private float fuel;
        private int crew;
        private bool docked;
        private Inventory inventory;
        private Texture2D image;
        private Random rand;
        private List<Object> objList;
        private const int speed = 5;
        bool idle;
        Directions direction;
        Island dockedIsland;                //the island you are parked at
        #endregion

        #region//----------------------------------------------------------------------->>PROPERTIES<<
        public int XPos
        {
            get { return rekt.X; }
            set { rekt.X = value; }
        }
        public int YPos
        {
            get { return rekt.Y; }
            set { rekt.Y = value; }
        }
        public float Fuel
        {
            get { return fuel; }
        }
        public int Currency
        {
            get { return currency; }
        }
        public float Health
        {
            get { return health; }
        }
        public int Crew { get { return crew; } set { crew = value; } }
        public bool Docked
        {
            get { return docked; }
            set { docked = value; }
        }
        public Inventory Inventory
        {
            get { return inventory; }
        }
        public Island DockedIsland
        {
            get { return this.dockedIsland; }
        }
        public String Name
        {
            get { return name; }
        }
        public Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }
        public List<Object> ObjList
        {
            get { return objList; }
            set { objList = value; }
        }
        #endregion

        #region//----------------------------------------------------------------------->>CONSTRUCTOR<<
        public Player(String name, Rectangle rect)
            : base(rect)
        {
            this.name = name;
            docked = true;
            fuel = 100f;
            health = 100f;
            currency = 50;
            crew = 5;
            inventory = new Inventory();
            this.image = null;
            rand = new Random();
            objList = new List<Object>();
            idle = false;
            direction = Directions.left;
            dockedIsland = null;

            //initialize enum
            type = ObjectType.player;

            //add starting items to inventory
            inventory.AddItem(Inventory.harpoon, 5);
            inventory.AddItem(Inventory.cage, 5);
            inventory.AddItem(Inventory.net, 5);
        }
        #endregion

        #region//----------------------------------------------------------------------->>FIELDS<<

        //increments or decrements player currency
        //if the wanted decrement puts currency below 0 returns absolute value of the result (doesn't do the decrement)
        //returns -1 elsewise
        public int changeCurrency(int num)
        {
            if (num > 0)
            {
                currency += num;
                return -1;
            }

            if (num < 0 && currency + num > -1)
            {
                currency += num;
                return -1;
            }

            return (num + currency) * -1;
        }

        //increments/decrements fuel
        //restricts amount to between 0 and 100
        //returns difference if exceeds 100
        public int changeFuel(float num)
        {
            if (num > 0)
            {
                fuel += num;
                if (fuel > 100)
                {
                    int temp = (int)fuel - 100;
                    fuel = 100;
                    return temp;
                }
            }

            if (num < 0)
            {
                fuel += num;
                if (fuel < 0)
                    fuel = 0;
            }

            return 0;
        }

        //increments or decrements players crew number
        //if the wanted decrement puts crew below 0 returns the difference
        //returns -1 elsewise
        public int changeCrew(int num)
        {
            if (num < 0 && crew + num < 0)
            {
                int temp = (crew + num) * -1;
                crew = 0;
                return temp;
            }
            else
            {
                crew += num;
            }

            return -1;
        }

        //increments/decrements player health
        //keeps between 0-100
        //returns difference if exceeds 100
        //returns 0 with no issue
        public int changeHealth(int num)
        {
            health += num;
            if (health > 100)
            {
                int temp = (int)health - 100;
                health = 100;
                return temp;
            }

            if (num < 0)
            {
                if (health < 0)
                    health = 0;
            }

            return 0;
        }
        #endregion

        #region//----------------------------------------------------------------------->>METHODS<<

        #region Update / Draw
        public States UpdateLocation(KeyboardState keyboard, List<Object> objects, States currentState)
        {
            idle = true;
            objList = objects;
            bool collided = false;
            Rectangle rectangle = rekt;


            foreach (Storm storm in objects.OfType<Storm>())
            {
                if (this.rekt.Intersects(storm.Rekt))
                {
                    storm.ResolveStorm(this, new Random());
                }
            }

            foreach (Pirate pirate in objects.OfType<Pirate>())
            {
                if (this.rekt.Intersects(pirate.Rekt))
                {
                    pirate.ResolvePirate(this);
                }
            }

            #region If the player has fuel
            if (fuel > 0)
            {
                //check if the keyboard will move the player
                #region Going left (Keys.A)
                if (keyboard.IsKeyDown(Keys.A))
                {
                    rekt.X -= speed;
                    direction = Directions.left;
                    idle = false;
                }
                #endregion
                #region Going right (Keys.D)
                if (keyboard.IsKeyDown(Keys.D))
                {
                    rekt.X += speed;
                    direction = Directions.right;
                    idle = false;
                }
                #endregion
                #region Going up (Keys.W)
                if (keyboard.IsKeyDown(Keys.W))
                {
                    rekt.Y -= speed;
                    idle = false;
                }
                #endregion
                #region Going down (Keys.S)
                if (keyboard.IsKeyDown(Keys.S))
                {
                    rekt.Y += speed;
                    idle = false;
                }
                #endregion
            #endregion
                if (!idle)
                {
                    changeFuel(-.025f);
                }
            }

            for (int i = 0; i < objects.Count; i++)
            {
                //check if picking up the player himself
                if (objects[i] != this)
                {
                    //check if intersection is occuring
                    if (this.rekt.Intersects(objects[i].Rekt))
                    {
                        //check if the object is an island
                        if (objects[i].Type == ObjectType.island)
                        {
                            currentState = States.docked;
                            collided = true;
                            dockedIsland = (objects[i] as Island);
                        }
                        else if (objects[i].Type == ObjectType.fishingSpot)
                        {
                            currentState = States.fishing;
                            collided = true;
                        }
                    }
                }
                if (collided)
                    rekt = rectangle;
            }
            return currentState;
        }

        //should do animating here i suppose
        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            SpriteEffects imageFlip = SpriteEffects.FlipHorizontally;
            if (this.direction == Directions.left)
            {
                spriteBatch.Draw(image, rekt, new Rectangle(0, 0, 120, 115), Color.White, 0f, Vector2.Zero, imageFlip, 0);
            }
            if (this.direction == Directions.right)
            {
                spriteBatch.Draw(image, rekt, Color.White);
            }
            if (direction == Directions.up || direction == Directions.down || direction == Directions.idle)
                spriteBatch.Draw(image, rekt, Color.White);

        }

        #endregion

        #endregion
    }
}