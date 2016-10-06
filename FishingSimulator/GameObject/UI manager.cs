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
    class UI_manager
    {
        private List<UI_Item> list;
        private MouseState last;

        public List<UI_Item> List { get { return list; } }
        
        public UI_manager()
        {
            list = new List<UI_Item>();   
        }

        //add objects to UI object list
        public void AddUIObject(UI_Item item)
        {
            list.Add(item);
        }
        
        //update mousestate
        //check for collisions with mouse and buttons
        public void UpdateManager(MouseState mouseState, KeyboardState key, GameTime gameTime, Player play, SoundEffectInstance sound)
        {
            Point mousePosition = new Point(mouseState.X, mouseState.Y);

            foreach (Button button in list.OfType<Button>())
            {
                if (button.Rectangle.Contains(mousePosition) && mouseState.LeftButton == ButtonState.Pressed && button.Pressed == false)
                {
                    button.Pressed = true;
                }

                if (button.Pressed == true)
                {
                    button.UserInput = button.GetUserInput(button.UserInput, key, gameTime);
                    
                    
                    foreach (Button button2 in list.OfType<Button>())
                    {
                        button2.Pressed = false;
                        button2.NumError = false;
                    }
                    

                    button.Pressed = true;
                }

                if (button.ReturnValue == true)
                {
                    button.UpdatePlayer(play, sound);
                    button.Pressed = false;
                    button.UserInput = "0";
                }
            }

            foreach (TextBox text in List.OfType<TextBox>())
            {
                text.UpdateLabel(play);
                if(text.Rectangle.Contains(mousePosition) && mouseState.LeftButton == ButtonState.Pressed && text.Pressed == false && last.LeftButton == ButtonState.Released)
                {
                    text.Pressed = true;
                }
            }

            last = mouseState;
        }

        //draw all UI elements in this manager
        public void Draw(SpriteBatch spriteBatch, SpriteFont font, GraphicsDevice device)
        {
            device.Clear(new Color(204, 195, 122));

            //draw all UI items
            foreach(UI_Item item in list)
            {
                item.Draw(spriteBatch, font, device, Color.White);
            }
        }
        
    }
}
