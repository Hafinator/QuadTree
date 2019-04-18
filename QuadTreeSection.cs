using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuadTreeTest
{
    class QuadTreeSection
    {
        Vector2 position;
        int generation;
        QuadTreeSection[] branches = new QuadTreeSection[4];
        int width, height;
        const int MAX_BALLS = 4;
        Ball[] curentContainedBalls;
        bool devided;
        int numOfBalls;
        public int id;
        bool locked;



        public QuadTreeSection(Vector2 pos, int gen, int w, int h, int i)
        {
            position = pos;
            generation = gen;
            width = w;
            height = h;
            devided = false;
            if (generation == 0)
                curentContainedBalls = new Ball[0];

            curentContainedBalls = new Ball[MAX_BALLS];
            numOfBalls = 0;
            id = i;
            locked = false;
        }


        public bool Insert(Ball ball)
        {

            if (!CheckIfContains(ball))
            {
                return false;
            }
            if ( generation > 0 && !locked && curentContainedBalls[MAX_BALLS - 1] == null )
            {
                if (curentContainedBalls[0] == null)
                    curentContainedBalls[0] = ball;
                else if (curentContainedBalls[1] == null)
                    curentContainedBalls[1] = ball;
                else if (curentContainedBalls[2] == null)
                    curentContainedBalls[2] = ball;
                else
                    curentContainedBalls[3] = ball;
                ball.ColorID = id;
                ball.GenerationID = generation;
                numOfBalls++;
                return true;
            }
            else
            {
                if (!devided)
                {
                    Subdevide(position, generation + 1, width, height/*, curentContainedBalls*/);
                    if (generation > 0)
                    {
                        for (int i = 0; i < curentContainedBalls.Length; i++)
                        {
                            if (!branches[0].Insert(curentContainedBalls[i]))
                                if (!branches[1].Insert(curentContainedBalls[i]))
                                    if (!branches[2].Insert(curentContainedBalls[i]))
                                        branches[3].Insert(curentContainedBalls[i]);
                            curentContainedBalls[i] = null;
                        }
                    }                    
                }
                if (!branches[0].Insert(ball))
                if (!branches[1].Insert(ball))
                if (!branches[2].Insert(ball))
                    branches[3].Insert(ball);
                
            }
            return false;
        }

        bool CheckIfContains(Ball ball)
        {
            if (ball.center.X > position.X &&
                ball.center.X <= position.X + width &&
                ball.center.Y > position.Y &&
                ball.center.Y <= position.Y + height)
                return true;
            else
                return false;
        }

        public void Subdevide(Vector2 pos, int gen, int w, int h/*, List<Ball> balls*/)
        { // TODO test SUBDIVIDE (debug!!!) top left is not the correct top left
            id++;
            branches[0] = new QuadTreeSection(pos, gen, (int)(w * 0.5f), (int)(h * 0.5f), id/*, balls*/);
            id++;
            branches[1] = new QuadTreeSection(new Vector2(pos.X + w * 0.5f, pos.Y), gen, (int)(w * 0.5f), (int)(h * 0.5f), id/*, balls*/);
            id++;
            branches[2] = new QuadTreeSection(new Vector2(pos.X, pos.Y + h * 0.5f), gen, (int)(w * 0.5f), (int)(h * 0.5f), id/*, balls*/);
            id++;
            branches[3] = new QuadTreeSection(new Vector2(pos.X +w * 0.5f, pos.Y + h * 0.5f), gen, (int)(w * 0.5f), (int)(h * 0.5f), id/*, balls*/);
            locked = true;
            devided = true;
        }

        //private void Load(GraphicsDevice graphicsDevice)
        //{
        //    texture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
        //    texture.SetData(new[] { Color.White });
        //}
        public void CheckForColisions(QuadTreeSection branch, List<Ball> balls)
        {
            for (int i = 0; i < branch.branches.Length; i++)
            {
                if (branch.branches[i] != null && branch.branches[i].devided)
                {
                    CheckForColisions(branch.branches[i], balls);
                }
                else
                {
                    for (int j = 0; j < branch.branches[i].curentContainedBalls.Length; j++)
                    {
                        if (branch.branches[i].curentContainedBalls[j] == null)
                            continue;
                        for (int k = j; k < branch.branches[i].curentContainedBalls.Length; k++)
                        {
                            if (branch.branches[i].curentContainedBalls[k] == null || k == j)
                                continue;
                            float temp1, temp2, temp3, temp4, temp5, temp6, temp7, temp8;
                            int id1, id2;
                            id1 = branch.branches[i].curentContainedBalls[j].ID;
                            id2 = branch.branches[i].curentContainedBalls[k].ID;
                            temp1 = branch.branches[i].curentContainedBalls[k].center.X - branch.branches[i].curentContainedBalls[k].r2;
                            temp2 = branch.branches[i].curentContainedBalls[j].center.X + branch.branches[i].curentContainedBalls[j].r2;
                            temp3 = branch.branches[i].curentContainedBalls[k].center.X + branch.branches[i].curentContainedBalls[j].r2;
                            temp4 = branch.branches[i].curentContainedBalls[j].center.X - branch.branches[i].curentContainedBalls[j].r2;
                            temp5 = branch.branches[i].curentContainedBalls[k].center.Y - branch.branches[i].curentContainedBalls[k].r2;
                            temp6 = branch.branches[i].curentContainedBalls[j].center.Y + branch.branches[i].curentContainedBalls[k].r2;
                            temp7 = branch.branches[i].curentContainedBalls[k].center.Y + branch.branches[i].curentContainedBalls[j].r2;
                            temp8 = branch.branches[i].curentContainedBalls[j].center.Y - branch.branches[i].curentContainedBalls[j].r2;
                            if (temp1 > temp2 ||
                                temp3 < temp4 ||
                                temp5 > temp6 ||
                                temp7 < temp8) // Sumim da je krivo to da niso na najvišjem nivoju v drevesu ampak da so še nek uspodej to bo treba vržt vn na veje tm pa da se pregleda
                            {
                                continue;
                            }
                            else
                            {
                                balls.Remove(branch.branches[i].curentContainedBalls[k]);
                                balls.Remove(branch.branches[i].curentContainedBalls[j]);
                            }
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Texture2D texture)
        {

            // Draw top line 
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, width, 1), Color.Black);

            // Draw left line
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, 1, height), Color.Black);

            // Draw right line
            spriteBatch.Draw(texture, new Rectangle((int)position.X + width - 1, (int)position.Y, 1, height), Color.Black);
            // Draw bottom line
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y + height - 1, width, 1), Color.Black);
            if (branches[0] != null)
            {
                foreach (QuadTreeSection branch in branches)
                {
                    branch.Draw(spriteBatch, font, texture);
                }
            }
            //if (!isRoot)
            if (!devided)
                spriteBatch.DrawString(font, $"{id} | {generation}", position + new Vector2(1, 1), Color.Black);

            //branches = null;
            //curentContainedBalls = null;

            //curentContainedBalls = new Ball[MAX_BALLS];
            //devided = false;

        }
    }
}
