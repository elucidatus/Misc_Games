using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PhysicsBounce
{
    class Floor
    {
        Vector2 Position;
        public Rectangle Rect;
        Texture2D Sprite;

        public Floor(Vector2 position)
        {
            Position = position;
        }

        public void LoadContent(ContentManager content)
        {
            Sprite = content.Load<Texture2D>("floor");
        }

        public void Update(GameTime gameTime)
        {
            Rect = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Sprite, Position, Color.Red);
            spriteBatch.End();
        }
    }
}
