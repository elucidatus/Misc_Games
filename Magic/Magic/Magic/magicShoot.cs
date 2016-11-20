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

namespace Magic
{
    class magicShoot
    {
        Vector2 position;
        public Rectangle rect;
        Texture2D sprite;
        float ShooterYVelocity;

        public magicShoot(Texture2D Sprite, Vector2 shooterPosition, float shooterYVelocity)
        {
            sprite = Sprite;
            position = shooterPosition;
            ShooterYVelocity = shooterYVelocity;
        }

        public void Update(float speed)
        {
            position.X += speed;
            position.Y += .97f*ShooterYVelocity;
            rect = new Rectangle((int)position.X + sprite.Width/2-5, (int)position.Y + sprite.Height/2 - 5, 10, 10);
            if (position.Y < 0 || position.Y > 600 - rect.Height)
                ShooterYVelocity = -ShooterYVelocity;
 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(sprite, rect, Color.White);
            spriteBatch.End();
        }

        public bool isOutside()
        {
            return position.Y > 600 || position.Y < 0 || position.X > 800 || position.X < 0;
        }
    }
}
