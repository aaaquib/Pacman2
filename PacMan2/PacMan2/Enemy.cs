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
    public class Enemy : Microsoft.Xna.Framework.GameComponent
    {
        SpriteBatch spriteBatch;
        public Texture2D texture;
        Texture2D ghostEye;
        Texture2D ghostPupil;

        public int ghostNumber;
        public int PositionX;
        public int PositionY;
        public int moveX;
        public int moveY;
        public int LastDirection=3;
        public bool changeDirection=true;
        public bool gtChange = false;
        public bool isSlow;
        int randomSeed=0;

        public Enemy(Game game,int i)
            : base(game)
        {
            // TODO: Construct any child components here
            ghostNumber = i;
            PositionX = 400+i*10;
            PositionY = 290+i*10;
            moveX = 0;
            moveY = 0;
            LastDirection = -1;
            gtChange = false;
            changeDirection = false;
            isSlow = false;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        public void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            texture = Game.Content.Load<Texture2D>("ghost");
            ghostEye = Game.Content.Load<Texture2D>("Eye");
            ghostPupil = Game.Content.Load<Texture2D>("Pupil");
        }

        public void AnimateGhost()
        {
             //need to select random direction,
             //move in that direction until collision
             //then select a new random direction that isn't the last direction
             //then move in that direction until collision
          
                    if (changeDirection)
                    {
                        changeDirection = false; // makes sure we can still move

                        Random rnd = new Random(randomSeed);
                        // 0 up 1 down 2 left 3 right

                        int randomDirection = rnd.Next(0, 4);

                        if ((LastDirection == 0) || (LastDirection == 1))
                            randomDirection = rnd.Next(2, 4);
                        else
                            randomDirection = rnd.Next(0, 2);
                        if (!isSlow)
                        {
                            if (randomDirection == 0)
                            {
                                moveY = -4;
                                moveX = 0;
                            }
                            if (randomDirection == 1)
                            {
                                moveY = 4;
                                moveX = 0;
                            }
                            if (randomDirection == 2)
                            {
                                moveY = 0;
                                moveX = -4;
                            }
                            if (randomDirection == 3)
                            {
                                moveY = 0;
                                moveX = 4;
                            }
                        }
                        if (isSlow)
                        {
                            if (randomDirection == 0)
                            {
                                moveY = -2;
                                moveX = 0;
                            }
                            if (randomDirection == 1)
                            {
                                moveY = 2;
                                moveX = 0;
                            }
                            if (randomDirection == 2)
                            {
                                moveY = 0;
                                moveX = -2;
                            }
                            if (randomDirection == 3)
                            {
                                moveY = 0;
                                moveX = 2;
                            }
                        }
                        LastDirection = randomDirection;
                    }

                    if (gtChange)           //This is to just change the direction of the ghost randomly
                    {
                        gtChange = false;
                        randomSeed++;
                        Random rndShallWe = new Random(randomSeed);
                        if (rndShallWe.Next(0, 100) > 50)
                        {
                            changeDirection = true;
                        }
                    }
                }

  

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if (randomSeed > 2147483)       // if the Randomseed value is approaching it's maximum, reset it to
                randomSeed = 0;             // prevent crashing! :)
            randomSeed++;           

            int startsecs = 2000;
            int moveSecs;
            
            AnimateGhost();
            
            moveSecs = startsecs + ghostNumber * 100;
            if (gameTime.TotalGameTime.Milliseconds % moveSecs == 0)
                {
                    gtChange = true;
                }

            base.Update(gameTime);
        }
        public void Draw(GameTime gameTime)
        {
           if (texture != null)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                spriteBatch.Draw(texture, new Rectangle(PositionX, PositionY, texture.Width, texture.Height), Color.Blue);
                spriteBatch.Draw(ghostEye, new Rectangle(PositionX + 3, PositionY + 4, ghostEye.Width, ghostEye.Height), Color.White);
                spriteBatch.Draw(ghostEye, new Rectangle(PositionX + 15, PositionY + 4, ghostEye.Width, ghostEye.Height), Color.White);
                if ((LastDirection == 0) || (LastDirection == -1)) // up (or starting)
                {
                    spriteBatch.Draw(ghostPupil, new Rectangle(PositionX + 7, PositionY + 4, ghostPupil.Width, ghostPupil.Height), Color.White);
                    spriteBatch.Draw(ghostPupil, new Rectangle(PositionX + 19, PositionY + 4, ghostPupil.Width, ghostPupil.Height), Color.White);
                }

                if (LastDirection == 1) // down
                {
                    spriteBatch.Draw(ghostPupil, new Rectangle(PositionX + 7, PositionY + 11, ghostPupil.Width, ghostPupil.Height), Color.White);
                    spriteBatch.Draw(ghostPupil, new Rectangle(PositionX + 19, PositionY + 11, ghostPupil.Width, ghostPupil.Height), Color.White);
                }

                if (LastDirection == 2) // left
                {
                    spriteBatch.Draw(ghostPupil, new Rectangle(PositionX + 4, PositionY + 7, ghostPupil.Width, ghostPupil.Height), Color.White);
                    spriteBatch.Draw(ghostPupil, new Rectangle(PositionX + 16, PositionY + 7, ghostPupil.Width, ghostPupil.Height), Color.White);
                }

                if (LastDirection == 3) // right
                {
                    spriteBatch.Draw(ghostPupil, new Rectangle(PositionX + 10, PositionY + 7, ghostPupil.Width, ghostPupil.Height), Color.White);
                    spriteBatch.Draw(ghostPupil, new Rectangle(PositionX + 22, PositionY + 7, ghostPupil.Width, ghostPupil.Height), Color.White);
                }

                spriteBatch.End();
            }
        }
    }
}
