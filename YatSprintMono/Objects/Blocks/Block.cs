using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace YatSprint.Objects.Blocks
{
    public enum BlockType
    {
        Top = 0,
        Middle,
        Bottom
    }
    public class Block : ILevelObject
    {
        private Texture2D texture, ladder;
        private Rectangle Window;
        public Vector2 Size;
        public float X, Y;
        public BlockType Type { get; set; }
        public bool HasLadder { get; set; }

        public Rectangle GetBoundingBox()
        {
            return new Rectangle((int)X, (int)Y, (int)Size.X, (int)Size.Y);
        }

        public void ShiftX(float amount)
        {
            X += amount;
        }

        public void Update(GameTime time)
        {
            
        }

        public void UpdateWindow(Rectangle newWindow)
        {
            Window = newWindow;
        }

        public void Draw(SpriteBatch batch)
        {
            Vector2 positon = new Vector2(X, Y);
            batch.Draw(texture, positon, Color.White);
            if (HasLadder)
                batch.Draw(ladder, positon, Color.White);
        }

        public void Reset()
        {
            
        }

        public bool Dead()
        {
            Rectangle bb = GetBoundingBox();
            return bb.X > Window.Width;
        }

        public Block(Texture2D texture, Texture2D ladder, float x, float y, BlockType type, bool hasladder, Rectangle window)
        {
            this.texture = texture;
            this.ladder = ladder;
            this.Size = new Vector2(texture.Width, texture.Height);
            this.X = x;
            this.Y = y;
            this.Type = type;
            this.HasLadder = hasladder;
            this.Window = window;
        }
    }
}
