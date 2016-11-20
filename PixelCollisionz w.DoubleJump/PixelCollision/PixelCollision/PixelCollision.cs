using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PixelCollision
{
    class PixelCollision
    {
        public Texture2D[] Sprites;
        public Color[] Data;
        public Vector2 Position;
        public Rectangle Rect;

        private int spriteIndex = 0;

        private int timeCounter = 0;

        public PixelCollision(Game game, Vector2 position, string[] fileNames)
        {
            Position = position;

            Sprites = new Texture2D[fileNames.Length];

            for (int i = 0; i < fileNames.Length; i++)
                Sprites[i] = game.Content.Load<Texture2D>(fileNames[i]);

            Data = new Color[Sprites[0].Width * Sprites[0].Height];
            Sprites[0].GetData(Data);
        }

        public void Update(GameTime gameTime)
        {
            Rect = new Rectangle((int)Position.X, (int)Position.Y, Sprites[0].Width, Sprites[0].Height);

            if (timeCounter % 30 == 0)
                spriteIndex = spriteIndex < Sprites.Length - 1 ? spriteIndex + 1 : 0;

            timeCounter++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Sprites[spriteIndex], Position, Color.White);
            spriteBatch.End();
        }
    }
}
