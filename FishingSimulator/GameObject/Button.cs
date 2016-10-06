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

namespace BoatGame
{
    enum ChangeValue
    {
        health,
        fuel,
        crew,
        currency,
        inventoryBuy,
        inventorySell
    }

    class Button : UI_Item
    {
        private bool returnValue;
        private bool numError;
        private string label;
        private string userInput;
        private Rectangle rectangle;
        private Texture2D texture;
        private long lastKeyPress;
        private ChangeValue changeValue;
        private float CurrentAmount;
        private InventoryItem inventoryItem;
        private Player play;

        public bool ReturnValue { get { return returnValue; } set { returnValue = value; } }

        public bool NumError { get { return numError; } set { numError = value; } }

        public string UserInput { get { return userInput; } set { userInput = value; } }

        public Button(String label, Player play, Rectangle rectangle, Texture2D texture, ChangeValue changeValue): base(rectangle, texture)
        {
            this.label = label;
            this.rectangle = rectangle;
            returnValue = false;
            this.texture = texture;
            lastKeyPress = 0;
            this.changeValue = changeValue;
            this.play = play;
        }

        public Button(String label, Player play, Rectangle rectangle, Texture2D texture, ChangeValue changeValue, InventoryItem inventoryItem)
            : base(rectangle, texture)
        {
            this.label = label;
            this.rectangle = rectangle;
            returnValue = false;
            this.texture = texture;
            lastKeyPress = 0;
            this.changeValue = changeValue;
            this.inventoryItem = inventoryItem;
            this.play = play;
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font, GraphicsDevice device, Color color)
        {
            Rectangle messageBox = new Rectangle(device.Viewport.Width / 2 - 125, device.Viewport.Height / 2 + 70, 260, 80);
            TextBox box = new TextBox("", messageBox, this.texture);

            spriteBatch.Draw(texture, rectangle, color);
            spriteBatch.DrawString(font, label, new Vector2(rectangle.X + rectangle.Width / 2 - (label.Length * 4) - 9, rectangle.Y + rectangle.Height / 10), new Color(Color.Black, .35f));
            spriteBatch.DrawString(font, label, new Vector2(rectangle.X + rectangle.Width/2 - (label.Length*4) - 10, rectangle.Y + (rectangle.Height / 10) -1), new Color(26/255,15/255,0));
            

            string error = "Insufficient Currency";
            if (Pressed == true)
			{
                string purchase = "Purchase How Much?";
                
                if (changeValue == ChangeValue.fuel)
                {
                    CurrentAmount = (float)Math.Truncate(play.Fuel);
                }
                if (changeValue == ChangeValue.health)
                {
                    CurrentAmount = (float)Math.Truncate(play.Health);
                }
                if (changeValue == ChangeValue.inventoryBuy)
                {
                    if(play.Inventory.GetItemOfType(inventoryItem.Name) != null)
                        CurrentAmount = play.Inventory.GetItemOfType(inventoryItem.Name).StackNum;
                    else
                        CurrentAmount = 0;
                }
                if (changeValue == ChangeValue.inventorySell)
                {
                    if (play.Inventory.GetItemOfType(inventoryItem.Name) != null)
                        CurrentAmount = play.Inventory.GetItemOfType(inventoryItem.Name).StackNum;
                    else
                        CurrentAmount = 0;

                    purchase = "Sell How Much?";
                    error = "Not Enough in Inventory";
                }
                if (changeValue == ChangeValue.crew)
                {
                    CurrentAmount = play.Crew;
                    if(this.label.Contains("Sell"))
                        purchase = "Sell How Much?";
                }
                numError = false;

                box.Draw(spriteBatch, font, device, color);
                
                string have = "You Currently Have : " + CurrentAmount;

                spriteBatch.DrawString(font, purchase, new Vector2(messageBox.X + messageBox.Width / 2 - (purchase.Length * 4) - 14, (messageBox.Y + messageBox.Height / 8) + 1), new Color(Color.Black, .35f));
                spriteBatch.DrawString(font, purchase, new Vector2(messageBox.X + messageBox.Width / 2 - (purchase.Length * 4) - 15, messageBox.Y + messageBox.Height / 8), new Color(26/255,15/255,0));

                spriteBatch.DrawString(font, have, new Vector2(messageBox.X + messageBox.Width / 2 - (have.Length * 4) - 4, messageBox.Y + messageBox.Height / 8 + 41), new Color(Color.Black, .35f));
                spriteBatch.DrawString(font, have, new Vector2(messageBox.X + messageBox.Width / 2 - (have.Length * 4) - 5, messageBox.Y + messageBox.Height / 8 + 40), new Color(26 / 255, 15 / 255, 0));

				try
				{
                    int.Parse(userInput);
				}
				catch (Exception e)
				{ 
                    userInput = "0"; 
                }
                if (int.Parse(userInput) >= 100)
                {
                    userInput = "100";
                }
                userInput = int.Parse(userInput).ToString();

                spriteBatch.DrawString(font, userInput, new Vector2(messageBox.X + messageBox.Width / 2 - (userInput.Length * 4) - 4, messageBox.Y + messageBox.Height / 8 + 21), new Color(Color.Black, .35f));
                spriteBatch.DrawString(font, userInput, new Vector2(messageBox.X + messageBox.Width / 2 - (userInput.Length * 4) - 5, messageBox.Y + messageBox.Height / 8 + 20), new Color(26 / 255, 15 / 255, 0));
			}                
                
            if(numError == true)
                {
                    box.Draw(spriteBatch, font, device, color);
                    spriteBatch.DrawString(font, error, new Vector2(messageBox.X + messageBox.Width / 2 - (error.Length * 4), messageBox.Y + messageBox.Height / 8 + 20), Color.Black);
                    userInput = "0";
                    Pressed = false;
                }
        }

