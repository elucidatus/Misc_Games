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

namespace PixelCollision
{
    class ShieldClass 
    {
        Texture2D Sprite;
        public Texture2D shield;
        Game1 Switch;
        public ShieldClass (ContentManager content)
        {
            Sprite = content.Load<Texture2D>("shield");
        }
        public void Update(GameTime gametime, Vector2 personPosition)
        {
            
        }
        public void Draw()
        {
            
        }
    }
}
