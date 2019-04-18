using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace QuadTreeTest
{
    public class Ball
    {
        public Vector2 position;
        Vector2 direction;
        public Texture2D texture;
        public Vector2 center;
        public float r2;
        public int ColorID = -1;
        public int GenerationID = -1;
        public int ID = -1;

        public Ball(ContentManager content, Vector2 pos, Vector2 dir)
        {
            Load(content);
            position = pos;
            direction = dir;
            center = position + new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            r2 = texture.Width/* * 0.5f*/;
        }
        public Ball(int displaySize, ContentManager content, int id)
        {
            Load(content);
            position = new Vector2(Game1.RNG.Next(displaySize - texture.Width * 2), Game1.RNG.Next(displaySize - texture.Height * 2));
            direction = new Vector2(Game1.RNG.Next(1,2), Game1.RNG.Next(1,2));
            center = position + new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            r2 = texture.Width /** 0.5f*/;
            ID = id;
        }

        void Load(ContentManager content)
        {
            texture = content.Load<Texture2D>("ball");
        }
        public void Update(int displaySize)
        {
            position += direction;
            center = position + new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            if (position.X + texture.Width > displaySize - texture.Width)
            {
                direction.X *= -1;
            }
            else if (position.X <= 0)
            {
                direction.X *= -1;
            }
            else if (position.Y + texture.Height > displaySize - texture.Height)
            {
                direction.Y *= -1;
            }
            else if (position.Y <= 0)
            {
                direction.Y *= -1;
            }
        }
        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.DrawString(font, $"{ID} | {ColorID}", position - new Vector2(0,15), Color.Black);

        }
    }
}
