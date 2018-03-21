using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YatSprint.Objects;

namespace YatSprint.UI
{
    public enum AmountType
    {
        Percentage,
        Amount,
        AmountAndTotal
    }
    public abstract class Progressbar : IButton
    {
        public Texture2D cage, health;

        protected SpriteFont font;

        protected AmountType amountType;

        protected Resize resize;

        protected int margin;

        protected int maxValue;

        protected int startValue;

        protected int Value;

        public Vector2 Position;

        protected Yat yat;

        public Rectangle GetBoundingBox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, cage.Width, cage.Height);
        }

        public bool RequestScreen(out Request screen)
        {
            screen = null;
            return false;
        }

        public virtual void Update(GameTime time)
        {
            
        }

        public void UpdateWindow(Rectangle newWindow)
        {
            Position = resize.GetPosition(newWindow);
        }

        public void Draw(SpriteBatch batch)
        {
            float percent = Value / (float)maxValue;
            Rectangle source = new Rectangle(0, 0, (int)(health.Width * percent), health.Height);
            batch.Draw(cage, Position, Color.White);

            Vector2 healthPos = new Vector2(Position.X + margin, Position.Y + margin);
            batch.Draw(health, healthPos, source, Color.White);

            string text;

            switch (amountType)
            {
                case AmountType.Percentage: default:
                    int expressed = (int)Math.Round(percent * 100, 0);
                    text = expressed.ToString() + "%";
                    break;
                case AmountType.AmountAndTotal:
                    text = string.Format("{0}/{1}", Value, maxValue);
                    break;
                case AmountType.Amount:
                    text = Value.ToString();
                    break;
            }
            Vector2 size = font.MeasureString(text);
            Vector2 normal = new Vector2(Position.X + ((cage.Width / 2) - (size.X / 2)), Position.Y + ((cage.Height / 2) - (size.Y / 2)));
            batch.DrawString(font, text, normal, Color.Black);
        }

        public void Reset()
        {
            this.Value = startValue;
        }
    }
}
