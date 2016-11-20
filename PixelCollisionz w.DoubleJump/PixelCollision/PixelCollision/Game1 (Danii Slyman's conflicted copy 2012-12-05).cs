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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        PixelCollision person;
        PixelCollision triangle;
        bool hit = false;

        public Game1()
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
            // TODO: Add your initialization logic here
            person = new PixelCollision(this, new Vector2(100, 100), "Person");
            triangle = new PixelCollision(this, new Vector2(300, 90), "Block");
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

            // TODO: use this.Content to load your game content here
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            KeyboardState keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.Left))
                person.Position.X -= 1;
            if (keys.IsKeyDown(Keys.Right))
                person.Position.X += 1;
            if (keys.IsKeyDown(Keys.Up))
                person.Position.Y += 1;
            if (keys.IsKeyDown(Keys.Down))
                person.Position.Y -= 1;
            hit = false;
            if (PixelIntersect(person.rect, person.data, triangle.rect, triangle.data))
            {
                hit = true;
            }

            person.Update(gameTime);
            triangle.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (hit)
                GraphicsDevice.Clear(Color.Red);
            else
                GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            person.Draw(spriteBatch);
            triangle.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public bool PixelIntersect(Rectangle rectagnleA, Color[] dataA, Rectangle rectangleB, Color[] dataB)
        {
            int top = Math.Max(rectagnleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectagnleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectagnleA.Left, rectangleB.Left);
            int right = Math.Min(rectagnleA.Right, rectangleB.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color colorA = dataA[(x - rectagnleA.Left) + (y - rectagnleA.Top) * rectagnleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) + (y - rectangleB.Top) * rectangleB.Width];

                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
