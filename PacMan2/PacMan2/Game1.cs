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
//using HUD;
using System.Xml;

namespace PacMan2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D gameOver1;
        Texture2D gameOver2;
        Texture2D gameOver3;
        SpriteFont scoreText;
        Texture2D Background;
        //HUD.Timer timer;
        //HUD.Background backGround;
        

        Hero pacman1;
        Hero pacman2;
        Menus menu;
        map map;
        Enemy[] ghost;

        int noGhosts=2;
        int totalGhosts=6;
        int Level;
        bool endGame;
        bool invincibility1;
        bool invincibility2;
        double InvTime;
        double slowTime;

        GamePadState gamePadState;
        GamePadState gamePadState2;

        string filename = "xmlFiles/hudSettings.xml";

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.graphics.PreferredBackBufferWidth = 960;
            this.graphics.PreferredBackBufferHeight = 720;
           
            pacman1 = new Hero(this,760,615);
            pacman2 = new Hero(this,175,30);
            pacman1.direction = 2;
            pacman2.direction = 3;
            map = new map(this);
            menu = new Menus(this);

            

            CreateGhosts(totalGhosts);
            Level = 1; // Reset the level
        }

        void CreateGhosts(int howMany)
        {             
            ghost = new Enemy[howMany];

            for (int i = 0; i < howMany; i++)
            {
                
                ghost[i] = new Enemy(this,i);
                
            }

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
            //hud = new HUD.HUD(this);

            //timer = new Timer(this);
            //backGround = new HUD.Background(this);

            pacman1.Initialize();
            pacman2.Initialize();
            map.Initialize();
            ghost.Initialize();
            menu.Initialize();
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
            scoreText = Content.Load<SpriteFont>("font");
            gameOver1 = Content.Load<Texture2D>("GameOver1");
            gameOver2 = Content.Load<Texture2D>("GameOver2");
            gameOver3 = Content.Load<Texture2D>("GameOver3");
            Background = Content.Load<Texture2D>("level");

            //backGround.LoadBackground(this.Content, "level");

            pacman1.LoadContent();
            pacman2.LoadContent();
            menu.LoadContent();
            map.LoadContent();
            for (int i = 0; i < totalGhosts; i++)
                ghost[i].LoadContent();
                        
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
            gamePadState = GamePad.GetState(PlayerIndex.One);
            gamePadState2 = GamePad.GetState(PlayerIndex.Two);
            
            KeyboardState currentState = Keyboard.GetState();
            Keys[] currentKeys = currentState.GetPressedKeys();

            // Allows the game to exit
            if (gamePadState.Buttons.Back == ButtonState.Pressed || gamePadState.Buttons.Back == ButtonState.Pressed || currentState.IsKeyDown(Keys.Escape))
                this.Exit();
            if (menu.mainMenu == true)
            {
                menu.Update(gameTime);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.P) || gamePadState.Buttons.Start == ButtonState.Pressed)
            {
                menu.mainMenu = false;
                //timer.timer = 0;
                menu.startScreen = true;
                endGame = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.I) || gamePadState.Buttons.X == ButtonState.Pressed)
            {
                menu.menuItem = 2;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A) || gamePadState.Buttons.A == ButtonState.Pressed)
            {
                menu.titleScreen = false;
                menu.mainMenu = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.B) || gamePadState.Buttons.B == ButtonState.Pressed)
            {
                menu.menuItem = 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.H) || gamePadState.Buttons.Y == ButtonState.Pressed)
            {
                menu.ShowTable();
                menu.menuItem = 3;
            }
            if (menu.startScreen == true)
            {
                if (currentState.IsKeyDown(Keys.F1))
                {
                        noGhosts = 2;
                        menu.SaveHighScore(pacman1.Score, pacman2.Score);
                    ResetPacman(pacman1);
                    ResetPacman(pacman2);
                    menu.mainMenu = true;
                    menu.startScreen = false;
                    endGame = true;
                    //pacman1.Lives = 3;
                    //pacman2.Lives = 3;
                    //pacman1.Score = 0;
                    //pacman2.Score = 0;
                    Level = 1;
                    StartGame(0, 0, 1, 3, 3, true);
                }
                if (currentState.IsKeyDown(Keys.F2))
                {
                    noGhosts = 2;
                    ResetPacman(pacman1);
                    ResetPacman(pacman2);
                    //endGame = false;
                    StartGame(0, 0, 1, 3, 3, true);
                    //timer.timer = 0;
                }
                // TODO: Add your update logic here

                if (!endGame)
                {
                    if (gamePadState.ThumbSticks.Left.Y > 0)
                    {
                        pacman1.direction = 0;
                        pacman1.moveY = -5;
                        pacman1.moveX = 0;
                        if (CollisionCheck(pacman1.PositionX, pacman1.PositionY, pacman1.moveX, pacman1.moveY, pacman1.texture, true, gameTime))
                            pacman1.PositionY = pacman1.PositionY + pacman1.moveY;

                    }
                    if (gamePadState.ThumbSticks.Left.Y < 0)
                    {
                        pacman1.direction = 1;
                        pacman1.moveY = 5;
                        pacman1.moveX = 0;
                        if (CollisionCheck(pacman1.PositionX, pacman1.PositionY, pacman1.moveX, pacman1.moveY, pacman1.texture, true, gameTime))
                            pacman1.PositionY = pacman1.PositionY + pacman1.moveY;

                    }
                    if (gamePadState.ThumbSticks.Left.X < 0)
                    {
                        pacman1.direction = 2;
                        pacman1.moveY = 0;
                        pacman1.moveX = -5;
                        if (CollisionCheck(pacman1.PositionX, pacman1.PositionY, pacman1.moveX, pacman1.moveY, pacman1.texture, true, gameTime))
                        {
                            Rectangle tempRect = new Rectangle(pacman1.PositionX, pacman1.PositionY, pacman1.texture.Width, pacman1.texture.Height);
                            if (tempRect.Intersects(map.tunnel1))
                                pacman1.PositionX = map.tunnel2.Left - tempRect.Width - 1;
                            else if (tempRect.Intersects(map.tunnel2))
                                pacman1.PositionX = map.tunnel1.Left + map.tunnel1.Width + 1;
                            else
                                pacman1.PositionX = pacman1.PositionX + pacman1.moveX;
                        }
                    }
                    if (gamePadState.ThumbSticks.Left.X > 0)
                    {
                        pacman1.direction = 3;
                        pacman1.moveY = 0;
                        pacman1.moveX = 5;
                        if (CollisionCheck(pacman1.PositionX, pacman1.PositionY, pacman1.moveX, pacman1.moveY, pacman1.texture, true, gameTime))
                        {
                            Rectangle tempRect = new Rectangle(pacman1.PositionX, pacman1.PositionY, pacman1.texture.Width, pacman1.texture.Height);
                            if (tempRect.Intersects(map.tunnel1))
                                pacman1.PositionX = map.tunnel2.Left - tempRect.Width - 1;
                            else if (tempRect.Intersects(map.tunnel2))
                                pacman1.PositionX = map.tunnel1.Left + map.tunnel1.Width + 1;
                            else
                                pacman1.PositionX = pacman1.PositionX + pacman1.moveX;
                        }
                    }
                    if (gamePadState2.ThumbSticks.Left.Y > 0)
                    {
                        pacman2.direction = 0;
                        pacman2.moveY = -5;
                        pacman2.moveX = 0;
                        if (CollisionCheck(pacman2.PositionX, pacman2.PositionY, pacman2.moveX, pacman2.moveY, pacman2.texture, true, gameTime))
                            pacman2.PositionY = pacman2.PositionY + pacman2.moveY;

                    }
                    if (gamePadState2.ThumbSticks.Left.Y < 0)
                    {
                        pacman2.direction = 1;
                        pacman2.moveY = 5;
                        pacman2.moveX = 0;
                        if (CollisionCheck(pacman2.PositionX, pacman2.PositionY, pacman2.moveX, pacman2.moveY, pacman2.texture, true, gameTime))
                            pacman2.PositionY = pacman2.PositionY + pacman2.moveY;

                    }
                    if (gamePadState2.ThumbSticks.Left.X < 0)
                    {
                        pacman2.direction = 2;
                        pacman2.moveY = 0;
                        pacman2.moveX = -5;
                        if (CollisionCheck(pacman2.PositionX, pacman2.PositionY, pacman2.moveX, pacman2.moveY, pacman2.texture, true, gameTime))
                        {
                            Rectangle tempRect = new Rectangle(pacman2.PositionX, pacman2.PositionY, pacman2.texture.Width, pacman2.texture.Height);
                            if (tempRect.Intersects(map.tunnel1))
                                pacman2.PositionX = map.tunnel2.Left - tempRect.Width - 1;
                            else if (tempRect.Intersects(map.tunnel2))
                                pacman2.PositionX = map.tunnel1.Left + map.tunnel1.Width + 1;
                            else
                                pacman2.PositionX = pacman2.PositionX + pacman2.moveX;
                        }
                    }
                    if (gamePadState2.ThumbSticks.Left.X > 0)
                    {
                        pacman2.direction = 3;
                        pacman2.moveY = 0;
                        pacman2.moveX = 5;
                        if (CollisionCheck(pacman2.PositionX, pacman2.PositionY, pacman2.moveX, pacman2.moveY, pacman2.texture, true, gameTime))
                        {
                            Rectangle tempRect = new Rectangle(pacman2.PositionX, pacman2.PositionY, pacman2.texture.Width, pacman2.texture.Height);
                            if (tempRect.Intersects(map.tunnel1))
                                pacman2.PositionX = map.tunnel2.Left - tempRect.Width - 1;
                            else if (tempRect.Intersects(map.tunnel2))
                                pacman2.PositionX = map.tunnel1.Left + map.tunnel1.Width + 1;
                            else
                                pacman2.PositionX = pacman2.PositionX + pacman2.moveX;
                        }
                    }
                    foreach (Keys keys in currentKeys)
                    {
                        if (keys == Keys.Up || gamePadState.ThumbSticks.Left.Y > 0)
                        {
                            pacman1.direction = 0;
                            pacman1.moveY = -5;
                            pacman1.moveX = 0;
                            if (CollisionCheck(pacman1.PositionX, pacman1.PositionY, pacman1.moveX, pacman1.moveY, pacman1.texture, true, gameTime))
                                pacman1.PositionY = pacman1.PositionY + pacman1.moveY;

                        }
                        if (keys == Keys.Down || gamePadState.ThumbSticks.Left.Y < 0)
                        {
                            pacman1.direction = 1;
                            pacman1.moveY = 5;
                            pacman1.moveX = 0;
                            if (CollisionCheck(pacman1.PositionX, pacman1.PositionY, pacman1.moveX, pacman1.moveY, pacman1.texture, true, gameTime))
                                pacman1.PositionY = pacman1.PositionY + pacman1.moveY;

                        }
                        if (keys == Keys.Left || gamePadState.ThumbSticks.Left.X < 0)
                        {
                            pacman1.direction = 2;
                            pacman1.moveY = 0;
                            pacman1.moveX = -5;
                            if (CollisionCheck(pacman1.PositionX, pacman1.PositionY, pacman1.moveX, pacman1.moveY, pacman1.texture, true, gameTime))
                            {
                                Rectangle tempRect = new Rectangle(pacman1.PositionX, pacman1.PositionY, pacman1.texture.Width, pacman1.texture.Height);
                                if (tempRect.Intersects(map.tunnel1))
                                    pacman1.PositionX = map.tunnel2.Left - tempRect.Width - 1;
                                else if (tempRect.Intersects(map.tunnel2))
                                    pacman1.PositionX = map.tunnel1.Left + map.tunnel1.Width + 1;
                                else
                                    pacman1.PositionX = pacman1.PositionX + pacman1.moveX;
                            }
                        }
                        if (keys == Keys.Right || gamePadState.ThumbSticks.Left.X > 0)
                        {
                            pacman1.direction = 3;
                            pacman1.moveY = 0;
                            pacman1.moveX = 5;
                            if (CollisionCheck(pacman1.PositionX, pacman1.PositionY, pacman1.moveX, pacman1.moveY, pacman1.texture, true, gameTime))
                            {
                                Rectangle tempRect = new Rectangle(pacman1.PositionX, pacman1.PositionY, pacman1.texture.Width, pacman1.texture.Height);
                                if (tempRect.Intersects(map.tunnel1))
                                    pacman1.PositionX = map.tunnel2.Left - tempRect.Width - 1;
                                else if (tempRect.Intersects(map.tunnel2))
                                    pacman1.PositionX = map.tunnel1.Left + map.tunnel1.Width + 1;
                                else
                                    pacman1.PositionX = pacman1.PositionX + pacman1.moveX;
                            }
                        }
                        if (keys == Keys.W || gamePadState2.ThumbSticks.Left.Y > 0)
                        {
                            pacman2.direction = 0;
                            pacman2.moveY = -5;
                            pacman2.moveX = 0;
                            if (CollisionCheck(pacman2.PositionX, pacman2.PositionY, pacman2.moveX, pacman2.moveY, pacman2.texture, true, gameTime))
                                pacman2.PositionY = pacman2.PositionY + pacman2.moveY;

                        }
                        if (keys == Keys.S || gamePadState2.ThumbSticks.Left.Y < 0)
                        {
                            pacman2.direction = 1;
                            pacman2.moveY = 5;
                            pacman2.moveX = 0;
                            if (CollisionCheck(pacman2.PositionX, pacman2.PositionY, pacman2.moveX, pacman2.moveY, pacman2.texture, true, gameTime))
                                pacman2.PositionY = pacman2.PositionY + pacman2.moveY;

                        }
                        if (keys == Keys.A || gamePadState2.ThumbSticks.Left.X < 0)
                        {
                            pacman2.direction = 2;
                            pacman2.moveY = 0;
                            pacman2.moveX = -5;
                            if (CollisionCheck(pacman2.PositionX, pacman2.PositionY, pacman2.moveX, pacman2.moveY, pacman2.texture, true, gameTime))
                            {
                                Rectangle tempRect = new Rectangle(pacman2.PositionX, pacman2.PositionY, pacman2.texture.Width, pacman2.texture.Height);
                                if (tempRect.Intersects(map.tunnel1))
                                    pacman2.PositionX = map.tunnel2.Left - tempRect.Width - 1;
                                else if (tempRect.Intersects(map.tunnel2))
                                    pacman2.PositionX = map.tunnel1.Left + map.tunnel1.Width + 1;
                                else
                                    pacman2.PositionX = pacman2.PositionX + pacman2.moveX;
                            }
                        }
                        if (keys == Keys.D || gamePadState2.ThumbSticks.Left.X > 0)
                        {
                            pacman2.direction = 3;
                            pacman2.moveY = 0;
                            pacman2.moveX = 5;
                            if (CollisionCheck(pacman2.PositionX, pacman2.PositionY, pacman2.moveX, pacman2.moveY, pacman2.texture, true, gameTime))
                            {
                                Rectangle tempRect = new Rectangle(pacman2.PositionX, pacman2.PositionY, pacman2.texture.Width, pacman2.texture.Height);
                                if (tempRect.Intersects(map.tunnel1))
                                    pacman2.PositionX = map.tunnel2.Left - tempRect.Width - 1;
                                else if (tempRect.Intersects(map.tunnel2))
                                    pacman2.PositionX = map.tunnel1.Left + map.tunnel1.Width + 1;
                                else
                                    pacman2.PositionX = pacman2.PositionX + pacman2.moveX;
                            }
                        }


                    }

                    for (int i = 0; i < noGhosts; i++)
                    {
                       if (CollisionCheck(ghost[i].PositionX, ghost[i].PositionY, ghost[i].moveX, ghost[i].moveY, ghost[i].texture, false, gameTime))
                        {
                            Rectangle tempRect = new Rectangle(ghost[i].PositionX, ghost[i].PositionY, ghost[i].texture.Width, ghost[i].texture.Height);
                            if (tempRect.Intersects(map.tunnel1))
                                ghost[i].PositionX = map.tunnel2.Left - tempRect.Width - 1;
                            else if (tempRect.Intersects(map.tunnel2))
                                ghost[i].PositionX = map.tunnel1.Left + map.tunnel1.Width + 1;
                            else
                                ghost[i].PositionX = ghost[i].PositionX + ghost[i].moveX;

                            ghost[i].PositionY = ghost[i].PositionY + ghost[i].moveY;
                        }
                        else
                            ghost[i].changeDirection = true;
                    }
                    for (int i = 0; i < noGhosts; i++)
                    {
                        ghost[i].Update(gameTime);
                    }
                    pacman1.Update(gameTime);
                    pacman2.Update(gameTime);
                    map.Update(gameTime);

                    if ((gameTime.TotalGameTime.TotalMilliseconds - InvTime) >= 9000)
                    {
                        if (invincibility1)
                            invincibility1 = false;
                        if (invincibility2)
                            invincibility2 = false;
                    }

                    if ((gameTime.TotalGameTime.TotalMilliseconds - slowTime) >= 9000)
                    {
                        for (int i = 0; i < noGhosts; i++)
                        {
                            ghost[i].isSlow = false;
                        }
                    }

                }
            }
            
            
            base.Update(gameTime);
        }
        
       public bool CollisionCheck(int CurrentX, int CurrentY, int AddX, int AddY, Texture2D character, bool isHero,GameTime gameTime)
        {
            /* need to check here to see if our character rectangle falls within
             * any of our array-ed rectangles! if it does, return false so the
             * character is unable to move.
             */

            // Also, if the character isn't sprite, then check if we're colliding with the sprite.

            Rectangle tempRect = new Rectangle(CurrentX + AddX, CurrentY + AddY, character.Width, character.Height);

            bool tempReturn = true;

            if (isHero)
            {
                for (int i = 0; i < map.noPts; i++)
                {
                    if (new Rectangle(pacman1.PositionX,pacman1.PositionY,pacman1.texture.Width,pacman1.texture.Height).Intersects(map.pts[i]))
                    {
                            map.pts[i] = new Rectangle(1, 1, 1, 1);
                            pacman1.Score += (Level * 10);
                            map.possiblePoints--;
                            
                                if (map.possiblePoints==0)  // Level complete, we need to advance!
                                {
                                    noGhosts++;
                                    Level++;
                                    map.possiblePoints = map.totalPts;

                                    StartGame(pacman1.Score, pacman2.Score, Level, pacman1.Lives, pacman2.Lives, true); // Advance to next level.
                                    break;
                                }       
                     }
                    if (new Rectangle(pacman2.PositionX, pacman2.PositionY, pacman2.texture.Width, pacman2.texture.Height).Intersects(map.pts[i]))
                    {
                        map.pts[i] = new Rectangle(1, 1, 1, 1);
                        pacman2.Score += (Level * 10);
                        map.possiblePoints--;

                        if (map.possiblePoints == 0)  // Level complete, we need to advance!
                        {
                            noGhosts++;
                            Level++;
                            map.possiblePoints = map.totalPts;

                            StartGame(pacman1.Score, pacman2.Score, Level, pacman1.Lives, pacman2.Lives, true); // Advance to next level.
                            break;
                        }

                        //}
                    }

                }

                if (new Rectangle(pacman1.PositionX, pacman1.PositionY, pacman1.texture.Width, pacman1.texture.Height).Intersects(map.InvRect))
                {
                    invincibility1 = true;
                    invincibility2 = false;
                    map.InvRect = new Rectangle(1, 1, 1, 1);
                    InvTime = gameTime.TotalGameTime.TotalMilliseconds;
                }
                else if (new Rectangle(pacman2.PositionX, pacman2.PositionY, pacman2.texture.Width, pacman2.texture.Height).Intersects(map.InvRect))
                {
                    invincibility2 = true;
                    invincibility1 = false;
                    map.InvRect = new Rectangle(1, 1, 1, 1);
                    InvTime = gameTime.TotalGameTime.TotalMilliseconds;
                }
                if (new Rectangle(pacman2.PositionX, pacman2.PositionY, pacman2.texture.Width, pacman2.texture.Height).Intersects(map.slowRect1) || new Rectangle(pacman1.PositionX, pacman1.PositionY, pacman1.texture.Width, pacman1.texture.Height).Intersects(map.slowRect1))
                {
                    for (int i = 0; i < totalGhosts; i++)
                    {
                        ghost[i].isSlow = true;
                        ghost[i].moveX = 2;
                        ghost[i].moveY = 2;
                    }
                    map.slowRect1 = new Rectangle(1, 1, 1, 1);
                    slowTime = gameTime.TotalGameTime.TotalMilliseconds;
                }
                if (new Rectangle(pacman2.PositionX, pacman2.PositionY, pacman2.texture.Width, pacman2.texture.Height).Intersects(map.slowRect2) || new Rectangle(pacman1.PositionX, pacman1.PositionY, pacman1.texture.Width, pacman1.texture.Height).Intersects(map.slowRect2))
                {
                    for (int i = 0; i < totalGhosts; i++)
                    {
                        ghost[i].isSlow = true;
                        ghost[i].moveX = 2;
                        ghost[i].moveY = 2;
                    }
                    map.slowRect2 = new Rectangle(1, 1, 1, 1);
                    slowTime = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }

            for (int i = 0; i < map.noRects; i++)
            {
                if (tempRect.Intersects(map.rects[i]))
                    tempReturn = false;
            }

            if (!isHero)
            {
                if (!invincibility1)
                {
                    if (tempRect.Intersects(new Rectangle(pacman1.PositionX, pacman1.PositionY, pacman1.texture.Width, pacman1.texture.Height)))
                    {
                        tempReturn = false;
                        
                            pacman1.Lives--;
                            
                            ResetPacman(pacman1);
                            StartGame(pacman1.Score, pacman2.Score, Level, pacman1.Lives, pacman2.Lives, false);
                            
                            if (pacman1.Lives == 0)
                            {
                                endGame = true;
                                menu.SaveHighScore(pacman1.Score, pacman2.Score);
                            }
                        
                    }
                }
                if (!invincibility2)
                {
                    if (tempRect.Intersects(new Rectangle(pacman2.PositionX, pacman2.PositionY, pacman2.texture.Width, pacman2.texture.Height)))
                    {
                        tempReturn = false;
                        
                            pacman2.Lives--;
                           
                            ResetPacman(pacman2);
                            StartGame(pacman1.Score, pacman2.Score, Level, pacman1.Lives, pacman2.Lives, false);
                            
                            if (pacman2.Lives == 0)
                            {
                                endGame = true;
                                menu.SaveHighScore(pacman1.Score, pacman2.Score);
                            }
                        
                    }
                }
            }

            return tempReturn;
        }
        void ResetPacman(Hero player)
        {
            if (player.Equals(pacman1))
            {
           
                pacman1.PositionX = 760;
                pacman1.PositionY = 615;
                pacman1.direction = 2;
                pacman1.moveY = 0;
                pacman1.moveX = 0;
            }
            else if (player.Equals(pacman2))
            {
                
                pacman2.PositionX = 175;
                pacman2.PositionY = 30;
                pacman2.direction = 3;
                pacman2.moveY = 0;
                pacman2.moveX = 0;
            }
        }
        void StartGame(int newScore1,int newScore2, int newLevel, int newLives1, int newLives2, bool killPoints)
        {
            //hud.seconds = 0;
            Level = newLevel;
            pacman1.Score = newScore1;
            pacman1.Lives = newLives1;
            pacman2.Lives = newLives2;
            pacman2.Score = newScore2;
            for (int i = 0; i < noGhosts; i++)
                {
                    ghost[i].PositionX = 400 + i * 10;
                    ghost[i].PositionY = 290 + i * 10;
                    ghost[i].moveX = 5;
                    ghost[i].moveY = 0;
                    ghost[i].LastDirection = -1;
                    ghost[i].gtChange = false;
                    ghost[i].changeDirection = false;
                }
            map.CreateWalls();
            if (killPoints)
                map.CreatePoints();
            map.createSpecialPoints();
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.GhostWhite);
            if (menu.mainMenu == true)
            {
                menu.Draw(gameTime);
            }
            if (menu.titleScreen == true)
            {
                menu.Draw(gameTime);
            }
            if (menu.startScreen == true)
            {


                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                spriteBatch.Draw(Background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                spriteBatch.End();
                //backGround.SetBackGround(this, 0, 0);
                //timer.DisplayTime(gameTime, scoreText, Window.ClientBounds.Width / 2 - 50, Window.ClientBounds.Height - 30, Color.Red);
                //pacman1.Score.showScore(scoreText, 50, 200, Color.White);
                //pacman2.Score.showScore(scoreText, 500, 200, Color.White);

                // TODO: Add your drawing code here
                if (invincibility1 == true)
                    pacman1.Draw(gameTime, Color.Green);
                else
                    pacman1.Draw(gameTime, Color.White);

                if (invincibility2 == true)
                    pacman2.Draw(gameTime, Color.Green);
                else
                {
                    pacman2.Draw(gameTime, Color.White);
                }



                map.Draw(gameTime);
                for (int i = 0; i < noGhosts; i++)
                    ghost[i].Draw(gameTime);

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                spriteBatch.DrawString(scoreText, pacman1.Score.ToString(), new Vector2(60.0f, 110.0f), Color.White);
                spriteBatch.DrawString(scoreText, pacman2.Score.ToString(), new Vector2(900.0f, 110.0f), Color.White);
                for (int i = 0; i < (pacman1.Lives - 1); i++)
                {
                    float tempTop = (float)pacman1.texture.Height * (float)i * 1.3f;
                    spriteBatch.Draw(pacman1.texture, new Rectangle(53, 570 + (int)tempTop, pacman1.texture.Width, pacman1.texture.Height), Color.White);
                }
                for (int i = 0; i < (pacman2.Lives - 1); i++)
                {
                    float tempTop = (float)pacman2.texture.Height * (float)i * 1.3f;
                    spriteBatch.Draw(pacman2.texture, new Rectangle(900, 570 + (int)tempTop, pacman2.texture.Width, pacman2.texture.Height), Color.White);
                }

                spriteBatch.End();

                if (endGame && pacman1.Score > pacman2.Score)
                {

                    if (endGame && pacman1.Lives == 0)
                    {
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                        spriteBatch.Draw(gameOver2, new Rectangle(294, 225, gameOver1.Width, gameOver2.Height), Color.White);
                        spriteBatch.End();

                    }
                    else
                    {
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                        spriteBatch.Draw(gameOver1, new Rectangle(294, 225, gameOver1.Width, gameOver1.Height), Color.White);
                        spriteBatch.End();

                    }


                    if (endGame && pacman2.Score > pacman1.Score)
                    {
                        if (pacman2.Lives == 0)
                        {
                            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                            spriteBatch.Draw(gameOver1, new Rectangle(294, 225, gameOver2.Width, gameOver1.Height), Color.White);
                            spriteBatch.End();
                        }
                        else
                        {
                            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                            spriteBatch.Draw(gameOver2, new Rectangle(294, 225, gameOver2.Width, gameOver2.Height), Color.White);
                            spriteBatch.End();
                        }
                    }
                    else if (endGame && pacman2.Score == pacman1.Score)
                    {

                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                        spriteBatch.Draw(gameOver3, new Rectangle(294, 225, gameOver2.Width, gameOver1.Height), Color.White);
                        spriteBatch.End();
                    }

                }
            }
            
            base.Draw(gameTime);
        }
    }
}
