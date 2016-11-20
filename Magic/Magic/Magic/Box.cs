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
    class Box
    {
        public Texture2D Sprite, Bullet;
        Vector2 position = Vector2.Zero;
        Rectangle rect;
        Keys[] directionKeys; 
        KeyboardState keys;
        List<magicShoot> ammo = new List<magicShoot>();
        bool isLeftSide;

        public float Health = 100;

        public Box(ContentManager Content, Vector2 Position)
        {
            position = Position;
            directionKeys = position == Vector2.Zero? new Keys[5] { Keys.W, Keys.S, Keys.D, Keys.A, Keys.LeftShift } :new Keys[5] { Keys.Up, Keys.Down, Keys.Right, Keys.Left, Keys.RightShift};
            if (position == Vector2.Zero)
                isLeftSide = true;
            Sprite = Content.Load<Texture2D>("shooter");
            Bullet = Content.Load<Texture2D>("rectange");
        }
        public void Update(float speed, Box person2)
        {
            rect = new Rectangle((int)position.X, (int)position.Y, Sprite.Width, Sprite.Height);
            keys = Keyboard.GetState();
            if (keys.IsKeyDown(directionKeys[0]))
                position.Y -= 2;
            if (keys.IsKeyDown(directionKeys[1]))
                position.Y += 2; 

            if(keys.IsKeyDown(directionKeys[4]) && ammo.Count < 50)
                ammo.Add(new magicShoot(Bullet, position, keys.IsKeyDown(directionKeys[0]) ? -2 : keys.IsKeyDown(directionKeys[1]) ? 2 : 0));
            
            foreach( magicShoot item in ammo)
                item.Update(speed);

            for (int i = 0; i < ammo.Count; i++)
            {
                if (ammo[i].rect.Intersects(person2.rect))
                    person2.Health -= 1;
                if (ammo[i].isOutside() || ammo[i].rect.Intersects(person2.rect))
                    ammo.RemoveAt(i);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            //spriteBatch.Draw(Sprite, position, Color.Lerp(Color.Transparent, Color.White, Health/100));
            spriteBatch.Draw(Sprite, position, null, Color.Lerp(Color.Transparent, Color.White, Health / 100), 0f, Vector2.Zero, 1, isLeftSide? SpriteEffects.FlipVertically : SpriteEffects.FlipHorizontally, 0f);
            spriteBatch.End();
            foreach (magicShoot item in ammo)
                item.Draw(spriteBatch);
        }




    }
}
