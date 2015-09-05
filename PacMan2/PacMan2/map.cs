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
    public class map : Microsoft.Xna.Framework.GameComponent
    {
        SpriteBatch spriteBatch;
        Texture2D wall;
        Texture2D point;
        Texture2D InvPoint;
        Texture2D slowPill;

        public Rectangle[] pts;
        public Rectangle[] rects;
        public Rectangle tunnel1;
        public Rectangle tunnel2;

        public int noRects;
        public int noPts;
        public int possiblePoints;
        public int totalPts;

        public Rectangle InvRect;
        public Rectangle slowRect1;
        public Rectangle slowRect2;


        public map(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            CreateWalls();
            CreatePoints();
            createSpecialPoints();
        }

       public void CreateWalls()
        {
            noRects = 55;
            rects = new Rectangle[noRects];
            rects[0] = new Rectangle(138, 15, 25, 207);
            rects[1] = new Rectangle(200, 66, 78, 42);
            rects[2] = new Rectangle(320, 66, 102, 42);
            rects[3] = new Rectangle(536, 66, 102, 42);
            rects[4] = new Rectangle(680, 66, 78, 42);
            rects[5] = new Rectangle(200, 150, 78, 21);
            rects[6] = new Rectangle(392, 150, 174, 21);
            rects[7] = new Rectangle(680, 150, 78, 21);
            rects[8] = new Rectangle(138, 0, 687, 25);
            rects[9] = new Rectangle(800, 15, 25, 207);
            rects[10] = new Rectangle(464, 15, 30, 93);
            rects[11] = new Rectangle(138, 213, 140, 21);
            rects[12] = new Rectangle(320, 213, 102, 21);
            rects[13] = new Rectangle(536, 213, 102, 21);
            rects[14] = new Rectangle(680, 213, 145, 21);
            rects[15] = new Rectangle(138, 414, 25, 249);
            rects[16] = new Rectangle(800, 414, 25, 249);
            rects[17] = new Rectangle(320, 150, 30, 147);
            rects[18] = new Rectangle(608, 150, 30, 147);
            rects[19] = new Rectangle(464, 150, 30, 84);
            rects[20] = new Rectangle(608, 339, 30, 84);
            rects[21] = new Rectangle(320, 339, 30, 84);
            rects[22] = new Rectangle(138, 654, 687, 25);
            rects[23] = new Rectangle(392, 402, 174, 21);
            rects[24] = new Rectangle(200, 465, 78, 21);
            rects[25] = new Rectangle(680, 465, 78, 21);
            rects[26] = new Rectangle(138, 276, 140, 21);
            rects[27] = new Rectangle(680, 276, 145, 21);
            rects[28] = new Rectangle(138, 339, 140, 21);
            rects[29] = new Rectangle(680, 339, 145, 21);
            rects[30] = new Rectangle(138, 402, 140, 21);
            rects[31] = new Rectangle(680, 402, 145, 21);
            rects[32] = new Rectangle(248, 339, 30, 84);
            rects[33] = new Rectangle(680, 339, 30, 84);
            rects[34] = new Rectangle(248, 213, 30, 84);
            rects[35] = new Rectangle(680, 213, 30, 84);
            rects[36] = new Rectangle(320, 465, 102, 21);
            rects[37] = new Rectangle(536, 465, 102, 21);
            rects[38] = new Rectangle(536, 591, 222, 21);
            rects[39] = new Rectangle(200, 591, 222, 21);
            rects[40] = new Rectangle(392, 528, 174, 21);
            rects[41] = new Rectangle(150, 528, 56, 21);
            rects[42] = new Rectangle(752, 528, 56, 21);
            rects[43] = new Rectangle(248, 469, 30, 80);
            rects[44] = new Rectangle(680, 469, 30, 80);
            rects[45] = new Rectangle(464, 406, 30, 80);
            rects[46] = new Rectangle(464, 532, 30, 80);
            rects[47] = new Rectangle(320, 528, 30, 80);
            rects[48] = new Rectangle(608, 528, 30, 80);
            //rects[49] = new Rectangle(392, 276, 9, 84);
            //rects[50] = new Rectangle(557, 276, 9, 84);
            //rects[51] = new Rectangle(392, 351, 170, 9);
            //rects[52] = new Rectangle(392, 276, 66, 9);
            //rects[53] = new Rectangle(500, 276, 66, 9);
            rects[54] = new Rectangle(1,1,1,1); // this is the door!

            tunnel1 = new Rectangle(200, 297, 10, 42);
            tunnel2 = new Rectangle(750, 297, 10, 42);
            //closedoor = new Rectangle(458, 276, 42, 9);

        }
       public void CreatePoints()
        {
            noPts = 783; // 29 * 27 [grid of points]
            pts = new Rectangle[noPts];
            possiblePoints = 0;
            totalPts = 0;
            int counter = -1;
            int x = 177;
            int y = 42;
            for (int i = 0; i < 29; i++)
            {
                for (int j = 0; j < 27; j++)
                {
                    counter++;

                    Rectangle tempRect = new Rectangle(x + (j * 24), y + (i * 21), 8, 7);

                    bool flag = false;

                    for (int t = 0; t < noRects; t++)
                    {
                        if (tempRect.Intersects(rects[t]))
                            flag = true;
                    }

                    if (!flag)
                    {
                        pts[counter] = tempRect;
                        possiblePoints++;
                    }
                    else
                        counter--;
                }
            }

            // kill some values from the points array:
            Rectangle centerArea1 = new Rectangle(100, 200, 150, 200);
            Rectangle centerArea2 = new Rectangle(700, 200, 960, 200);
            for (int i = 0; i < noPts; i++)
            {
                if (centerArea1.Intersects(pts[i]) || centerArea2.Intersects(pts[i]))
                {
                    pts[i] = new Rectangle(1, 1, 1, 1);
                    possiblePoints--;
                }
            }
            
            totalPts = possiblePoints;
        }
       public void createSpecialPoints()
       {
           InvRect = new Rectangle(470, 310, 20, 20);
           slowRect1 = new Rectangle(365, 565, 15, 15);
           slowRect2 = new Rectangle(580, 185, 15, 15);
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
            wall = Game.Content.Load<Texture2D>("rect");
            point = Game.Content.Load<Texture2D>("point");
            InvPoint = Game.Content.Load<Texture2D>("point");
            slowPill = Game.Content.Load<Texture2D>("point");
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            // draw the walls
            if (wall != null)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                for (int i = 0; i < noRects; i++)
                {
                    spriteBatch.Draw(wall, rects[i], Color.White);
                }
                spriteBatch.Draw(wall, tunnel1, Color.Red);
                spriteBatch.Draw(wall, tunnel2, Color.Red);
                spriteBatch.End();
            }
            if (point != null)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                for (int i = 0; i < noPts; i++)
                {
                    Rectangle tester = new Rectangle(1, 1, 1, 1); // this checks to see if the rectangle should be drawn.
                    if (pts[i] != tester)
                        spriteBatch.Draw(point, pts[i], Color.White); // this is the points.
                }
                spriteBatch.End();
            }
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            spriteBatch.Draw(InvPoint, InvRect , Color.Green);
            spriteBatch.Draw(slowPill, slowRect1, Color.Red);
            spriteBatch.Draw(slowPill, slowRect2, Color.Red);
            spriteBatch.End();
        }
    }
}
