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
using System.Diagnostics;
namespace PixelCollision
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        bool DoubleJumpCheck = false;
        bool jetPack = false;
        bool isDoubleJumping = false;
        bool wasUpKey = false;
        bool jumpCheck = false;
        byte wasLowHealth;

        PixelCollision Flycon;
        Random rand = new Random();
        Texture2D flycon;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D Protection;
        PixelCollision person;
        PixelCollision triangle;
        PixelCollision[] obstacles;

        bool isHit = false;
        bool wasHit = false;
        int mAxflycon = 10;
        int life = 10;

        Texture2D lifeBar;
        SpriteFont warning;

        const float minScreenSpeed = 2f;
        const float maxScreenSpeed = 20f;
        const float screenAcceleration = 0.05f;

        float screenSpeed = minScreenSpeed;

        bool isJumping = false;

        Vector2 personInitialPosition = new Vector2(250, 368);

        const float gravity = 280f;
        const float initialVerticalVelocity = 250f;
        float verticalVelocity = initialVerticalVelocity;

        bool isPause = false;
        bool wasKeyDown = false;
        bool doneJumping = true;

        SpriteFont gameFont;

        int score = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Flycon = new PixelCollision(this, new Vector2(400, 100), new string[] { "flycon" });
            person = new PixelCollision(this, personInitialPosition, new string[] { "person3", "Person2" });
            triangle = new PixelCollision(this, new Vector2(300, 90), new string[] { "flycon" });
            obstacles = new PixelCollision[1000];
            Vector2 jumpe = new Vector2(260, 478);
            for (int i = 0; i < obstacles.Length; i++)
                obstacles[i] = new PixelCollision(this, new Vector2(i * 300, 368), new string[] { "Person1" });

            base.Initialize();
        }

        protected override void LoadContent()
        {

            {
                // TODO: Load any ResourceManagementMode.Automatic content

                // TODO: Load any ResourceManagementMode.Manual content
            }

            spriteBatch = new SpriteBatch(GraphicsDevice);
            Protection = this.Content.Load<Texture2D>("Defense");
            gameFont = this.Content.Load<SpriteFont>("GameFont");
            lifeBar = this.Content.Load<Texture2D>("hearty");
            flycon = this.Content.Load<Texture2D>("flycon");
            warning = this.Content.Load<SpriteFont>("SpriteFont1");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void CheckPauseScreen(KeyboardState keys)
        {
            bool isKeyDown = keys.IsKeyDown(Keys.Space);

            if (!wasKeyDown && isKeyDown)
                isPause = !isPause;

            wasKeyDown = isKeyDown;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            KeyboardState keys = Keyboard.GetState();

            CheckPauseScreen(keys);


            person.Position.Y = MathHelper.Clamp(person.Position.Y, 0f, personInitialPosition.Y);
            Debug.WriteLine(person.Position.Y);

            if (!isPause)
            {
                float elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                //if (person.Position.Y < 0)
                //{
                //    person.Position.Y = 0;
                //}
                
                if (isAlive())
                {
                    if (screenSpeed < maxScreenSpeed)
                        screenSpeed += screenAcceleration * elapsedTime / 1000f;

                    if (!isJumping && keys.IsKeyDown(Keys.Up))
                    {
                        isJumping = true;
                        jumpCheck = true;
                    }
                    if (!jetPack && verticalVelocity <= 0)
                        jetPack = true;
                    if (keys.IsKeyDown(Keys.Up) && verticalVelocity <=0)
                    {
                        //verticalVelocity -= -gravity * elapsedTime / 1000f;
                        //person.Position.Y += verticalVelocity * elapsedTime / 1000f;
                        person.Position.Y -= 1;//elapsedTime / 1000f;
                        verticalVelocity = 1;
                        //verticalVelocity = verticalVelocity - 1 * verticalVelocity*elapsedTime ; 
                        //person.Position.Y -= verticalVelocity * elapsedTime / 1000f;
                                               
                    }

                    if (doneJumping && isJumping)
                    {
                        verticalVelocity -= gravity * elapsedTime / 1000f;
                        person.Position.Y -= verticalVelocity * elapsedTime / 1000f;
                    }

                    if (DoubleJumpCheck)
                    {
                        verticalVelocity = initialVerticalVelocity;
                        DoubleJumpCheck = false;
                    }

                    if (keys.IsKeyDown(Keys.Up) && (isJumping && person.Position.Y > personInitialPosition.Y))
                        doneJumping = false;
                    //else if (keys.IsKeyUp(Keys.Up))
                    //    doneJumping = true;

                    if (isJumping && person.Position.Y < personInitialPosition.Y && !isDoubleJumping && keys.IsKeyDown(Keys.Up) && !jumpCheck && !wasUpKey)
                    {
                        isDoubleJumping = true;
                        DoubleJumpCheck = true;
                    }

                    jumpCheck = false;

                    if (isJumping && person.Position.Y > personInitialPosition.Y)
                    {
                        ResetJumpData();
                    }

                    isHit = false;

                    wasUpKey = keys.IsKeyDown(Keys.Up);

                    person.Update(gameTime);

                    foreach (PixelCollision obstacle in obstacles)
                    {
                        obstacle.Update(gameTime);
                        if (PixelIntersect(person.Rect, person.Data, obstacle.Rect, obstacle.Data))
                        {
                            isHit = true;
                        }
                    }

                    if (!wasHit && isHit)
                    {
                        life--;

                        screenSpeed *= 0.90f;

                        if (screenSpeed < minScreenSpeed)
                            screenSpeed = minScreenSpeed;
                    }

                    foreach (PixelCollision obstacle in obstacles)
                    {
                        obstacle.Position.X -= screenSpeed;
                    }

                    wasHit = isHit;

                    score += (int)screenSpeed;
                }
            }

            base.Update(gameTime);
        }

        private bool Tap()
        {
            return mAxflycon > 0;
        }
        private bool isAlive()
        {
            return life > 0;
        }

        private void ResetJumpData()
        {
            isJumping = false;
            isDoubleJumping = false;
            //person.Position = personInitialPosition;
            verticalVelocity = initialVerticalVelocity;
            jetPack = false;
            doneJumping = true;
        }

        protected override void Draw(GameTime gameTime)
        {
            {
                GraphicsDevice.Clear(isAlive() ? (isPause ? Color.Blue : (isHit ? Color.Firebrick : Color.MediumSpringGreen)) : Color.Purple);

                person.Draw(spriteBatch);

                foreach (PixelCollision obstacle in obstacles)
                    obstacle.Draw(spriteBatch);

                DrawLifeBar(spriteBatch);

                DrawScoreReport(spriteBatch);

                base.Draw(gameTime);
            }
        }

        private void DrawLifeBar(SpriteBatch spriteBatch)
        {
            if (isAlive())
            {
                spriteBatch.Begin();
                for (int i = 0; i < life; i++)

                    spriteBatch.Draw(lifeBar, new Vector2(i * 40 + 20, 20), Color.White);
                spriteBatch.End();
            }
        }
        private void DrawJetPack(SpriteBatch spritebatch)
        {
            if (Tap())
            {
                spritebatch.Begin();
                for (int i = 0; i < mAxflycon; i++)
                    spritebatch.Draw(flycon, new Vector2(300, 200), Color.White);
                spritebatch.End();
            }
        }

        private void DrawScoreReport(SpriteBatch spriteBatch)
        {
            if (life<=3 && wasLowHealth > 3)
            {
                isPause = true;
            }
            wasLowHealth = (byte)life;
            if (life <= 3)
            {
                spriteBatch.Begin();
                if(!isAlive())
                    spriteBatch.DrawString(warning, "WARNING!!! " + life.ToString(), new Vector2(500, 500), Color.Orange);
                else
                    spriteBatch.DrawString(warning, "WARNING!!! " + life.ToString(), new Vector2(500, 500), Color.DarkViolet);
                spriteBatch.End();
            }
            if (!isAlive())
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(gameFont, "Score: " + score.ToString(), new Vector2(60, 20), Color.Red);
                spriteBatch.End();
            }

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