        public string GetUserInput(string input, KeyboardState oldState, GameTime gameTime)
        {

            Keys[] pressedkey = Keyboard.GetState().GetPressedKeys();

            long curr = gameTime.TotalGameTime.Ticks;

            Keys[] userInputKeyList = { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9 };

            if (pressedkey.Length > 0)
            {
                //Console.Write(curr - lastKeyPress + " ");
                Keys key = pressedkey[0];

                if (key == Keys.Back && input.Length > 0 && (curr - lastKeyPress) > 950000)
                {
                    input = input.Remove(input.Length - 1, 1);
                    lastKeyPress = gameTime.TotalGameTime.Ticks;
                }

                else if (key == Keys.Enter)
                {
                    Pressed = false;
                    returnValue = true;
                    return input;
                }

                else if (key == Keys.Space && (curr - lastKeyPress) > 950000)
                {
                    input += " ";
                    lastKeyPress = gameTime.TotalGameTime.Ticks;
                }

                else foreach (Keys key1 in userInputKeyList)
                    {
                        if (key == key1 && (curr - lastKeyPress) > 950000)
                        {
                            input += key1.ToString().Replace("D", "");
                            lastKeyPress = gameTime.TotalGameTime.Ticks;
                        }
                    }
            }
            return input;
        }

        public void UpdatePlayer(Player play, SoundEffectInstance sound)
        {
            sound.Play();
            //fuel
            if (changeValue == ChangeValue.fuel)
            {
                if (play.changeCurrency(int.Parse(this.UserInput) * -1) != -1)
                {
                    this.NumError = true;
                    if (this.UserInput == "0")
                        this.NumError = false;
                }
                else
                {
                    int diff = play.changeFuel(float.Parse(this.UserInput));
                    play.changeCurrency(diff * 1);
                }
                this.ReturnValue = false;
            }

            //health
            if(changeValue == ChangeValue.health)
            {
                if (play.changeCurrency(int.Parse(this.UserInput) * -1) != -1)
                {
                    this.NumError = true;
                    if (this.UserInput == "0")
                        this.NumError = false;
                }
                else
                {
                    int diff = play.changeHealth(int.Parse(this.UserInput));
                    play.changeCurrency(diff * 1);
                }
                this.ReturnValue = false;
            }

            //inventory buy
            if(changeValue == ChangeValue.inventoryBuy)
            {
                if (play.changeCurrency((int.Parse(this.UserInput)*inventoryItem.BuyPrice) * -1) != -1)
                {
                    this.NumError = true;
                    if (this.UserInput == "0")
                        this.NumError = false;
                }
                else
                {
                    int difference = play.Inventory.AddItem(this.inventoryItem, int.Parse(this.UserInput));
                    if(difference != -1)
                        play.changeCurrency(difference * inventoryItem.BuyPrice);
                }
                this.ReturnValue = false;
            }

            //inventory sell
            if(changeValue == ChangeValue.inventorySell)
            {
                int difference = play.Inventory.DecrementItem(this.inventoryItem, int.Parse(this.UserInput));
                play.changeCurrency(int.Parse(this.UserInput) * inventoryItem.SellPrice);
                if (difference != -1)
                {
                    play.changeCurrency((difference * inventoryItem.SellPrice) * -1);
                }
                this.ReturnValue = false;
            }

            //crew
            if(changeValue == ChangeValue.crew)
            {
                if (this.label.Contains("Sell"))
                {
                    int difference = play.changeCrew(int.Parse(this.UserInput) * -1);
                    play.changeCurrency(int.Parse(this.UserInput) * 1);
                    if(difference != -1)
                    {
                        play.changeCurrency(difference * -1);
                        this.NumError = true;
                        if (this.UserInput == "0")
                            this.NumError = false;
                    }
                    this.ReturnValue = false;

                }
                else
                {
                    if (play.changeCurrency(int.Parse(this.UserInput) * -1) != -1)
                    {
                        this.NumError = true;
                        if (this.UserInput == "0")
                            this.NumError = false;
                    }
                    else
                    {
                        play.changeCrew(int.Parse(this.UserInput));
                    }
                    this.ReturnValue = false;
                }
            }
        }
    }
}
