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
    class Ball
    {
        public Rectangle Rect;
        public Vector2 Position;
        
        
        private Texture2D sprite;
        private Vector2 velocity = Vector2.Zero;
        private float gravity = 0.005f;
        private bool isFalling = true;

        public Ball(Vector2 position)
        {
            Position = position;
        }

        public void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("ball");
        }

        public void Bounce()
        {
            velocity.Y *= -.7f;
            isFalling = false;
        }

        public bool IsFalling()
        {
            return isFalling;
        }

        public void Update(GameTime gameTime)
        {
            Position.Y = MathHelper.Clamp(Position.Y, 0f, 351f);
            Position.X = MathHelper.Clamp(Position.X, 0f, 750f);
            Rect = new Rectangle((int)Position.X, (int)Position.Y, sprite.Width, sprite.Height);
            float previousVerticalVelocity = velocity.Y;
            UpdateVelocity((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            LeftRight();

            if (previousVerticalVelocity < 0f && velocity.Y >= 0f)
                isFalling = true;
            Position += velocity;
        }

        public void LeftRight()
        {
            KeyboardState keys = Keyboard.GetState();

            if (Position.X >= 750 || Position.X <= 0)
                velocity.X = -velocity.X;
            

            if (keys.IsKeyDown(Keys.Left))
                velocity.X -= .1f;
            else if (keys.IsKeyDown(Keys.Right))
                velocity.X += .1f;
        }

        public void UpdateVelocity(float elapsedTime)
        {
            velocity.Y += gravity * elapsedTime;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(sprite, Position, Color.White);
            spriteBatch.End();
        }
    }
}
