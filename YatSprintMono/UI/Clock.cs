using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace YatSprint.UI
{
    public class Clock : IButton
    {
        private Rectangle Window;
        private SpriteFont font;
        private TimeSpan curTime = TimeSpan.MinValue;
        private TimeSpan initTime = TimeSpan.MinValue;
        private Vector2 Position;
        private Resize resize;
        private Color fontColor;

        public bool RequestScreen(out Request screen)
        {
            screen = null;
            return false;
        }

        public void Update(GameTime time)
        {
            if (initTime == TimeSpan.MinValue)
                initTime = time.TotalGameTime;
            curTime = time.TotalGameTime.Subtract(initTime);
        }

        public void UpdateWindow(Rectangle newWindow)
        {
            Position = resize.GetPosition(newWindow);
            Window = newWindow;
        }

        public void Draw(SpriteBatch batch)
        {
            if (curTime != TimeSpan.MinValue)
            {
                string text = curTime.ToString("hh':'mm':'ss");
                Vector2 size = font.MeasureString(text);
                Vector2 center = new Vector2(Position.X - (size.X / 2), Position.Y - (size.Y / 2));
                batch.DrawString(font, text, center, fontColor);
            }
        }

        public void Reset()
        {
            initTime = TimeSpan.MinValue;
        }

        public Clock(SpriteFont font, Vector2 position, Resize resize, Color fontColor, Rectangle window)
        {
            this.font = font;
            this.Position = position;
            this.resize = resize;
            this.fontColor = fontColor;
            this.Window = window;
        }
    }
}
