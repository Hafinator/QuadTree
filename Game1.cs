using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace QuadTreeTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        public static Random RNG = new Random();
        public static List<Ball> balls = new List<Ball>();
        const int DISPLAY_SIZE = 500;
        const int STARTING_NUMBER_OF_BALLS = 16;
        KeyboardState PreviousKState;
        SpriteFont font;
        QuadTreeSection quadTreeRoot;
        Texture2D pixel;
        bool pause = false;

        Ball b1, b2, b3, b4;
        int insertID = 0;

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
            graphics.PreferredBackBufferHeight = DISPLAY_SIZE;
            graphics.PreferredBackBufferWidth = DISPLAY_SIZE;

            IsMouseVisible = true;
            graphics.ApplyChanges();

            for (int i = 0; i < STARTING_NUMBER_OF_BALLS; i++)
            {
                balls.Add(new Ball(DISPLAY_SIZE, Content, insertID));
                insertID++;
            }
            //quadTreeRoot = new QuadTreeSection( Vector2.Zero, 0, graphics.GraphicsDevice, DISPLAY_SIZE, DISPLAY_SIZE/*, balls*/);

            //b1 = new Ball(Content, new Vector2(400, 100), new Vector2(0.5f, 0));
            //b2 = new Ball(Content, new Vector2(500, 100), new Vector2(-0.5f, 0));
            //b3 = new Ball(Content, new Vector2(100, 200), new Vector2(5, 0));
            //b4 = new Ball(Content, new Vector2(900, 200), new Vector2(4, 0));
            //balls.Add(b1);
            //balls.Add(b2);
            //balls.Add(b3);
            //balls.Add(b4);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");
            pixel = new Texture2D(graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            quadTreeRoot = null;
            quadTreeRoot = new QuadTreeSection(Vector2.Zero, 0, DISPLAY_SIZE, DISPLAY_SIZE, 0/*, balls*/);
            for (int i = 0; i < balls.Count; i++)
            {
                quadTreeRoot.Insert(balls[i]);
            }
            quadTreeRoot.CheckForColisions(quadTreeRoot, balls);
            if (!pause)
            {
                for (int i = 0; i < balls.Count; i++)
                {
                    balls[i].Update(DISPLAY_SIZE);
                }
            }

            //if (!on)
            //{
            
            //    on = true;
            //}


            if (Keyboard.GetState().IsKeyDown(Keys.Down) && PreviousKState.IsKeyUp(Keys.Down))
            {
                balls.Add(new Ball(DISPLAY_SIZE, Content, insertID));
                insertID++;
                quadTreeRoot.Insert(balls[balls.Count - 1]);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.P) && PreviousKState.IsKeyUp(Keys.P))
            {
                pause = !pause;
            }

            PreviousKState = Keyboard.GetState();
            //FPS = (1 / gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            for (int i = 0; i < balls.Count; i++)
            {
                balls[i].Draw(spriteBatch, font);
            }
            //spriteBatch.DrawString(font, $"balls: {balls.Count.ToString()}", Vector2.Zero, Color.Black);
            //spriteBatch.DrawString(font, $"FPS: {FPS}", new Vector2(0,15), Color.Black);

            //quadTreeRoot.Draw(spriteBatch, font, pixel);

            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
