#region Using Statements
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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Diagnostics;

#endregion

//Charles Emmerich
//Jeffrey Karger
//Sam Levey
//Chas Parr
namespace BoatGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    //state enum


    public class Game1 : Game
    {
        #region Fields
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player play;
        int currentRow;                     //current row in 2d array
        int currentCol;                     //current column in 2d array
        States gameState;                   //what state is the game in?
        MouseState mouseState;              //mouse state 
        Point mousePosition;                //mouse position
        List<Object> objectList;            //list for all objects in current node

        UI_manager ShopUImanager;           //UI manager
        UI_manager TitleUIManager;          //manages the buttons that change game states in title menu
        UI_manager OverUIManager;           //manages the transition from game over to title
        UI_manager ControlUIManager;        //manages the transition from controls to title
        UI_manager FishingUI;
        UI_manager InventoryUIManager;

        MapManager mapManager;              //Map manager for the map areas
        List<Texture2D> islandImages;       //List of the island images
        Texture2D mapImage;                 //picture of the water
        Texture2D blankImage;               //a blank image
        Texture2D fishingImage;             //The fishing spot image
        Texture2D stormImage;               //The storm image
        Texture2D pirateImage;              //The pirate image
        Texture2D buttonImage;              //The button image

        Texture2D keyImage;                 //The WASD keys for the control screen
        Texture2D iKeyImage;                //The I key for the control screen

        Texture2D titleText;                //Title letters
        Texture2D titleImage;               //Beginning of the game background
        Texture2D gameOverImage;            //End of the game background
        Texture2D shopMenu;                 //Shopmenu image
        Texture2D fishingScreenImage;             //Image when fishing

        SpriteFont font;
        SoundEffectInstance surf;
        SoundEffectInstance mart;
        SoundEffectInstance dead;
        SoundEffectInstance title;
        SoundEffectInstance ching;
        #endregion

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            #region Initialize Player
            //initialize player. will update width and length when image is present
            play = new Player("Player", new Rectangle(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 25, 25));
            #endregion

            #region Initialize Rows / Columns
            currentCol = 0;
            currentRow = 0;
            #endregion

            #region Initialize gameState
            //initialize enum
            gameState = States.titleMenu;
            #endregion

            #region Initialize objectList
            //Creates an object list to hold the objects currently on screen
            objectList = new List<Object>();
            #endregion

            #region Initialize UImanager
            //initialize manager
            ShopUImanager = new UI_manager();
            TitleUIManager = new UI_manager();
            ControlUIManager = new UI_manager();
            OverUIManager = new UI_manager();
            FishingUI = new UI_manager();
            InventoryUIManager = new UI_manager();
            #endregion

            #region Initialize mapManager
            mapManager = new MapManager();
            #endregion

            #region Initialize islandImages
            islandImages = new List<Texture2D>();
            #endregion

            #region Creates the fullscreen effect
            Window.Position = new Point(0, 0);
            Window.IsBorderless = true;
            graphics.PreferredBackBufferWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            graphics.PreferredBackBufferHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            graphics.ApplyChanges();
            #endregion

            //start external tool
            //Process.Start("C:/Users/Jeffrey Karger/Desktop/scrublords/GameObject/FileChecker/bin/Debug/FileChecker.exe");
            DirectoryInfo di = new DirectoryInfo(".");

            //ProcessStartInfo info = new ProcessStartInfo();
            //info.WorkingDirectory = "../../../../FileChecker/bin/Debug/";
            //info.FileName = "FileChecker.exe";
            //info.UseShellExecute = false;
            //Process.Start(info);
            Process.Start(di.FullName + "/../../../../FileChecker/bin/Debug/FileChecker.exe");


            base.Initialize();
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            surf = Content.Load<SoundEffect>("surf").CreateInstance();
            surf.Volume = .06f;
            surf.IsLooped = true;

            mart = Content.Load<SoundEffect>("shop").CreateInstance();
            mart.Volume = .06f;
            mart.IsLooped = true;

            dead = Content.Load<SoundEffect>("dead").CreateInstance();
            dead.Volume = .4f;

            title = Content.Load<SoundEffect>("menu").CreateInstance();
            title.Volume = .8f;
            title.IsLooped = true;

            ching = Content.Load<SoundEffect>("ching").CreateInstance();
            ching.Volume = .5f;

            bool newRow = true;

            #region Loading in images

            #region Loads in boat image(s)
            System.IO.Stream fs = TitleContainer.OpenStream("Content/Boat_Left.png");
            play.Image = Texture2D.FromStream(this.GraphicsDevice, fs);
            fs.Close();
            #endregion

            #region Loads in the MapNode image(s)
            //Load in the maps 
            System.IO.Stream mapStream = TitleContainer.OpenStream("Content/ocean.jpg");
            mapImage = Texture2D.FromStream(GraphicsDevice, mapStream);
            mapStream.Close();
            #endregion

            #region Loads in the island images
            //Load in all island images to islandImages -- make more efficient later
            System.IO.Stream islandStream = TitleContainer.OpenStream("Content/Island1.png");
            Texture2D loadedImages = Texture2D.FromStream(GraphicsDevice, islandStream);
            islandImages.Add(loadedImages);
            islandStream.Close();
            islandStream = TitleContainer.OpenStream("Content/Island2.png");
            loadedImages = Texture2D.FromStream(GraphicsDevice, islandStream);
            islandImages.Add(loadedImages);
            islandStream.Close();
            islandStream = TitleContainer.OpenStream("Content/Island3.png");
            loadedImages = Texture2D.FromStream(GraphicsDevice, islandStream);
            islandImages.Add(loadedImages);
            islandStream.Close();
            islandStream = TitleContainer.OpenStream("Content/Island4.png");
            loadedImages = Texture2D.FromStream(GraphicsDevice, islandStream);
            islandImages.Add(loadedImages);
            islandStream.Close();
            islandStream = TitleContainer.OpenStream("Content/Island5.png");
            loadedImages = Texture2D.FromStream(GraphicsDevice, islandStream);
            islandImages.Add(loadedImages);
            islandStream.Close();
            islandStream = TitleContainer.OpenStream("Content/Island6.png");
            loadedImages = Texture2D.FromStream(GraphicsDevice, islandStream);
            islandImages.Add(loadedImages);
            islandStream.Close();
            #endregion

            #region Loads in the Menu images
            System.IO.Stream menuStream;
            menuStream = TitleContainer.OpenStream("Content/Keys.png");
            keyImage = Texture2D.FromStream(GraphicsDevice, menuStream);

            menuStream = TitleContainer.OpenStream("Content/IKey.png");
            iKeyImage = Texture2D.FromStream(GraphicsDevice, menuStream);

            menuStream = TitleContainer.OpenStream("Content/Title.png");
            titleText = Texture2D.FromStream(GraphicsDevice, menuStream);

            menuStream = TitleContainer.OpenStream("Content/TitleScreen.png");
            titleImage = Texture2D.FromStream(GraphicsDevice, menuStream);

            menuStream = TitleContainer.OpenStream("Content/GameOver.png");
            gameOverImage = Texture2D.FromStream(GraphicsDevice, menuStream);

            menuStream = TitleContainer.OpenStream("Content/Menu Screen.png");
            shopMenu = Texture2D.FromStream(GraphicsDevice, menuStream);

            menuStream = TitleContainer.OpenStream("Content/Fishing Screen.png");
            fishingScreenImage = Texture2D.FromStream(GraphicsDevice, menuStream);

            menuStream = TitleContainer.OpenStream("Content/Empty.png");
            blankImage = Texture2D.FromStream(GraphicsDevice, menuStream);

            menuStream.Close();
            #endregion

            #region Loads in the font
            //load font
            this.font = Content.Load<SpriteFont>("Arial14");
            #endregion

            #endregion
            int width = GraphicsDevice.Viewport.Width / 7; //1600 / 8
            int halfWidth = GraphicsDevice.Viewport.Width / 2;
            int height = GraphicsDevice.Viewport.Height / 10; //900 / 10
            int heightAdj = height / 2;
            #region Creates and adds all UI items for the shops

            menuStream = TitleContainer.OpenStream("Content/Button.png");
            buttonImage = Texture2D.FromStream(GraphicsDevice, menuStream);
            menuStream.Close();

            //text box
            TextBox exit = new TextBox("Back to Map", new Rectangle(20, GraphicsDevice.Viewport.Height - 100, 300, 80), buttonImage);
            ShopUImanager.AddUIObject(exit);

            //all buy buttons
            Button fuel = new Button("Purchase Fuel: 1 currency per unit", play, new Rectangle(width + halfWidth, height, 370, 25), buttonImage, ChangeValue.fuel);
            Button repair = new Button("Repair Ship: 1 currency per unit", play, new Rectangle(width + halfWidth, height + heightAdj, 370, 25), buttonImage, ChangeValue.health);
            Button crew = new Button("Hire Crew Members: 1 currency per unit", play, new Rectangle(width + halfWidth, height + heightAdj * 2, 370, 25), buttonImage, ChangeValue.crew);
            Button net = new Button("Net: " + Inventory.net.BuyPrice + " currency per unit", play, new Rectangle(width + halfWidth, height + heightAdj * 3, 370, 25), buttonImage, ChangeValue.inventoryBuy, Inventory.net);
            Button harpoon = new Button("Harpoon : " + Inventory.harpoon.BuyPrice + " currency per unit", play, new Rectangle(width + halfWidth, height + heightAdj * 4, 370, 25), buttonImage, ChangeValue.inventoryBuy, Inventory.harpoon);
            Button cage = new Button("Cage : " + Inventory.cage.BuyPrice + " currency per unit", play, new Rectangle(width + halfWidth, height + heightAdj * 5, 370, 25), buttonImage, ChangeValue.inventoryBuy, Inventory.cage);

            ShopUImanager.AddUIObject(fuel);
            ShopUImanager.AddUIObject(repair);
            ShopUImanager.AddUIObject(crew);
            ShopUImanager.AddUIObject(net);
            ShopUImanager.AddUIObject(harpoon);
            ShopUImanager.AddUIObject(cage);

            //all sell buttons
            Button sellCrew = new Button("Sell Crew Members: 1 currency per unit", play, new Rectangle(width, height, 370, 25), buttonImage, ChangeValue.crew);
            Button sellNet = new Button("Sell Net: " + Inventory.net.SellPrice + " currency per unit", play, new Rectangle(width, height + heightAdj, 370, 25), buttonImage, ChangeValue.inventorySell, Inventory.net);
            Button sellHarpoon = new Button("Sell Harpoon : " + Inventory.harpoon.SellPrice + " currency per unit", play, new Rectangle(width, height + heightAdj * 2, 370, 25), buttonImage, ChangeValue.inventorySell, Inventory.harpoon);
            Button sellCage = new Button("Sell Cage : " + Inventory.cage.SellPrice + " currency per unit", play, new Rectangle(width, height + heightAdj * 3, 370, 25), buttonImage, ChangeValue.inventorySell, Inventory.cage);
            Button tuna = new Button("Sell Tuna : " + Inventory.tuna.SellPrice + " currency per unit", play, new Rectangle(width, height + heightAdj * 4, 370, 25), buttonImage, ChangeValue.inventorySell, Inventory.tuna);
            Button herring = new Button("Sell Herring : " + Inventory.herring.SellPrice + " currency per unit", play, new Rectangle(width, height + heightAdj * 5, 370, 25), buttonImage, ChangeValue.inventorySell, Inventory.herring);
            Button swordfish = new Button("Sell Swordfish : " + Inventory.swordfish.SellPrice + " currency per unit", play, new Rectangle(width, height + heightAdj * 6, 370, 25), buttonImage, ChangeValue.inventorySell, Inventory.swordfish);
            Button whale = new Button("Sell Whale : " + Inventory.whale.SellPrice + " currency per unit", play, new Rectangle(width, height + heightAdj * 7, 370, 25), buttonImage, ChangeValue.inventorySell, Inventory.whale);
            Button lobster = new Button("Sell Lobster : " + Inventory.lobster.sellPrice + " currency per unit", play, new Rectangle(width, height + heightAdj * 8, 370, 25), buttonImage, ChangeValue.inventorySell, Inventory.lobster);

            ShopUImanager.AddUIObject(sellCrew);
            ShopUImanager.AddUIObject(sellNet);
            ShopUImanager.AddUIObject(sellHarpoon);
            ShopUImanager.AddUIObject(sellCage);
            ShopUImanager.AddUIObject(tuna);
            ShopUImanager.AddUIObject(herring);
            ShopUImanager.AddUIObject(swordfish);
            ShopUImanager.AddUIObject(whale);
            ShopUImanager.AddUIObject(lobster);
            #endregion



            #region creates all gamestate buttons
            //create and add all title menu buttons
            TextBox startButton = new TextBox("Start", new Rectangle(GraphicsDevice.Viewport.Width / 2 - 125, GraphicsDevice.Viewport.Height / 2 + 70, 300, 80), buttonImage);
            TextBox controlButton = new TextBox("Controls", new Rectangle(GraphicsDevice.Viewport.Width / 2 - 125, GraphicsDevice.Viewport.Height / 2 + 150, 300, 80), buttonImage);
            TitleUIManager.AddUIObject(startButton);
            TitleUIManager.AddUIObject(controlButton);

            //create and add control screen buttons
            TextBox titleButton = new TextBox("To Title", new Rectangle(GraphicsDevice.Viewport.Width / 2 - 125, GraphicsDevice.Viewport.Height / 2 + 70, 300, 80), buttonImage);
            ControlUIManager.AddUIObject(titleButton);

            //create and add game over buttons
            TextBox overButton = new TextBox("To Title", new Rectangle(GraphicsDevice.Viewport.Width / 2 - 125, GraphicsDevice.Viewport.Height / 2 + 70, 300, 80), buttonImage);
            OverUIManager.AddUIObject(overButton);

            //fishing buttons
            TextBox tunaInvent = new TextBox(new Rectangle((int)Math.Floor(width * 5.5), height * 2, 250, 50), buttonImage, Inventory.tuna);
            TextBox herringInvent = new TextBox(new Rectangle((int)Math.Floor(width * 5.5), (int)Math.Ceiling(height * 3.5), 250, 50), buttonImage, Inventory.herring);
            TextBox lobsterInvent = new TextBox(new Rectangle((int)Math.Floor(width * 5.5), height * 5, 250, 50), buttonImage, Inventory.lobster);
            TextBox swordInvent = new TextBox(new Rectangle((int)Math.Floor(width * 5.5), (int)Math.Floor(height * 6.5), 250, 50), buttonImage, Inventory.swordfish);
            TextBox whaleInvent = new TextBox(new Rectangle((int)Math.Floor(width * 5.5), height * 8, 250, 50), buttonImage, Inventory.whale);

            TextBox useNet = new TextBox(new Rectangle(width * 4, height * 2, 250, 50), buttonImage, Inventory.net);
            TextBox useCage = new TextBox(new Rectangle(width * 4, height * 5, 250, 50), buttonImage, Inventory.cage);
            TextBox useHarp = new TextBox(new Rectangle(width * 4, height * 8, 250, 50), buttonImage, Inventory.harpoon);

            FishingUI.AddUIObject(exit);
            FishingUI.AddUIObject(useNet);
            FishingUI.AddUIObject(useCage);
            FishingUI.AddUIObject(useHarp);
            FishingUI.AddUIObject(tunaInvent);
            FishingUI.AddUIObject(herringInvent);
            FishingUI.AddUIObject(swordInvent);
            FishingUI.AddUIObject(whaleInvent);
            FishingUI.AddUIObject(lobsterInvent);

            //create and add inventory buttons    

            TextBox backgroundButton = new TextBox("", new Rectangle(GraphicsDevice.Viewport.Width - 580, -20, 650, 50), buttonImage);
            TextBox orange = new TextBox(new Rectangle(width * 4, (int)Math.Floor(height * 3.5), 250, 50), buttonImage, Inventory.orange);
            TextBox barrel = new TextBox(new Rectangle(width * 4, (int)Math.Floor(height * 6.5), 250, 50), buttonImage, Inventory.oilbarrel);
            InventoryUIManager.AddUIObject(exit);
            InventoryUIManager.AddUIObject(tunaInvent);
            InventoryUIManager.AddUIObject(herringInvent);
            InventoryUIManager.AddUIObject(lobsterInvent);
            InventoryUIManager.AddUIObject(swordInvent);
            InventoryUIManager.AddUIObject(whaleInvent);
            InventoryUIManager.AddUIObject(useNet);
            InventoryUIManager.AddUIObject(useCage);
            InventoryUIManager.AddUIObject(useHarp);
            InventoryUIManager.AddUIObject(orange);
            InventoryUIManager.AddUIObject(barrel);
            InventoryUIManager.AddUIObject(backgroundButton);
            #endregion


            //make temp variables for holding the info for inventory items
            String tempName;
            String tempDescrip;
            int tempNum;
            int tempMax;
            int tempBuy;
            int tempSell;
            List<InventoryItem> shopItems = new List<InventoryItem>();  //list of objects. each island will have this as a random item in their shop

            //load info from the file
            Stream fileStream = File.OpenRead("Content/shopInventory.bin");
            BinaryReader input = new BinaryReader(fileStream);

            //make a loop here
            while (input.PeekChar() != -1)
            {
                tempName = input.ReadString();
                tempDescrip = input.ReadString();
                tempNum = input.ReadInt32();
                tempMax = input.ReadInt32();
                tempBuy = input.ReadInt32();
                tempSell = input.ReadInt32();
                shopItems.Add(new InventoryItem(tempName, tempDescrip, tempNum, tempMax, tempBuy, tempSell));
            }

            #region Adds MapNodes to the MapManager
            //Adding MapNodes to the MapManager
            Random rand = new Random();
            int rows = rand.Next(6, 8);
            int cols = rand.Next(7, 9);
            Stream fishStream = File.OpenRead("Content/FishingSpot.png");
            fishingImage = Texture2D.FromStream(GraphicsDevice, fishStream);
            fishStream.Close();
            fishStream = File.OpenRead("Content/Vortex.png");
            stormImage = Texture2D.FromStream(GraphicsDevice, fishStream);
            fishStream.Close();
            fishStream = File.OpenRead("Content/Pirate.png");
            pirateImage = Texture2D.FromStream(GraphicsDevice, fishStream);
            fishStream.Close();


            for (int i = 1; i < rows; i++)
            {
                for (int j = 1; j < cols; j++)
                {
                    if (j > 1)
                        newRow = false;
                    mapManager.AddNode(newRow, mapImage, islandImages, fishingImage, stormImage, pirateImage, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, shopItems, play);
                }
                newRow = true;
            }
            #endregion


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            #region Get all states
            KeyboardState key = Keyboard.GetState();
            mouseState = Mouse.GetState();
            mousePosition = new Point(mouseState.X, mouseState.Y);
            #endregion

            #region Loading in all things onscreen into objList
            objectList.Clear();
            foreach (Island isl in mapManager.Islands(currentRow, currentCol))
            {
                objectList.Add(isl as Island);
            }
            foreach (FishingSpot fsp in mapManager.FishingSpots(currentRow, currentCol))
            {
                objectList.Add(fsp as FishingSpot);
            }
            foreach (Storm vort in mapManager.Vortexes(currentRow, currentCol))
            {
                if (vort.Available)
                    objectList.Add(vort as Storm);
            }
            foreach (Pirate pete in mapManager.Pirates(currentRow, currentCol))
            {
                if (pete.Available)
                    objectList.Add(pete as Pirate);
            }
            #endregion

            #region TitleMenu GameState
            //check the gamestate
            if (gameState == States.titleMenu)
            {
                title.Play();
                //check if player is moving to the game
                IsMouseVisible = true;
                TitleUIManager.UpdateManager(mouseState, key, gameTime, play, ching);
                List<UI_Item> list = TitleUIManager.List;
                if (list[0].Pressed)
                {
                    gameState = States.map;
                    title.Stop();
                    list[0].Pressed = false;
                }

                if (list[1].Pressed)
                {
                    gameState = States.controls;
                    list[1].Pressed = false;
                }
                /*
                if (key.IsKeyDown(Keys.Enter))
                {
                    //move to the map
                    gameState = States.map;
                }
                */
            }
            #endregion
            #region GameOver GameState
            else if (gameState == States.gameOver)
            {
                surf.Stop();
                //check if player is restarting
                IsMouseVisible = true;
                OverUIManager.UpdateManager(mouseState, key, gameTime, play, ching);
                List<UI_Item> list = OverUIManager.List;
                if (list[0].Pressed)
                {
                    gameState = States.titleMenu;
                    list[0].Pressed = false;
                    ResetGame();
                }
                /*
                if (key.IsKeyDown(Keys.LeftControl) || key.IsKeyDown(Keys.RightControl))
                {
                    ResetGame();
                }
                */
            }
            #endregion
            #region Map GameState
            #region Update player location
            else if (gameState == States.map)
            {
                gameState = play.UpdateLocation(key, objectList, gameState);
                int tempRow = currentRow;
                int tempCol = currentCol;
                //check if player is moving beyond map
                if (play.Rekt.X <= -1)
                {
                    //going to assume we will loop the map for the moment (ie if i go to the farthest right ill pop up back on the farthest left

                    play.XPos = this.GraphicsDevice.Viewport.Width;

                    //check if the player is at a grid edge
                    if (currentCol == 0)
                    {
                        //move player to other side of map
                        currentCol = mapManager.Columns() - 1;
                    }
                    else
                    {
                        //move player to next grid area
                        currentCol -= 1;
                    }
                }
                else if (play.Rekt.Y <= -1)
                {
                    play.YPos = this.GraphicsDevice.Viewport.Height;

                    //check if the player is at a grid edge
                    if (currentRow == 0)
                    {
                        //move player to other side of map
                        currentRow = mapManager.Rows() - 1;
                    }
                    else
                    {
                        //move player to next grid area
                        currentRow -= 1;
                    }
                }
                else if (play.Rekt.X >= GraphicsDevice.Viewport.Width + 1)
                {
                    play.XPos = 0;

                    //check if the player is at a grid edge
                    if (currentCol == mapManager.Columns() - 1)    //if the current column is the last column
                    {
                        //move player to other side of map
                        currentCol = 0;
                    }
                    else
                    {
                        //move player to next grid area
                        currentCol += 1;
                    }
                }
                else if (play.Rekt.Y >= GraphicsDevice.Viewport.Height + 1)
                {
                    play.YPos = 0;

                    //check if the player is at a grid edge
                    if (currentRow == mapManager.Rows() - 1)
                    {
                        //move player to other side of map
                        currentRow = 0;      //double check if 0 refers to column or row
                    }
                    else
                    {
                        //move player to next grid area
                        currentRow += 1;
                    }
                }
                if (tempRow != currentRow || tempCol != currentCol)
                {
                    foreach (Storm vort in mapManager.Vortexes(tempRow, tempCol))
                    {
                        if (!vort.Available)
                        {
                            vort.Available = true;
                        }
                    }
                    foreach (Pirate pete in mapManager.Pirates(tempRow, tempCol))
                    {
                        if (!pete.Available)
                        {
                            pete.Available = true;
                        }
                    }
                }

            #endregion
                #region Move vortexes and pirates
                foreach (Storm vort in mapManager.Vortexes(currentRow, currentCol))
                {
                    vort.Move(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, play);
                }
                foreach (Pirate pete in mapManager.Pirates(currentRow, currentCol))
                {
                    pete.Move(play, new Random());
                }

                #endregion

                #region Check if player is dead
                if (play.Fuel <= 0)
                {
                    //change to game over screen
                    gameState = States.gameOver;
                    dead.Play();

                    //update method of death
                    Death.instance.die = Doom.noFuel;
                }
                else if (play.Health <= 0)
                {
                    //change to game over screen
                    gameState = States.gameOver;
                    dead.Play();

                    //update method of death
                    Death.instance.die = Doom.noHP;
                }
                else if (play.Crew <= 0)
                {
                    //change to game over screen
                    gameState = States.gameOver;
                    dead.Play();

                    //update method of death
                    Death.instance.die = Doom.noCrew;
                }
                #endregion
                // Enters inventory screen
                if (key.IsKeyDown(Keys.I))
                {
                    gameState = States.invent;
                }

                if (surf.State != SoundState.Playing)
                {
                    surf.Play();
                }
            }
            #endregion
            #region Docked GameState
            else if (gameState == States.docked)
            {
                surf.Stop();
                mart.Play();
                bool added = false;
                int width = GraphicsDevice.Viewport.Width / 7; //1600 / 8
                int halfWidth = GraphicsDevice.Viewport.Width / 2;
                int height = GraphicsDevice.Viewport.Height / 10; //900 / 10
                int heightAdj = height / 2;

                if (play.DockedIsland.inventory.ContainsKey("Orange"))
                {
                    Button orange = new Button("Orange : " + Inventory.orange.BuyPrice + " currency per unit", play, new Rectangle(width + halfWidth, height + heightAdj * 6, 370, 25), ShopUImanager.List[0].Texture, ChangeValue.inventoryBuy, Inventory.orange);
                    ShopUImanager.AddUIObject(orange);
                    added = true;
                }
                else if (play.DockedIsland.inventory.ContainsKey("Oil Barrel"))
                {
                    Button oil = new Button("Oil Barrel : " + Inventory.oilbarrel.BuyPrice + " currency per unit", play, new Rectangle(width + halfWidth, height + heightAdj * 6, 370, 25), ShopUImanager.List[0].Texture, ChangeValue.inventoryBuy, Inventory.oilbarrel);
                    ShopUImanager.AddUIObject(oil);
                    added = true;
                }

                ShopUImanager.UpdateManager(mouseState, key, gameTime, play, ching);
                IsMouseVisible = true;
                List<UI_Item> list = ShopUImanager.List;
                if (list[0].Pressed)
                {
                    mart.Stop();
                    gameState = States.map;
                    list[0].Pressed = false;
                    if (added)
                        ShopUImanager.List.RemoveAt(ShopUImanager.List.Count - 1);
                }

                if (key.IsKeyDown(Keys.Tab))
                {
                    mart.Stop();
                    gameState = States.map;
                    if (added)
                        ShopUImanager.List.RemoveAt(ShopUImanager.List.Count - 1);
                }
            }
            #endregion
            #region Fishing Gamestate
            else if (gameState == States.fishing)
            {
                IsMouseVisible = true;
                FishingUI.UpdateManager(mouseState, key, gameTime, play, ching);
                List<UI_Item> list = FishingUI.List;
                Random rando = new Random();
                FishingSpot fish = new FishingSpot(new Rectangle(1, 1, 1, 1), fishingImage);
                if (list[0].Pressed)
                {
                    gameState = States.map;
                    list[0].Pressed = false;
                }

                if (list[1].Pressed)
                {
                    fish.ResolveFishing(play, rando, Inventory.net, mouseState);
                    list[1].Pressed = false;
                }

                if (list[2].Pressed)
                {
                    fish.ResolveFishing(play, rando, Inventory.cage, mouseState);
                    list[2].Pressed = false;
                }

                if (list[3].Pressed)
                {
                    fish.ResolveFishing(play, rando, Inventory.harpoon, mouseState);
                    list[3].Pressed = false;
                }

                if (key.IsKeyDown(Keys.Tab))
                {
                    gameState = States.map;
                }
            }
            #endregion
            #region Controls Gamestate
            else if (gameState == States.controls)
            {
                //check if player is restarting
                IsMouseVisible = true;
                ControlUIManager.UpdateManager(mouseState, key, gameTime, play, ching);
                List<UI_Item> list = ControlUIManager.List;
                if (list[0].Pressed)
                {
                    gameState = States.titleMenu;
                    list[0].Pressed = false;
                }
            }
            #endregion
            #region Inventory Gamestate
            else if (gameState == States.invent)
            {
                IsMouseVisible = true;
                InventoryUIManager.UpdateManager(mouseState, key, gameTime, play, ching);
                List<UI_Item> list = InventoryUIManager.List;
                if (list[0].Pressed)
                {
                    gameState = States.map;
                    list[0].Pressed = false;
                }

                if (key.IsKeyDown(Keys.Tab))
                {
                    gameState = States.map;
                }
            }
            #endregion
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Rectangle screen = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            //check state
            #region States.map gameState: for normal game play
            if (gameState == States.map)    //Sam put your animation code here
            {
                foreach (Button b in ShopUImanager.List.OfType<Button>())
                {
                    b.NumError = false;
                    b.Pressed = false;
                }

                spriteBatch.Draw(mapImage, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

                mapManager.Draw(spriteBatch, currentRow, currentCol, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

                InventoryUIManager.List[InventoryUIManager.List.Count - 1].Draw(spriteBatch, font, GraphicsDevice, Color.White);
                spriteBatch.DrawString(font, "Fuel: " + Math.Truncate(play.Fuel), new Vector2(GraphicsDevice.Viewport.Width - 80, 0), Color.Black);
                spriteBatch.DrawString(font, "Currency: " + play.Currency, new Vector2(GraphicsDevice.Viewport.Width - 200, 0), Color.Black);
                spriteBatch.DrawString(font, "Health: " + play.Health, new Vector2(GraphicsDevice.Viewport.Width - 300, 0), Color.Black);
                spriteBatch.DrawString(font, "Crew: " + play.Crew, new Vector2(GraphicsDevice.Viewport.Width - 390, 0), Color.Black);
                spriteBatch.DrawString(font, "Row: " + currentRow.ToString() + "  Column: " + currentCol.ToString(), new Vector2(GraphicsDevice.Viewport.Width - 550, 0), Color.Black);

                play.Draw(spriteBatch, Color.White);
            }
            #endregion
            #region States.docked gameState: for docking at an island
            else if (gameState == States.docked)
            {
                spriteBatch.Draw(shopMenu, screen, Color.White);
                ShopUImanager.Draw(spriteBatch, font, GraphicsDevice);

                InventoryUIManager.List[InventoryUIManager.List.Count - 1].Draw(spriteBatch, font, GraphicsDevice, Color.White);
                spriteBatch.DrawString(font, "Fuel: " + Math.Truncate(play.Fuel), new Vector2(GraphicsDevice.Viewport.Width - 80, 0), Color.Black);
                spriteBatch.DrawString(font, "Currency: " + play.Currency, new Vector2(GraphicsDevice.Viewport.Width - 200, 0), Color.Black);
                spriteBatch.DrawString(font, "Health: " + play.Health, new Vector2(GraphicsDevice.Viewport.Width - 300, 0), Color.Black);
                spriteBatch.DrawString(font, "Crew: " + play.Crew, new Vector2(GraphicsDevice.Viewport.Width - 390, 0), Color.Black);
                spriteBatch.DrawString(font, "Row: " + currentRow.ToString() + "  Column: " + currentCol.ToString(), new Vector2(GraphicsDevice.Viewport.Width - 550, 0), Color.Black);
            }
            #endregion
            #region States.gameOver gameState: wen u ded
            else if (gameState == States.gameOver)
            {
                //check how you died
                spriteBatch.Draw(gameOverImage, screen, Color.White);

                if (Death.instance.die == Doom.noFuel)
                {
                    spriteBatch.DrawString(font, "You ran out of fuel.  You are now stranded in the ocean", new Vector2(0, 0), Color.Black);
                }
                else if (Death.instance.die == Doom.noHP)
                {
                    spriteBatch.DrawString(font, "You ran out of health and your ship was destroyed", new Vector2(0, 0), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(font, "You were abandoned by your crew", new Vector2(0, 0), Color.Black);
                }

                spriteBatch.DrawString(font, "Game Over", new Vector2(this.GraphicsDevice.Viewport.Width / 2, this.GraphicsDevice.Viewport.Height / 2), Color.Black);
                OverUIManager.Draw(spriteBatch, font, GraphicsDevice);
            }
            #endregion
            #region States.titleMenu gameState: for starting the game
            else if (gameState == States.titleMenu)
            {
                //Draws the background
                spriteBatch.Draw(titleImage, screen, Color.White);
                //Draws the button for the title to be on
                spriteBatch.Draw(buttonImage,
                                 new Rectangle((screen.Width / 2) - (titleText.Width / 4) - 20,
                                               (screen.Height / 2) - (titleText.Height / 2) - 20,
                                               (int)(titleText.Width / 2) + 40,
                                               (int)(titleText.Height / 2) + 40),
                                 Color.White);
                //Draws the title of the game
                spriteBatch.Draw(titleText,
                                 new Rectangle((screen.Width / 2) - (titleText.Width / 4),
                                               (screen.Height / 2) - (titleText.Height / 2),
                                               (int)titleText.Width / 2,
                                               (int)titleText.Height / 2),
                                 Color.White);
                //Draws the buttons
                TitleUIManager.Draw(spriteBatch, font, GraphicsDevice);
            }
            #endregion
            #region States.fishing gameState: for fishing
            else if (gameState == States.fishing)
            {
                spriteBatch.Draw(fishingScreenImage, screen, Color.White);
                FishingUI.Draw(spriteBatch, font, GraphicsDevice);

                InventoryUIManager.List[InventoryUIManager.List.Count - 1].Draw(spriteBatch, font, GraphicsDevice, Color.White);
                spriteBatch.DrawString(font, "Fuel: " + Math.Truncate(play.Fuel), new Vector2(GraphicsDevice.Viewport.Width - 80, 0), Color.Black);
                spriteBatch.DrawString(font, "Currency: " + play.Currency, new Vector2(GraphicsDevice.Viewport.Width - 200, 0), Color.Black);
                spriteBatch.DrawString(font, "Health: " + play.Health, new Vector2(GraphicsDevice.Viewport.Width - 300, 0), Color.Black);
                spriteBatch.DrawString(font, "Crew: " + play.Crew, new Vector2(GraphicsDevice.Viewport.Width - 390, 0), Color.Black);
                spriteBatch.DrawString(font, "Row: " + currentRow.ToString() + "  Column: " + currentCol.ToString(), new Vector2(GraphicsDevice.Viewport.Width - 550, 0), Color.Black);
            }
            #endregion
            #region States.controls gameState: for seeing the controls at the beginning of the game
            else if (gameState == States.controls)
            {
                //Draws the background
                spriteBatch.Draw(titleImage, screen, Color.White);
                //Draws the buttons
                ControlUIManager.Draw(spriteBatch, font, GraphicsDevice);
                //Draws the visuals for the instructions
                spriteBatch.Draw(buttonImage,               //Draws button background
                                 new Rectangle((int)(screen.Width / 4) - (keyImage.Width / 4),
                                               (int)(screen.Height / 4) - (keyImage.Width / 2),
                                                (800),
                                                (400)),
                                 Color.White);
                spriteBatch.Draw(pirateImage,               //Draws pirate ship
                                 new Rectangle((int)(screen.Width / 4) - 15,
                                              (int)(screen.Height / 2) - (keyImage.Height + 100) + (pirateImage.Height / 8),
                                               pirateImage.Width / 2,
                                               pirateImage.Height /2 ),
                                 Color.White);
                spriteBatch.Draw(keyImage,                  //Draws keys
                                 new Rectangle((int)(screen.Width / 2) - (keyImage.Width / 2) - 65,
                                               (int)(screen.Height / 2) - (keyImage.Height + 100),
                                               keyImage.Width,
                                               keyImage.Height),
                                 Color.White);
                spriteBatch.Draw(stormImage,               //Draws vortex
                                 new Rectangle((int)(screen.Width / 1.75f) + (stormImage.Width / 3) + 5,
                                              (int)(screen.Height / 2) - (keyImage.Height + 100) + (stormImage.Height / 8),
                                               pirateImage.Width / 2,
                                               pirateImage.Height / 2),
                                 Color.White);
                spriteBatch.Draw(iKeyImage,                 //Draws the i key
                                 new Rectangle((int)(screen.Width / 2) - (keyImage.Width / 2) + 250,
                                               (int)(screen.Height / 2) - (keyImage.Height + 100) + 45,
                                               iKeyImage.Width,
                                               iKeyImage.Height),
                                 Color.White);
                //Draws the text
                spriteBatch.DrawString(font, "Controls: WASD to move.  Press I to view your inventory.",
                                       new Vector2((int)(screen.Width / 2) - 235,
                                                   (int)(screen.Height / 2) - 80), Color.Black);
                spriteBatch.DrawString(font, "Goal: Catch fish at locations on the map and sell it to make money.\n" +
                                             "                 Don't run out of fuel and watch out for other hazards!",
                                       new Vector2((int)(screen.Width / 2) - 275,
                                                   (int)(screen.Height / 2) - 20), Color.Black);
            }
            #endregion
            #region States.invent gameState: for the inventory
            else if (gameState == States.invent)
            {
                InventoryUIManager.Draw(spriteBatch, font, GraphicsDevice);
                spriteBatch.DrawString(font, "Fuel: " + Math.Truncate(play.Fuel), new Vector2(GraphicsDevice.Viewport.Width - 80, 0), Color.Black);
                spriteBatch.DrawString(font, "Currency: " + play.Currency, new Vector2(GraphicsDevice.Viewport.Width - 200, 0), Color.Black);
                spriteBatch.DrawString(font, "Health: " + play.Health, new Vector2(GraphicsDevice.Viewport.Width - 300, 0), Color.Black);
                spriteBatch.DrawString(font, "Crew: " + play.Crew, new Vector2(GraphicsDevice.Viewport.Width - 390, 0), Color.Black);
                spriteBatch.DrawString(font, "Row: " + currentRow.ToString() + "  Column: " + currentCol.ToString(), new Vector2(GraphicsDevice.Viewport.Width - 550, 0), Color.Black);
            }
            #endregion

            spriteBatch.End();

            base.Draw(gameTime);
        }
        public void ResetGame()
        {
            #region Reset Player
            //Reset player. will update width and length when image is present
            Texture2D temp = play.Image;
            play = new Player("Player", new Rectangle(5, 5, 25, 25));
            play.Image = temp;
            #endregion

            #region Reset Rows / Columns
            currentCol = 0;
            currentRow = 0;
            #endregion

            #region Reset gameState
            //Reset enum
            gameState = States.titleMenu;
            #endregion

            #region Reset objectList
            //Creates an object list to hold the objects currently on screen
            objectList = new List<Object>();
            #endregion

            #region IReset mapManager
            mapManager = new MapManager();
            #endregion

            bool newRow = true;
            List<InventoryItem> shopItems = new List<InventoryItem>();
            #region Adds MapNodes to the MapManager
            //Adding MapNodes to the MapManager
            Random rand = new Random();
            int rows = rand.Next(6, 8);
            int cols = rand.Next(7, 9);
            for (int i = 1; i < rows; i++)
            {
                for (int j = 1; j < cols; j++)
                {
                    if (j > 1)
                        newRow = false;
                    mapManager.AddNode(newRow, mapImage, islandImages, fishingImage, stormImage, pirateImage, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, shopItems, play);
                }
                newRow = true;
            }
            #endregion
        }
    }
}
