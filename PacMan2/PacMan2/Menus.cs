using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;

namespace PacMan2
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Menus : Microsoft.Xna.Framework.GameComponent
    {
        SpriteBatch spriteBatch;
        SpriteFont font;
        SpriteFont font1;
        SpriteFont font2;
        public String play;
        public String highscores;
        public String insturctions;
        public String back;
        public Boolean mainMenu = false;
        public Boolean titleScreen = true;
        public Boolean startScreen = false;
        public int menuItem = 1;
        int[] scoreArray = new int[10];

        public Menus(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            
            play = "[P]LAY";
            insturctions = "[I]NSTRUCTIONS";
            highscores = "[H]IGHSCORES";
            back = "[B]ACK";
            base.Initialize();
        }

        public void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            font = Game.Content.Load<SpriteFont>("MenuFont");
            font1 = Game.Content.Load<SpriteFont>("TitleFont");
            font2 = Game.Content.Load<SpriteFont>("PacmanTitle");
        }

        public void SaveHighScore(int Score1, int Score2)
        {
            String line1 = Score1.ToString(); 
            String line2 = Score2.ToString();
            FileStream highscore = File.Open("Content/Highscores.txt", FileMode.OpenOrCreate, FileAccess.Write);
           
            StreamWriter swriter = new StreamWriter(highscore);
            swriter.WriteLine(Score1);
            swriter.WriteLine(Score2);
            swriter.Close();
        }
        public void ShowTable()
        {
            string score;
            FileStream highscore = File.Open("Content/Highscores.txt", FileMode.Open, FileAccess.Read);
  
            StreamReader sreader = new StreamReader(highscore);
            int i = 0;
            while (!sreader.EndOfStream && i < 10)
            {
                score = sreader.ReadLine();
                scoreArray[i] = Convert.ToInt32(score);
                i++;
            }
            sreader.Close();
            int j, tmp;
            i = 0;

            for (i = 1; i < scoreArray.Length; i++)
            {
                j = i;

                while (j > 0 && scoreArray[j - 1] < scoreArray[j])
                {

                    tmp = scoreArray[j];

                    scoreArray[j] = scoreArray[j - 1];

                    scoreArray[j - 1] = tmp;

                    j--;

                }

            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public void Draw(GameTime gameTime)
        {
            if (mainMenu == true)
            {
                if (menuItem == 1)
                {
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, play, new Vector2(300, 350), Color.Red, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.DrawString(font, insturctions, new Vector2(300, 380), Color.Black, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.DrawString(font, highscores, new Vector2(300, 410), Color.Black, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.End();
                }
                if (menuItem == 2)
                {
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, "Player 1 Controls :", new Vector2(160, 150), Color.Red, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.DrawString(font, "Press UP arrow      : Move UP", new Vector2(160, 180), Color.Black, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.DrawString(font, "        Down arrow : Move Down", new Vector2(160, 200), Color.Black, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.DrawString(font, "        left arrow    : Move Left", new Vector2(160, 220), Color.Black, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.DrawString(font, "        Right arrow  : Move Down", new Vector2(160, 240), Color.Black, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.DrawString(font, "Player 2 Controls :", new Vector2(160, 300), Color.Red, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.DrawString(font, "Press W arrow   : Move UP", new Vector2(160, 330), Color.Black, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.DrawString(font, "        S arrow     : Move Down", new Vector2(160, 350), Color.Black, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.DrawString(font, "        A arrow    : Move Left", new Vector2(160, 370), Color.Black, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.DrawString(font, "        D arrow    : Move Down", new Vector2(160, 390), Color.Black, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.DrawString(font, back, new Vector2(140, 650), Color.Black, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.End();
                }
                if (menuItem == 3)
                {

                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, "High Scores", new Vector2(Game.Window.ClientBounds.Width / 2 - 100, 30), Color.Red, 0, new Vector2(0, 0), 2.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.DrawString(font, back, new Vector2(140, 650), Color.Black, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                    
                    for (int i = 0; i < 10 && scoreArray[i] != null; i++)
                    {
                        spriteBatch.DrawString(font1, scoreArray[i].ToString(), new Vector2(Game.Window.ClientBounds.Width / 2 - 10, (Game.Window.ClientBounds.Width / 2 - 300) + i * 20), Color.Red, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                    }
                    spriteBatch.End();
                }
            }

            if (titleScreen == true)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font2, "PAC MAN", new Vector2(150, 150), Color.DarkGoldenrod, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.DrawString(font1, "PRESS (A) to Start the Game", new Vector2(350, 450), Color.Black, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.End();
            }
            
        }
    }
}
