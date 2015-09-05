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


namespace PacMan2
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Hero : Microsoft.Xna.Framework.GameComponent
    {
        SpriteBatch spriteBatch;
        public Texture2D texture;
        public Texture2D texture1;
        public Texture2D texture2;
        public Texture2D texture3;
        public Texture2D pacmanClosed;

        public Rectangle PacRect;
        //public HUD.PlayerLives Lives;
        //public HUD.PlayerScore Score;

        public int Score;
        public int Lives;
        public int PositionX = 0;
        public int PositionY = 0;
        public int moveY;
        public int moveX;
        public int direction=3;
        public bool mouthOpen = true;

        public Hero(Game game,int x,int y)
            : base(game)
        {
            // TODO: Construct any child components here
            //PacRect = new Rectangle(PositionX, PositionY, texture.Width, texture.Height);
            PositionX = x;
            PositionY = y;
            moveY = 0;
            moveX = 0;
            Score = 0;
            Lives = 3;
            

        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            //Lives = new HUD.PlayerLives(this.Game, 3);
            //Score = new HUD.PlayerScore(this.Game, 0);

            base.Initialize();
        }

        public void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            texture = Game.Content.Load<Texture2D>("pacmanUp");
            texture1 = Game.Content.Load<Texture2D>("pacmanDown");
            texture2 = Game.Content.Load<Texture2D>("pacmanLeft");
            texture3 = Game.Content.Load<Texture2D>("pacmanRight");
            pacmanClosed = Game.Content.Load<Texture2D>("pacmanClosed");
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if (gameTime.TotalGameTime.Milliseconds % 500 == 0)
            {
                if (mouthOpen)
                {
                    mouthOpen = false;
                }
                else
                {
                    mouthOpen = true;
                }
            }
            if (PositionX < 0)
                PositionX = 0;
            if (PositionY < 0)
                PositionY = 0;
            if (PositionX + moveX > Game.GraphicsDevice.Viewport.Width)
                PositionX = Game.GraphicsDevice.Viewport.Width - moveX;
            if (PositionY + moveY > Game.GraphicsDevice.Viewport.Height)
                PositionY = Game.GraphicsDevice.Viewport.Height - moveY;

            PacRect = new Rectangle(PositionX, PositionY, texture.Width, texture.Height);

            base.Update(gameTime);
        }
        public void Draw(GameTime gameTime,Color color)
        {
            // draw the ball
            if (texture != null)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                if (mouthOpen)
                {
                    if (direction == 0)
                        spriteBatch.Draw(texture, PacRect, color);
                    if (direction == 1)
                        spriteBatch.Draw(texture1, PacRect, color);
                    if (direction == 2)
                        spriteBatch.Draw(texture2, PacRect, color);
                    if (direction == 3)
                        spriteBatch.Draw(texture3, PacRect, color);
                }
                else
                    spriteBatch.Draw(pacmanClosed, PacRect, color);

                spriteBatch.End();
            }
        }
    }
}
