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
    class TextBox : UI_Item
    {
        private string text;
        private Rectangle rectangle;
        private Texture2D texture;
        private InventoryItem item;

        public TextBox(String text, Rectangle rectangle, Texture2D texture) : base(rectangle, texture)
        {
            this.texture = texture;
            this.text = text;
            this.rectangle = rectangle;
        }

        public TextBox(Rectangle rectangle, Texture2D texture, InventoryItem item) : base(rectangle, texture)
        {
            this.texture = texture;
            text = "";
            this.rectangle = rectangle;
            this.item = item;
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font, GraphicsDevice device, Color color)
        {
            spriteBatch.Draw(texture, rectangle, color);
            spriteBatch.DrawString(font, text, new Vector2(rectangle.X + rectangle.Width / 2 - (text.Length * 4) - 9, rectangle.Y + 1 + rectangle.Height / 10), new Color(Color.Black, .35f));
            spriteBatch.DrawString(font, text, new Vector2(rectangle.X + rectangle.Width / 2 - (text.Length * 4) - 10, (rectangle.Y + 1 + rectangle.Height / 10) - 1), new Color(26 / 255, 15 / 255, 0));
        }

        public void UpdateLabel(Player play)
        {
            if (item == null)
                return;
            if (play.Inventory.GetItemOfType(item.Name) != null)
                text = item.Name + " : " + play.Inventory.GetItemOfType(item.Name).StackNum;
            else
                text = item.Name + " : 0";
        }
    }
}
