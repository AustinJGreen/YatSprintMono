using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using YatSprint.Global;

namespace YatSprint.Objects
{
    public class Sky : IObject
    {
        private Texture2D sky;
        private Rectangle first = Rectangle.Empty, second = Rectangle.Empty, window;

        public void ShiftX(float amount)
        {
            first.X -= (int)amount;
            second.X -= (int)amount;
        }

        public void Update(GameTime time)
        {
            if (first == Rectangle.Empty)
                first = window;
            if (second == Rectangle.Empty)
                second = new Rectangle(window.Width, 0, window.Width, window.Height);

            first.X--;
            second.X--;
            if (first.X < -window.Width)
                first.X = second.X + second.Width;
            if (second.X < -window.Width)
                second.X = first.X + first.Width;
        }

        public void UpdateWindow(Rectangle newWindow)
        {
            window = newWindow;
            first = Rectangle.Empty;
            second = Rectangle.Empty;
            //first.Width = newWindow.Width;
            //first.Height = newWindow.Height;
            //second.Width = newWindow.Width;
            //second.Height = newWindow.Height;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(sky, first, Color.White);
            batch.Draw(sky, second, Color.White);
        }

        public void Reset()
        {
        }

        public Sky(Texture2D sky, Rectangle window)
        {
            this.sky = sky;
            this.window = window;
        }
    }
}
